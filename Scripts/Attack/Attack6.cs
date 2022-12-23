using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack6 : PositionAttack
{
    private void OnEnable()
    {
        Debug.Log(gameObject.name);
        gameObject.transform.localScale = Vector3.one * AttackManager.instance.attack6Data.scale;
        StartCoroutine(TimeCheck(AttackManager.instance.attack6Data.duration));
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
            other.gameObject.GetComponent<Monster>().Damaged(AttackManager.instance.attack6Data.damage);
        }
    }
}
