using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackData
{
    public int level;
    public int count;
    public float scale;
    public float damage;
    public float cooltime;
    public float duration;

    public AttackData(int level = 0, int count = 1, float scale = 1f, float damage = 10f, float cooltime = 1f, float duration = 3f)
    {
        this.level = level;
        this.count = count;
        this.scale = scale;
        this.damage = damage;
        this.cooltime = cooltime;
        this.duration = duration;
    }
}
