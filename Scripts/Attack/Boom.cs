using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour
{
    IEnumerator ScaleUp(int limitTime)
    {
        int time = 0;
        Debug.Log($"Boom activate by {limitTime}");
        while (true)
        {
            if (GameManager.instance.isPause == true)
            {
                yield return new WaitForEndOfFrame();
                continue;
            }

            time++;

            if(time<= limitTime / 2)
            {
                gameObject.transform.localScale += Vector3.one * 0.2f;

            }

            if(time == limitTime)
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
            other.gameObject.GetComponent<Monster>().Damaged(1000f);
        }
    }

    public void TriggerBoom(float startScale, int limitTime)
    {
        gameObject.SetActive(true);
        gameObject.transform.localScale = Vector3.one * startScale;
        StartCoroutine(ScaleUp(limitTime));
    }
}
