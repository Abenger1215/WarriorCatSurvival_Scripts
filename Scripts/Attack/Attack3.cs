// Æø¹ßÇÏ´Â °ø°Ý
// ÆøÅº - Bomb

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack3 : DirectionAttack
{
    private void OnEnable()
    {
        timeCoroutine = StartCoroutine(DisableByTime(AttackManager.instance.attack3Data.duration));
        SoundManager.instance.PlaySFXSound("Attack1", 0.2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MONSTER"))
        {
            AttackManager.instance.ActivePostionAttack("Attack3Boom", transform.position);
            ReturnPool();
            //Debug.Log("disable by trigger / " + AM.ObjPools[PrefabName].Count + " / " + gameObject.name);
        }
    }
}
