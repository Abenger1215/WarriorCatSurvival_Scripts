// ƨ��� ����
// ������?
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack5 : DirectionAttack
{
    private void OnEnable()
    {
        MoveSpeed = 10f;
        timeCoroutine = StartCoroutine(DisableByTime(AttackManager.instance.attack5Data.duration));
        SoundManager.instance.PlaySFXSound("Attack1", 0.2f);
    }

    private void OnCollisionEnter2D(Collision2D collision) // �÷��̾�� �浹�� ���� - ����ó��
    {
        if (collision.gameObject.CompareTag("MONSTER"))
        {
            dir = -dir;
            collision.gameObject.GetComponent<Monster>().Damaged(AttackManager.instance.attack5Data.damage);
            //Debug.Log("disable by trigger / " + AM.ObjPools[PrefabName].Count + " / " + gameObject.name);
        }
    }

    //IEnumerator Delay(float time)
    //{


    //    yield return;
    //}
}
