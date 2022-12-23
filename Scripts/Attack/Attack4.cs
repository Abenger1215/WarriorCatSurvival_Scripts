// 관통, 아래로 떨어지는 공격
// 쇠구슬 - iron beed

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack4 : DirectionAttack
{

    private void OnEnable()
    {
        MoveSpeed = 5f;
        timeCoroutine = StartCoroutine(DisableByTime(AttackManager.instance.attack4Data.duration));
        SoundManager.instance.PlaySFXSound("Attack1", 0.2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MONSTER"))
        {
            collision.gameObject.GetComponent<Monster>().Damaged(AttackManager.instance.attack4Data.damage);
            //Debug.Log("disable by trigger / " + AM.ObjPools[PrefabName].Count + " / " + gameObject.name);
        }
    }
}
