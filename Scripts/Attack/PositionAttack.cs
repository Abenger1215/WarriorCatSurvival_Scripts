using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionAttack : MonoBehaviour
{
    protected string PrefabName;
    public Rigidbody2D rb;
    protected Player player;
    protected Coroutine timeCoroutine;

    protected void Awake()
    {
        PrefabName = gameObject.name.Split('_')[0];

        rb = GetComponent<Rigidbody2D>();
        player = Player.instance;
    }

    public void ActiveAttack(Vector3 target)
    {
        gameObject.transform.position = target;
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
