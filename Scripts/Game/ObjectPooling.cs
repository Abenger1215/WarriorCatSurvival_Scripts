using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    private int CreateObjCount = 0;

    public int ObjTypeCount;

    [HideInInspector] public Stack<GameObject> ObjPool;
    [HideInInspector] public int MaxCount;
    [HideInInspector] public GameObject ObjPrefab;
    [HideInInspector] public GameObject ObjGroup;

    [HideInInspector] public Dictionary<string, int> ObjMaxCounts = new Dictionary<string, int>();
    [HideInInspector] public Dictionary<string, GameObject> ObjGroups = new Dictionary<string, GameObject>();
    [HideInInspector] public Dictionary<string, GameObject> ObjPrefabs = new Dictionary<string, GameObject>();
    [HideInInspector] public Dictionary<string, Stack<GameObject>> ObjPools = new Dictionary<string, Stack<GameObject>>();

    protected GameObject CreateNewObject(GameObject ObjPrefab, Transform parent)
    {
        var newObj = Instantiate<GameObject>(ObjPrefab, parent);
        newObj.gameObject.SetActive(false);
        return newObj;
    }

    protected virtual void CreateObjectPool(Stack<GameObject> ObjPool, int MaxCount, GameObject ObjPrefab, GameObject ObjGroup)
    {
        for (int i = 0; i < MaxCount; i++)
        {
            var _object = CreateNewObject(ObjPrefab, ObjGroup.transform);

            _object.name = $"{ObjPrefab.name}_{i:00}";

            ObjPool.Push(_object);
        }

        CreateObjCount = MaxCount;
    }

    public virtual GameObject GetObjectInPool(Stack<GameObject> ObjPool, GameObject ObjPrefab, GameObject ObjGroup)
    {
        if(ObjPool.Count == 0)
        {
            CreateNewObjectToPool(ObjPool, ObjPrefab, ObjGroup);
        }

        var _object = ObjPool.Pop();
        _object.gameObject.SetActive(true);

        return _object.gameObject;
    }

    protected virtual void CreateNewObjectToPool(Stack<GameObject> ObjPool, GameObject ObjPrefab, GameObject ObjGroup)
    {
        var _object = CreateNewObject(ObjPrefab, ObjGroup.transform);

        _object.name = $"{ObjPrefab.name}_{CreateObjCount:00}";
        CreateObjCount++;

        ObjPool.Push(_object);
    }

    protected virtual void CreateMultipleObjectPool()
    {
        foreach(var tmp in ObjPrefabs)
        {
            GameObject ObjPrefab = tmp.Value;
            string name = ObjPrefab.name;
            int MaxCount = ObjMaxCounts[name];
            ObjGroups[name + "Group"].SetActive(false);

            for (int i = 0; i < MaxCount; i++)
            {
                if (!ObjPools.ContainsKey(ObjPrefab.name))
                {
                    Stack<GameObject> newList = new Stack<GameObject>();
                    ObjPools.Add(ObjPrefab.name, newList);
                }

                var _object = CreateNewObject(ObjPrefab, ObjGroups[name + "Group"].transform);

                _object.name = $"{name}_{i:00}";

                ObjPools[ObjPrefab.name].Push(_object);
            }

            ObjGroups[name + "Group"].SetActive(true);
        }
    }

    public virtual GameObject GetObjectInMultiplePool(string name)
    {
        var _object = ObjPools[name].Pop();
        _object.gameObject.SetActive(true);

        return _object.gameObject;
    }

    protected void CreateNewObjectToMultiplePool(string name)
    {
        GameObject ObjPrefab = ObjPrefabs[name];
        GameObject ObjGroup = ObjGroups[name + "Group"];
        
        GameObject _object = CreateNewObject(ObjPrefab, ObjGroup.transform);

        _object.name = $"{ObjPrefab.name}_{ObjMaxCounts[ObjPrefab.name]:00}";
        //Debug.Log(_object.name);
        ObjMaxCounts[ObjPrefab.name]++;

        ObjPools[name].Push(_object);
    }
}
