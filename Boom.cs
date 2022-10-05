using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour
{
    private void OnEnable()
    {
        gameObject.transform.localScale = Vector3.one * 0.0001f;
        StartCoroutine(ScaleUp());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator ScaleUp()
    {
        int time = 0;
        while (true)
        {
            if (GameManager.instance.isPause == true)
            {
                yield return new WaitForEndOfFrame();
                continue;
            }

            time++;

            if(time<= 10)
            {
                gameObject.transform.localScale += Vector3.one * 0.2f;

            }

            if(time == 25)
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MONSTER"))
        {
            other.gameObject.GetComponent<MonsterCtrl>().Damaged(1000f);
        }
    }
}
