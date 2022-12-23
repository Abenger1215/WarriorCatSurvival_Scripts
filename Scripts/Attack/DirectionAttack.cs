using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionAttack : MonoBehaviour
{
    public float MoveSpeed = 15.0f;

    protected string PrefabName;
    public Vector3 dir;
    public Rigidbody2D rb;
    protected Player player;
    protected Coroutine timeCoroutine;

    protected void Awake()
    {
        PrefabName = gameObject.name.Split('_')[0];
        player = Player.instance;
    }

    protected void FixedUpdate()
    {
        transform.Translate(MoveSpeed * Time.deltaTime * dir, Space.World);
    }

    public void SetDir(Vector3 target)
    {
        dir = (target - transform.position).normalized;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(dir.y, dir.x) + 180);
    }

    protected void ReturnPool()
    {
        AttackManager.instance.ObjPools[PrefabName].Push(gameObject);
        gameObject.SetActive(false);
    }

    protected IEnumerator DisableByTime(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        //Debug.Log("DisableByTime / " + AM.ObjPools[PrefabName].Count + " / " + gameObject.name);
        ReturnPool();
    }
}
