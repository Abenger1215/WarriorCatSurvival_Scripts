using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public GameObject Player;
    private Transform pTr;

    private void Start()
    {
        pTr = Player.GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        gameObject.transform.position = pTr.position + new Vector3(0, 0, -10);
    }
}
