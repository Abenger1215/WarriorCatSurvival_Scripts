using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack3Boom : PositionAttack
{
    private void OnEnable()
    {
        gameObject.transform.localScale = Vector3.one * AttackManager.instance.attack3Data.scale;
        StartCoroutine(TimeCheck(AttackManager.instance.attack3BoomDuration));
    }

    IEnumerator TimeCheck(float duration)
    {
        float time = 0;
        while (true)
        {
            time += 0.1f;
            if (time >= duration)
            {
                break;
            }
            yield return new WaitForSecondsRealtime(0.1f);
        }
        ReturnPool();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MONSTER"))
        {
            other.gameObject.GetComponent<Monster>().Damaged(AttackManager.instance.attack3Data.damage);
        }
    }
}
