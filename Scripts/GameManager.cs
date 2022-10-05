using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public bool isPlaying;
    public bool isPause;
    public bool isRevived;
    public UnityEvent onPlayerDead;
    public UnityEvent onPlayerLevelUp;

    public float Hp = 100.0f;
    public float maxHp = 100.0f;
    public int regenHp = 0;
    public float damage = 1.0f;
    public float MoveSpeed = 10.0f;

    public int Level = 1;
    public float exp = 0.0f;
    private float maxExp = 100.0f;
    private float increaseExp = 1.0f;

    public float PlayTime;
    public string PlayTimeText;

    public float score = 0;

    private GameObject player;

    public static GameManager instance = null;

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
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("PLAYER");
        Application.targetFrameRate = 30;
        PlayNewGame();
        //PlayTime = Time.deltaTime;
    }

    private void Update()
    {
        //PlayTime += 1 * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.K))
        {
            AttackManager.instance.ActiveAttack2();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            exp += 500;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            Hp = 0f;
            GameOver();
        }
    }

    public void PlayNewGame()
    {
        ResetField();
        StartCoroutine(PlusTimer());
    }

    public void ResetField()
    {
        isPlaying = true;
        isPause = false;
        isRevived = false;
        Time.timeScale = 1;
        Hp = 100.0f;
        maxHp = 100.0f;
        regenHp = 0;
        damage = 1.0f;
        Level = 1;
        exp = 0.0f;
        maxExp = 100.0f;
        increaseExp = 1.0f;
        score = 0f;
        PlayTime = 0f;
        PlayTimeText = "00 : 00";
        UIManager.instance.ExpBar.fillAmount = 0;
    }

    public void GameOver()
    {
        SoundManager.instance.PlayBGMSound("GameOver", 0.25f);

        if(isRevived == true)
        {
            isPlaying = false;
            StopAllCoroutines();
            Debug.Log("GameOver");
        }

        Pause();
        onPlayerDead.Invoke();
    }

    public void Revive()
    {
        SoundManager.instance.PlayBGMSound("Stage", 0.25f);
        isRevived = true;
        UIManager.instance.DeactiveGameOverUI();
        AttackManager.instance.ActiveBoom();
        Hp = maxHp;
        Resume();
    }

    public void Pause()
    {
        UIManager.instance.ActivePauseBG();
        SoundManager.instance.PauseBGM();
        Time.timeScale = 0;
        isPause = true;
    }

    public void Resume()
    {
        SoundManager.instance.UnPauseBGM();
        Time.timeScale = 1;
        isPause = false;
        UIManager.instance.DeactivePauseBG();
    }

    public void Damaged(float damage)
    {

        if (Hp < 0)
        {
            GameOver();
            return;
        }
        Hp -= damage;

        StartCoroutine(ChangePlayerColorByDamage());
    }

    IEnumerator ChangePlayerColorByDamage()
    {
        player.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        player.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void ExpUp(float exp)
    {
        this.exp += exp * increaseExp;
        if(this.exp >= maxExp)
        {
            LevelUp();
            this.exp = this.exp - maxExp;
            maxExp = maxExp * 1.05f;
        }
        UIManager.instance.ExpBar.fillAmount = this.exp / maxExp;
    }

    public void LevelUp()
    {
        SoundManager.instance.PlaySFXSound("LevelUp", 0.3f);
        Level++;
        Debug.Log("Player Level Up" + Level);

        if(MonsterManager.instance.SpawnCooltime >= 0.1f)
        {
            MonsterManager.instance.SpawnCooltime -= 0.01f;
        }

        onPlayerLevelUp.Invoke();
    }

    IEnumerator PlusTimer()
    {
        while(isPlaying == true)
        {
            if(isPause == true)
            {
                yield return new WaitForSecondsRealtime(1f);
                continue;
            }
            yield return new WaitForSecondsRealtime(1f);
            PlayTime++;
            PlayTimeText = $"{System.Math.Truncate(PlayTime / 60):00} : {PlayTime % 60:00}";
            RegenHp();
            score += 1f;
        }

        yield return null;
    }

    private void RegenHp()
    {
        Hp += maxHp * regenHp * 0.01f;
        UIManager.instance.UpdateHpUI();
    }

    public void RunMethod(string methodName)
    {
        SendMessage(methodName, instance);
    }

    public void HpIncrease()
    {
        Hp = Hp * 1.15f;
        maxHp = maxHp * 1.15f;
        Debug.Log("maxHp : " + maxHp + " / Hp : " + Hp);

        UIManager.instance.DeactiveLevelUpUI();
        Resume();
    }

    public void RegenIncrease()
    {
        regenHp += 1;
        Debug.Log("RegenIncrease / " + regenHp);

        UIManager.instance.DeactiveLevelUpUI();
        Resume();
    }

    public void DamageIncrease()
    {
        damage += 0.1f;
        Debug.Log("DamageIncrease / " + damage);

        UIManager.instance.DeactiveLevelUpUI();
        Resume();
    }

    public void SpeedIncrease()
    {
        MoveSpeed += 1f;
        Debug.Log("SpeedIncrease / " + MoveSpeed);

        UIManager.instance.DeactiveLevelUpUI();
        Resume();
    }

    public void ExpIncrease()
    {
        increaseExp += 0.1f;
        Debug.Log("ExpIncrease / " + increaseExp);

        UIManager.instance.DeactiveLevelUpUI();
        Resume();
    }

    public void Attack1Increase()
    {
        AttackManager.instance.Attack1Up();
        Debug.Log("Attack1Up / " + AttackManager.instance.attack1Lv);

        UIManager.instance.DeactiveLevelUpUI();
        Resume();
    }
    public void Attack2Increase()
    {
        AttackManager.instance.Attack2Up();
        Debug.Log("Attack2Up / " + AttackManager.instance.attack2Lv);

        UIManager.instance.DeactiveLevelUpUI();
        Resume();
    }

    public void ScoreIncrease()
    {
        Debug.Log("Score Increase");

        score += 15f;
        Resume();
    }
}
