// Attack의 생성과 초기화 기능, 관련 멤버를 관리하는 메서드
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : ObjectPooling // Player에 부착된 component
{
    Player player;
    public Boom reviveBoom;

    public AttackData attack1Data = new AttackData(level: 0, damage: 10f, count: 1, cooltime: 0.5f);
    public AttackData attack2Data = new AttackData(level: 0, damage: 15f, count: 0);
    public AttackData attack3Data = new AttackData(level: 0, damage: 20f, count: 1, cooltime: 2f, scale: 0.5f);
    public AttackData attack4Data = new AttackData(level: 0, damage: 15f, count: 1, cooltime: 3f, scale: 1f);
    public AttackData attack5Data = new AttackData(level: 0, damage: 5f, count: 1, cooltime: 1f, duration: 2f);
    public AttackData attack6Data = new AttackData(level: 0, damage: 10f, count: 2, cooltime: 3f, scale: 0.2f, duration: 0.2f);
    public AttackData attack7Data = new AttackData(level: 0, damage: 1f, count: 1, cooltime: 3f, scale: 3f, duration: 3f);

    public GameObject activeAttack2Group;
    public float attack2Deg = 0.0f;
    public float attack2Speed = 100.0f;

    public float attack3BoomDuration = 0.2f;

    public Transform attack7PointGroup;
    public List<Transform> attack7Points;
    public int attack7Index = 0;

    public static AttackManager instance;

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
        { // 딕셔너리 초기화
            ObjPrefabs.Add(prefab.name, prefab);
            //Debug.Log(prefab.name);
            if(prefab.name == "Attack2")
            {
                ObjMaxCounts[prefab.name] = 5;
            }
            else
            {
                ObjMaxCounts[prefab.name] = 10;
            }
            GameObject AttackGroup = new GameObject($"{prefab.name}Group");
            ObjGroups.Add(AttackGroup.name, AttackGroup);
        }

        ObjTypeCount = ObjPrefabs.Count;

        player = Player.instance;

        CreateMultipleObjectPool();
    }

    public void FixedUpdate()
    {
        attack2Deg += Time.fixedDeltaTime * attack2Speed;

        if(attack2Deg > 360)
        {
            attack2Deg = 0;
        }
    }

    public IEnumerator AttackNearestMonster(string name)
    {
        if(name == "Attack1")
        {
            while (true)
            {
                if (GameManager.instance.isPause == true)
                {
                    yield return new WaitForSecondsRealtime(0.1f);
                    continue;
                }
                for (int i = 0; i < attack1Data.count; i++)
                {
                    if (ObjPools[name].Count == 0)
                    {
                        CreateNewObjectToMultiplePool(name);
                    }
                    ActiveDirectionAttack(name);
                    yield return new WaitForSecondsRealtime(0.05f);
                }
                yield return new WaitForSecondsRealtime(attack1Data.cooltime);
            }
        }
        else if (name == "Attack3")
        {
            while (true)
            {
                if (GameManager.instance.isPause == true)
                {
                    yield return new WaitForSecondsRealtime(0.1f);
                    continue;
                }
                for (int i = 0; i < attack3Data.count; i++)
                {
                    if (ObjPools[name].Count == 0)
                    {
                        CreateNewObjectToMultiplePool(name);
                    }
                    ActiveDirectionAttack(name);
                    yield return new WaitForSecondsRealtime(0.05f);
                }
                yield return new WaitForSecondsRealtime(attack3Data.cooltime);
            }
        }
        else if (name == "Attack4")
        {
            while (true)
            {
                if (GameManager.instance.isPause == true)
                {
                    yield return new WaitForSecondsRealtime(0.1f);
                    continue;
                }
                for (int i = 0; i < attack4Data.count; i++)
                {
                    if (ObjPools[name].Count == 0)
                    {
                        CreateNewObjectToMultiplePool(name);
                    }
                    GameObject AttackObj = GetObjectInMultiplePool(name);
                    AttackObj.transform.position = transform.position;
                    if (player.nearestMonster != null)
                    {
                        AttackObj.GetComponent<DirectionAttack>().dir = new Vector2(Random.Range(-1f, 1f), 3f);// 가장 가까운 몬스터 조준
                        AttackObj.SetActive(true);
                    }
                    yield return new WaitForSecondsRealtime(0.05f);
                }
                yield return new WaitForSecondsRealtime(attack4Data.cooltime);
            }
        }
        else if (name == "Attack5")
        {
            while (true)
            {
                if (GameManager.instance.isPause == true)
                {
                    yield return new WaitForSecondsRealtime(0.1f);
                    continue;
                }
                for (int i = 0; i < attack5Data.count; i++)
                {
                    if (ObjPools[name].Count == 0)
                    {
                        CreateNewObjectToMultiplePool(name);
                    }
                    ActiveDirectionAttack(name);
                    yield return new WaitForSecondsRealtime(0.1f);
                }
                yield return new WaitForSecondsRealtime(attack5Data.cooltime);
            }
        }
        else if(name == "Attack6")
        {
            while (true)
            {
                int count;
                if (GameManager.instance.isPause == true)
                {
                    yield return new WaitForSecondsRealtime(0.1f);
                    continue;
                }
                for (int i = 0; i < attack6Data.count; i++)
                {
                    if (ObjPools[name].Count == 0)
                    {
                        CreateNewObjectToMultiplePool(name);
                    }
                    count = MonsterManager.instance.activeMonsterGroup.transform.childCount;
                    if(count != 0 && MonsterManager.instance.activeMonsterTree.Count != 0)
                    {
                        ActivePostionAttack(name, player.nearestMonster.transform.position, true);
                        Debug.Log(player.nearestMonster.name);
                    }
                    yield return new WaitForSecondsRealtime(0.1f);
                }
                yield return new WaitForSecondsRealtime(attack6Data.cooltime);
            }
        }
        else if(name == "Attack7")
        {
            while(true)
            {
                Debug.Log("Attack7 Active");
                if (GameManager.instance.isPause == true)
                {
                    yield return new WaitForSecondsRealtime(0.1f);
                    continue;
                }
                for (int i = 0; i < attack7Data.count; i++)
                {
                    if (ObjPools[name].Count == 0)
                    {
                        CreateNewObjectToMultiplePool(name);
                    }
                    ActivePostionAttack(name, attack7Points[attack7Index].position);

                    attack7Index++;
                    if(attack7Index == 8)
                    {
                        attack7Index = 0;
                    }
                    yield return new WaitForSecondsRealtime(0.1f);
                }
                yield return new WaitForSecondsRealtime(attack7Data.cooltime);
            }
        }
    }

    public void ActiveDirectionAttack(string name)
    {
        GameObject AttackObj = GetObjectInMultiplePool(name);
        AttackObj.transform.position = transform.position;
        if (player.nearestMonster != null)
        {
            AttackObj.GetComponent<DirectionAttack>().SetDir(player.nearestMonster.transform.position); // 가장 가까운 몬스터 조준
            AttackObj.SetActive(true);
        }
    }

    public void ActivePostionAttack(string name, Vector3 target, bool isCheckDistance = false)
    {
        if(isCheckDistance == true)
        {
            Vector3 offset = target - transform.position;
            if (Vector3.SqrMagnitude(offset) <= 400)
            {
                GameObject AttackObj = GetObjectInMultiplePool(name);
                AttackObj.GetComponent<PositionAttack>().ActiveAttack(target);
                AttackObj.SetActive(true);
            }
        }
        else
        {
            GameObject AttackObj = GetObjectInMultiplePool(name);
            AttackObj.GetComponent<PositionAttack>().ActiveAttack(target);
            AttackObj.SetActive(true);
        }
    }

    public void ActiveAttack1()
    {
        StartCoroutine(AttackNearestMonster("Attack1"));
    }

    public void ActiveAttack2()
    {
        GameObject Attack2Obj = base.GetObjectInMultiplePool("Attack2");
        Attack2Obj.transform.SetParent(activeAttack2Group.transform);
        Attack2Obj.GetComponent<Attack2>().idx = attack2Data.count;
        attack2Data.count++;
        //Debug.Log("ActiveAttack2 - " + attack2Data.count);
    }
    public void ActiveAttack3()
    {
        StartCoroutine(AttackNearestMonster("Attack3"));
    }

    public void ActiveAttack4()
    {
        StartCoroutine(AttackNearestMonster("Attack4"));
    }
    public void ActiveAttack5()
    {
        StartCoroutine(AttackNearestMonster("Attack5"));
    }
    public void ActiveAttack6()
    {
        StartCoroutine(AttackNearestMonster("Attack6"));
    }

    public void ActiveAttack7()
    {
        Debug.Log("ActiveAttack7");
        //foreach (Transform point in attack7PointGroup)
        //{
        //    attack7Points.Add(point);
        //}

        for (int i = 0; i < attack7Points.Count; i++)
        {
            float rad = Mathf.Deg2Rad * +(i * (360 / attack7Points.Count));
            float x = 7.5f * Mathf.Sin(rad);
            float y = 7.5f * Mathf.Cos(rad);
            attack7PointGroup.GetChild(i).transform.position = new Vector3(x, y);
        }

        StartCoroutine(AttackNearestMonster("Attack7"));
    }

    public void ActiveReviveBoom()
    {
        reviveBoom.TriggerBoom(0.1f, 25);
    }
    public override GameObject GetObjectInMultiplePool(string name)
    {
        var _object = ObjPools[name].Pop();
        //Debug.Log("POP / " + _object.name + " / " + ObjPools[name].Count);

        return _object.gameObject;
    }

}
