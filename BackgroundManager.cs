using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : ObjectPooling
{
    public GameObject BackgroundGroup;

    public GameObject BackgroundPrefab;
    public int BackgroundMaxCount;
    public Stack<GameObject> BackgroundPool = new Stack<GameObject>();

    public static BackgroundManager instance = null;

    public bool isCreate;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this.gameObject);
        }

        CreateObjectPool(BackgroundPool, BackgroundMaxCount, BackgroundPrefab, BackgroundGroup);

        isCreate = false;
    }

    void Start()
    {
        GameObject StartBackGround = GetObjectInPool(BackgroundPool, BackgroundPrefab, BackgroundGroup);

        StartBackGround.transform.position = Vector3.zero;
    }

    public void ChangeCreate()
    {
        StartCoroutine("ChangeCreateCoroutine");
    }

    IEnumerator ChangeCreateCoroutine()
    {
        isCreate = true;
        yield return new WaitForSecondsRealtime(0.1f);
        isCreate = false;
    }
}
