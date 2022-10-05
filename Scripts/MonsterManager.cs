using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : ObjectPooling
{
    private GameObject Player;

    public GameObject ActiveMonsterGroup;
    public KdTree<MonsterCtrl> ActiveMonsterTree = new KdTree<MonsterCtrl>();

    public List<Transform> SpawnPoints = new List<Transform>();

    public static MonsterManager instance = null;

    public float SpawnCooltime;

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

        Player = GameObject.FindGameObjectWithTag("PLAYER");

        Transform spawnPointGroup = GameObject.Find("SpawnPointGroup")?.transform;
        foreach (Transform point in spawnPointGroup)
        {
            SpawnPoints.Add(point);
        }

        for (int i = 0; i < spawnPointGroup.childCount; i++)
        {
            float rad = Mathf.Deg2Rad * + (i * (360 / spawnPointGroup.childCount));
            float x = 25 * Mathf.Sin(rad);
            float y = 25 * Mathf.Cos(rad);
            spawnPointGroup.GetChild(i).transform.position = Player.transform.position + new Vector3(x, y);

        }

        foreach (var prefab in Resources.LoadAll<GameObject>("Prefabs/Monster/")){ // µÒº≈≥ ∏Æ √ ±‚»≠
            ObjPrefabs.Add(prefab.name, prefab);
            Debug.Log(prefab.name);
            ObjMaxCounts[prefab.name] = 30;
        }

        ObjTypeCount = ObjPrefabs.Count;

        for (int i = 1; i <= ObjTypeCount; i++)
        {
            GameObject MonsterGroup = new GameObject($"Monster{i}Group");
            ObjGroups.Add(MonsterGroup.name, MonsterGroup);
        }

        SpawnCooltime = 0.5f;

        CreateMultipleObjectPool();
        StartCoroutine(SpawnMonster("Monster1"));
        StartCoroutine(SpawnMonster("Monster2"));
        FindNearestMonster();
    }
    private void Update()
    {
        FindNearestMonster();
    }

    private void FindNearestMonster()
    {
        ActiveMonsterTree.UpdatePositions();

        MonsterCtrl nearestObj = ActiveMonsterTree.FindClosest(Player.transform.position);
        Player.GetComponent<PlayerCtrl>().NearestMonster = nearestObj;
    }

    public override GameObject GetObjectInMultiplePool(string name)
    {
        var _object = ObjPools[name].Pop();
        _object.transform.SetParent(ActiveMonsterGroup.transform);

        if (ObjPools[name].Count == 1)
        {
            CreateNewObjectToMultiplePool(name);
        }

        return _object.gameObject;
    }

    public IEnumerator SpawnMonster(string name)
    {
        while (GameManager.instance.isPlaying == true)
        {
            if(GameManager.instance.isPause == true)
            {
                yield return new WaitForSecondsRealtime(1f);
                continue;
            }

            int idx = Random.Range(0, SpawnPoints.Count);

            GameObject MonsterObj = GetObjectInMultiplePool(name);

            MonsterObj.transform.position = SpawnPoints[idx].transform.position;
            MonsterObj.SetActive(true);
            MonsterObj.transform.SetParent(ActiveMonsterGroup.transform);

            ActiveMonsterTree.Add(MonsterObj.GetComponent<MonsterCtrl>());

            yield return new WaitForSecondsRealtime(SpawnCooltime);
        }
    }

}
