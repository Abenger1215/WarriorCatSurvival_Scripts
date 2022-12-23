using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpManager : MonoBehaviour
{
    public static LevelUpManager instance = null;

    [SerializeField]
    private LevelUpUI levelUpUI;

    AttackData attack1Data;
    AttackData attack2Data;
    AttackData attack3Data;
    AttackData attack4Data;
    AttackData attack5Data;
    AttackData attack6Data;
    AttackData attack7Data;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        attack1Data = AttackManager.instance.attack1Data;
        attack2Data = AttackManager.instance.attack2Data;
        attack3Data = AttackManager.instance.attack3Data;
        attack4Data = AttackManager.instance.attack4Data;
        attack5Data = AttackManager.instance.attack5Data;
        attack6Data = AttackManager.instance.attack6Data;
        attack7Data = AttackManager.instance.attack7Data;


        if (GameManager.instance.characterName == "warrior")
        {
            Attack1Up();
            levelUpUI.SkillLevelUp("Attack1");
        }
        else if (GameManager.instance.characterName == "mage")
        {
            Attack6Up();
            levelUpUI.SkillLevelUp("Attack6");
        }
        else if (GameManager.instance.characterName == "pirate")
        {
            Attack3Up();
            levelUpUI.SkillLevelUp("Attack3");
        }
    }


    #region Level Increase
    public void HpIncrease()
    {
        GameManager.instance.Hp *= 1.15f;
        GameManager.instance.maxHp *= 1.15f;

        UIManager.instance.DeactiveLevelUpUI();
        GameManager.instance.Resume();
    }

    public void RegenIncrease()
    {
        GameManager.instance.regenHp += 1;

        UIManager.instance.DeactiveLevelUpUI();
        GameManager.instance.Resume();
    }

    public void DamageIncrease()
    {
        GameManager.instance.damage += 0.1f;

        UIManager.instance.DeactiveLevelUpUI();
        GameManager.instance.Resume();
    }

    public void SpeedIncrease()
    {
        GameManager.instance.MoveSpeed += 1f;

        UIManager.instance.DeactiveLevelUpUI();
        GameManager.instance.Resume();
    }

    public void ExpIncrease()
    {
        GameManager.instance.increaseExp += 0.1f;

        UIManager.instance.DeactiveLevelUpUI();
        GameManager.instance.Resume();
    }

    public void Attack1Increase()
    {
        Attack1Up();

        UIManager.instance.DeactiveLevelUpUI();
        GameManager.instance.Resume();
    }

    public void Attack2Increase()
    {
        Attack2Up();

        UIManager.instance.DeactiveLevelUpUI();
        GameManager.instance.Resume();
    }

    public void Attack3Increase()
    {
        Attack3Up();

        UIManager.instance.DeactiveLevelUpUI();
        GameManager.instance.Resume();
    }

    public void Attack4Increase()
    {
        Attack4Up();

        UIManager.instance.DeactiveLevelUpUI();
        GameManager.instance.Resume();
    }
    public void Attack5Increase()
    {
        Attack5Up();

        UIManager.instance.DeactiveLevelUpUI();
        GameManager.instance.Resume();
    }
    public void Attack6Increase()
    {
        Attack6Up();

        UIManager.instance.DeactiveLevelUpUI();
        GameManager.instance.Resume();
    }
    public void Attack7Increase()
    {
        Attack7Up();

        UIManager.instance.DeactiveLevelUpUI();
        GameManager.instance.Resume();
    }

    public void ScoreIncrease()
    {
        GameManager.instance.score += 15f;
        GameManager.instance.Resume();
    }
#endregion

