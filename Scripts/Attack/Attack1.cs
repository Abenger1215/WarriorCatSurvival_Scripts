// 단방향 공격
// 실뭉치 - string ball
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack1 : DirectionAttack
{
    private void OnEnable()
    {
        timeCoroutine = StartCoroutine(DisableByTime(AttackManager.instance.attack1Data.duration));
        SoundManager.instance.PlaySFXSound("Attack1", 0.2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MONSTER"))
        {
            collision.gameObject.GetComponent<Monster>().Damaged(AttackManager.instance.attack1Data.damage);
            StopCoroutine(timeCoroutine);
            ReturnPool();
            //Debug.Log("disable by trigger / " + AM.ObjPools[PrefabName].Count + " / " + gameObject.name);
        }
    }
}
