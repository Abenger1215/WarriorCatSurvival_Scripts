using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : ObjectPooling
{
    public GameObject activeMonsterGroup;
    public KdTree<Monster> activeMonsterTree = new KdTree<Monster>();

    public List<Transform> spawnPoints = new List<Transform>();

    public static MonsterManager instance = null;

    public float reduceCoolTime;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != null)
        {
            Destroy(this.gameObject);
        }


        Transform spawnPointGroup = GameObject.Find("SpawnPointGroup")?.transform;
        foreach (Transform point in spawnPointGroup)
        {
            spawnPoints.Add(point);
        }

        for (int i = 0; i < spawnPointGroup.childCount; i++)
        {
            float rad = Mathf.Deg2Rad * + (i * (360 / spawnPointGroup.childCount));
            float x = 25 * Mathf.Sin(rad);
            float y = 25 * Mathf.Cos(rad);
            spawnPointGroup.GetChild(i).transform.position = Player.instance.transform.position + new Vector3(x, y);
        }

        foreach (var prefab in Resources.LoadAll<GameObject>("Prefabs/Monster/")){ // µÒº≈≥ ∏Æ √ ±‚»≠
            ObjPrefabs.Add(prefab.name, prefab);
            //Debug.Log(prefab.name);
            ObjMaxCounts[prefab.name] = 30;
        }

        ObjTypeCount = ObjPrefabs.Count;

        for (int i = 1; i <= ObjTypeCount; i++)
        {
            GameObject MonsterGroup = new GameObject($"Monster{i}Group");
            ObjGroups.Add(MonsterGroup.name, MonsterGroup);
        }

        CreateMultipleObjectPool();
        StartCoroutine(SpawnMonster("Monster1", 0.5f));
        StartCoroutine(SpawnMonster("Monster2", 0.5f));
        StartCoroutine(SpawnMonster("Monster3", 30f));
        StartCoroutine(FindNearest());
    }

    IEnumerator FindNearest()
    {
        while(GameManager.instance.isPlaying)
        {
            FindNearestMonster();
            //Debug.Log(Player.instance.nearestMonster.name);
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }

    private void FindNearestMonster()
    {
        if(activeMonsterTree.Count != 0)
        {
            activeMonsterTree.UpdatePositions();

            //Debug.Log(activeMonsterTree.Count);
            Monster nearestObj = activeMonsterTree.FindClosest(Player.instance.transform.position);
            Player.instance.nearestMonster = nearestObj;
        }
    }

    public override GameObject GetObjectInMultiplePool(string name)
    {
        var _object = ObjPools[name].Pop();
        _object.transform.SetParent(activeMonsterGroup.transform);

        if (ObjPools[name].Count == 1)
        {
            CreateNewObjectToMultiplePool(name);
        }

        return _object.gameObject;
    }

    public IEnumerator SpawnMonster(string name, float spawnCooltime)
    {
        while (GameManager.instance.isPlaying == true)
        {
            if(GameManager.instance.isPause == true)
            {
                yield return new WaitForSecondsRealtime(1f);
                continue;
            }

            int idx = Random.Range(0, spawnPoints.Count);

            GameObject MonsterObj = GetObjectInMultiplePool(name);

            MonsterObj.transform.position = spawnPoints[idx].transform.position;
            MonsterObj.SetActive(true);
            MonsterObj.transform.SetParent(activeMonsterGroup.transform);

            activeMonsterTree.Add(MonsterObj.GetComponent<Monster>());

            yield return new WaitForSecondsRealtime(spawnCooltime - reduceCoolTime);
        }
    }

}