#region ·¹º§¾÷ Logic
    public void Attack1Up()
    {
        attack1Data.level += 1;

        switch(attack1Data.level)
        {
            case 1:
                AttackManager.instance.ActiveAttack1();
                break;
            case 2:
                attack1Data.count += 1;
                break;
            case 3:
                attack1Data.damage += 5;
                break;
            case 4:
                attack1Data.cooltime -= 0.1f;
                break;
            case 5:
                attack1Data.count += 1;
                break;
            case 6:
                attack1Data.cooltime -= 0.1f;
                break;
            case 7:
                attack1Data.damage += 5;
                break;
            case 8:
                attack1Data.damage += 10;
                break;
        }
    }

    public void Attack2Up()
    {
        attack2Data.level += 1;

        switch (attack2Data.level)
        {
            case 1:
                AttackManager.instance.ActiveAttack2();
                break;
            case 2:
                AttackManager.instance.ActiveAttack2();
                break;
            case 3:
                AttackManager.instance.ActiveAttack2();
                break;
            case 4:
                AttackManager.instance.attack2Speed *= 1.2f;
                break;
            case 5:
                AttackManager.instance.ActiveAttack2();
                break;
            case 6:
                attack2Data.damage += 5;
                break;
            case 7:
                AttackManager.instance.ActiveAttack2();
                break;
            case 8:
                attack2Data.damage += 10;
                break;
        }
    }
    public void Attack3Up()
    {
        attack3Data.level += 1;

        switch (attack3Data.level)
        {
            case 1:
                AttackManager.instance.ActiveAttack3();
                break;
            case 2:
                attack3Data.damage += 10;
                break;
            case 3:
                attack3Data.damage += 10;
                break;
            case 4:
                attack3Data.scale *= 1.2f;
                break;
            case 5:
                attack3Data.cooltime -= 0.2f;
                break;
            case 6:
                attack3Data.damage += 15;
                break;
            case 7:
                attack3Data.cooltime -= 0.3f;
                break;
            case 8:
                attack3Data.count += 1;
                break;
        }
    }
    public void Attack4Up()
    {
        attack4Data.level += 1;

        switch (attack4Data.level)
        {
            case 1:
                AttackManager.instance.ActiveAttack4();
                break;
            case 2:
                attack4Data.damage += 10;
                break;
            case 3:
                attack4Data.damage += 15;
                break;
            case 4:
                attack4Data.scale *= 1.2f;
                break;
            case 5:
                attack4Data.count += 2;
                break;
            case 6:
                attack4Data.damage += 20;
                break;
            case 7:
                attack4Data.cooltime -= 1f;
                break;
            case 8:
                attack4Data.count += 2;
                break;
        }
    }

    public void Attack5Up()
    {
        attack5Data.level += 1;

        switch (attack5Data.level)
        {
            case 1:
                AttackManager.instance.ActiveAttack5();
                break;
            case 2:
                attack5Data.damage += 5;
                break;
            case 3:
                attack5Data.damage += 5;
                break;
            case 4:
                attack5Data.duration += 2f;
                break;
            case 5:
                attack5Data.count += 1;
                break;
            case 6:
                attack5Data.cooltime -= 1;
                break;
            case 7:
                attack5Data.damage += 20;
                break;
            case 8:
                attack5Data.count += 1;
                break;
        }
    }
    public void Attack6Up()
    {
        attack6Data.level += 1;

        switch (attack6Data.level)
        {
            case 1:
                AttackManager.instance.ActiveAttack6();
                break;
            case 2:
                attack6Data.damage += 5f;
                break;
            case 3:
                attack6Data.count += 1;
                break;
            case 4:
                attack6Data.damage += 5f;
                break;
            case 5:
                attack6Data.cooltime -= 0.5f;
                break;
            case 6:
                attack6Data.scale *= 1.2f;
                break;
            case 7:
                attack6Data.count += 3;
                break;
            case 8:
                attack6Data.damage += 20f;
                break;
        }
    }

    public void Attack7Up()
    {
        attack7Data.level += 1;

        switch (attack7Data.level)
        {
            case 1:
                AttackManager.instance.ActiveAttack7();
                break;
            case 2:
                attack7Data.damage += 5f;
                break;
            case 3:
                attack7Data.count += 1;
                break;
            case 4:
                attack7Data.damage += 5f;
                break;
            case 5:
                attack7Data.cooltime -= 0.5f;
                break;
            case 6:
                attack7Data.scale *= 1.2f;
                break;
            case 7:
                attack7Data.count += 3;
                break;
            case 8:
                attack7Data.damage += 20f;
                break;
        }
    }
    #endregion
}
