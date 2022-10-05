using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack1 : MonoBehaviour
{
    public float MoveSpeed;

    private string PrefabName;
    public Vector3 dir;
    public Rigidbody2D rb;
    private GameObject player;
    private Coroutine timeCoroutine;

    private void Start()
    {
        MoveSpeed = 15.0f;
        PrefabName = gameObject.name.Split('_')[0];

        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("PLAYER");
    }

    private void OnEnable()
    {
        timeCoroutine = StartCoroutine(DisableByTime(3.0f));
        SoundManager.instance.PlaySFXSound("Attack1", 0.4f);
    }

    private void Update()
    {
        transform.Translate(MoveSpeed * Time.deltaTime * dir);
    }

    public void SetDir(Vector3 target)
    {
        dir = (target - transform.position).normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MONSTER"))
        {
            collision.gameObject.GetComponent<MonsterCtrl>().Damaged(AttackManager.instance.Atk1Damage);
            StopCoroutine(timeCoroutine);
            ReturnPool();
            //Debug.Log("disable by trigger / " + AM.ObjPools[PrefabName].Count + " / " + gameObject.name);
        }
    }

    void ReturnPool()
    {
        AttackManager.instance.ObjPools[PrefabName].Push(gameObject);
        gameObject.SetActive(false);
    }

    IEnumerator DisableByTime(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        //Debug.Log("DisableByTime / " + AM.ObjPools[PrefabName].Count + " / " + gameObject.name);
        ReturnPool();
    }
}
