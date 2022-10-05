using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    private int CreateObjCount = 0;

    public int ObjTypeCount;
    private Dictionary<string, int> _ObjMaxCounts = new Dictionary<string, int>();
    public Dictionary<string, int> ObjMaxCounts
    {
        get { return _ObjMaxCounts; }
    }
    private Dictionary<string, GameObject> _ObjGroups = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> _ObjPrefabs = new Dictionary<string, GameObject>();
    private Dictionary<string, Stack<GameObject>> _ObjPools = new Dictionary<string, Stack<GameObject>>();
    public Dictionary<string, GameObject> ObjGroups { get { return _ObjGroups; } }
    public Dictionary<string, GameObject> ObjPrefabs { get { return _ObjPrefabs; } }
    public Dictionary<string, Stack<GameObject>> ObjPools { get { return _ObjPools; } }

    protected GameObject CreateNewObject(GameObject ObjPrefab)
    {
        var newObj = Instantiate<GameObject>(ObjPrefab);
        newObj.gameObject.SetActive(false);
        return newObj;
    }

    protected virtual void CreateObjectPool(Stack<GameObject> ObjPool, int MaxCount, GameObject ObjPrefab, GameObject ObjGroup)
    {
        for (int i = 0; i < MaxCount; i++)
        {
            var _object = CreateNewObject(ObjPrefab);

            _object.name = $"{ObjPrefab.name}_{i:00}";
            _object.transform.SetParent(ObjGroup.transform);

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
        var _object = CreateNewObject(ObjPrefab);

        _object.name = $"{ObjPrefab.name}_{CreateObjCount:00}";
        CreateObjCount++;
        _object.transform.SetParent(ObjGroup.transform);

        ObjPool.Push(_object);
    }

    protected virtual void CreateMultipleObjectPool()
    {
        foreach(var tmp in ObjPrefabs)
        {
            GameObject ObjPrefab = tmp.Value;
            string name = ObjPrefab.name;
            int MaxCount = ObjMaxCounts[name];

            for(int i = 0; i < MaxCount; i++)
            {
                if (!ObjPools.ContainsKey(ObjPrefab.name))
                {
                    Stack<GameObject> newList = new Stack<GameObject>();
                    ObjPools.Add(ObjPrefab.name, newList);
                }

                var _object = CreateNewObject(ObjPrefab);

                _object.name = $"{name}_{i:00}";
                _object.transform.SetParent(ObjGroups[name+"Group"].transform);

                ObjPools[ObjPrefab.name].Push(_object);
            }
        }
    }

    public virtual GameObject GetObjectInMultiplePool(string name)
    {
        var _object = ObjPools[name].Pop();
        _object.gameObject.SetActive(true);

        if (ObjPools[name].Count == 1)
        {
            CreateNewObjectToMultiplePool(name);
        }

        return _object.gameObject;
    }

    protected void CreateNewObjectToMultiplePool(string name)
    {
        GameObject ObjPrefab = ObjPrefabs[name];
        GameObject ObjGroup = ObjGroups[name + "Group"];
        
        GameObject _object = CreateNewObject(ObjPrefab);

        _object.name = $"{ObjPrefab.name}_{ObjMaxCounts[ObjPrefab.name]:00}";
        Debug.Log(_object.name);
        ObjMaxCounts[ObjPrefab.name]++;
        _object.transform.SetParent(ObjGroup.transform);

        ObjPools[name].Push(_object);
    }
}
