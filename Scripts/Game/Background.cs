using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private BackgroundManager BM;
    private GameObject Player;

    int Mask = 1 << 8;

    RaycastHit2D hit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PLAYER"))
        {
            //Debug.Log("PLAYER ENTER");
            CreateBackGround();
        }
    }

    private void Start()
    {
        BM = BackgroundManager.instance;
        Player = GameObject.Find("Player");
    }

    private void FixedUpdate()
    {
        float distance = (gameObject.transform.position - Player.transform.position).magnitude;

        if(distance > 70.0f)
        {
            //Debug.Log(gameObject.name);
            ReturnPool();
        }
    }

    void CreateBackGround()
    {
        if (BM.isCreate == true)
        {
            return;
        }

        Vector3[] directions = new Vector3[] { Vector3.left, Vector3.up, Vector3.right, Vector3.down,
            Vector3.up + Vector3.left, Vector3.up + Vector3.right, Vector3.down + Vector3.left,Vector3.down + Vector3.right };

        foreach(Vector3 dir in directions)
        {
            if (Physics2D.Raycast(gameObject.transform.position + dir * 30f, dir, 15f, Mask)) // 레이캐스트로 배경이 있는지 확인
            {
                //Debug.Log("Hit!");
            }
            else // 없다면 배경 생성
            {
                //Debug.Log("Create BG");
                GameObject NewBg = BM.GetObjectInPool(BM.BackgroundPool, BM.BackgroundPrefab, BM.BackgroundGroup);
                NewBg.transform.position = gameObject.transform.position + dir * 30.0f;
                
            }
        }

        BM.ChangeCreate();
    }

    void ReturnPool()
    {
        //Debug.Log(gameObject.name);
        gameObject.SetActive(false);
        BM.BackgroundPool.Push(gameObject);
    }
}
