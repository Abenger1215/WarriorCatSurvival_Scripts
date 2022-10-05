using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : ObjectPooling // Playerø° ∫Œ¬¯µ» component
{
    PlayerCtrl PlayerCtrl;
    public GameObject ActiveAttack2Group;
    public GameObject Boom;

    public int activeCount;

    public float Atk1Damage = 10f;
    public int Atk1Count = 1;
    public float Atk1Cooltime = 0.5f;

    public float Atk2Damage;
    public float Atk2Deg;
    private float Atk2Speed;

    public static AttackManager instance;

    public int attack1Lv = 0;
    public int attack2Lv = 0;

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

        foreach (var prefab in Resources.LoadAll<GameObject>("Prefabs/Attack/"))
        { // µÒº≈≥ ∏Æ √ ±‚»≠
            ObjPrefabs.Add(prefab.name, prefab);
            Debug.Log(prefab.name);
            ObjMaxCounts[prefab.name] = 10;
            if(prefab.name == "Attack2")
            {
                ObjMaxCounts[prefab.name] = 5;
            }
        }

        ObjTypeCount = ObjPrefabs.Count;

        for (int i = 1; i <= ObjTypeCount; i++)
        {
            GameObject AttackGroup = new GameObject($"Attack{i}Group");
            ObjGroups.Add(AttackGroup.name, AttackGroup);
        }

        PlayerCtrl = GetComponent<PlayerCtrl>();

        CreateMultipleObjectPool();

        activeCount = 0;
    }

    private void Start()
    {
        Atk2Damage = 15f;
        Atk2Speed = 100.0f;
        Atk2Deg = 0;

        Atk1Damage = 10f;
        Atk1Count = 1;
        Atk1Cooltime = 0.5f;

    StartCoroutine(AttackNearestMonster("Attack1"));
    }

    public void FixedUpdate()
    {
        Atk2Deg += Time.fixedDeltaTime * Atk2Speed;

        if(Atk2Deg > 360)
        {
            Atk2Deg = 0;
        }
    }

    public IEnumerator AttackNearestMonster(string name)
    {
        while (true)
        {
            if (GameManager.instance.isPause == true)
            {
                yield return new WaitForSecondsRealtime(0.1f);
                continue;
            }
            for(int i = 0; i < Atk1Count; i++)
            {
                if (ObjPools[name].Count == 0)
                {
                    //Debug.Log(name + " Stack is empty");
                }
                else if (ObjPools[name].Count != 0)
                {
                    GameObject AttackObj = GetObjectInMultiplePool(name);
                    AttackObj.transform.position = transform.position;
                    AttackObj.GetComponent<Attack1>().SetDir(PlayerCtrl.NearestMonster.transform.position);
                    if (PlayerCtrl.NearestMonster.transform.position.z == 0)
                    {
                        AttackObj.SetActive(true);
                    }
                }
                yield return new WaitForSecondsRealtime(0.05f);
            }
            yield return new WaitForSecondsRealtime(Atk1Cooltime);
        }
    }

    public override GameObject GetObjectInMultiplePool(string name)
    {
        var _object = ObjPools[name].Pop();
        //Debug.Log("POP / " + _object.name + " / " + ObjPools[name].Count);

        return _object.gameObject;
    }

    public void ActiveAttack2()
    {
        GameObject Attack2Obj = base.GetObjectInMultiplePool("Attack2");
        Attack2Obj.transform.SetParent(ActiveAttack2Group.transform);
        Attack2Obj.GetComponent<Attack2>().idx = activeCount;
        activeCount++;
        Debug.Log("ActiveAttack2 - " + activeCount);
    }

    public void ActiveBoom()
    {
        Boom.SetActive(true);
    }

    public void Attack1Up()
    {
        attack1Lv += 1;
        Debug.Log("Attack1Up / " + attack1Lv);

        if (attack1Lv == 1)
        {
            Atk1Damage += 5;
        }
        else if (attack1Lv == 2)
        {
            Atk1Count += 1;
        }
        else if (attack1Lv == 3)
        {
            Atk1Damage += 5;
        }
        else if (attack1Lv == 4)
        {
            Atk1Cooltime -= 0.1f;
        }
        else if (attack1Lv == 5)
        {
            Atk1Count += 1;
        }
        else if (attack1Lv == 6)
        {
            Atk1Cooltime -= 0.1f;
        }
        else if (attack1Lv == 7)
        {
            Atk1Damage += 5;
        }
        else if (attack1Lv == 8)
        {
            Atk1Damage += 10;
        }
    }

    public void Attack2Up()
    {
        attack2Lv += 1;
        Debug.Log("Attack1Up / " + attack2Lv);

        if(attack2Lv == 1)
        {
            ActiveAttack2();
        }
        else if(attack2Lv == 2)
        {
            ActiveAttack2();
        }
        else if (attack2Lv == 3)
        {
            ActiveAttack2();

        }
        else if (attack2Lv == 4)
        {
            foreach (var ActiveAttack2 in ActiveAttack2Group.GetComponentsInChildren<Attack2>())
            {
                Atk2Speed *= 1.2f;
            }

        }
        else if (attack2Lv == 5)
        {
            ActiveAttack2();

        }
        else if (attack2Lv == 6)
        {
            Atk2Damage += 5;
        }
        else if (attack2Lv == 7)
        {
            ActiveAttack2();

        }
        else if (attack2Lv == 8)
        {
            Atk2Damage += 10;
        }
    }
}
