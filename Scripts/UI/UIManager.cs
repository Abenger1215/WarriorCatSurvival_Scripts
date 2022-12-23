using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI PlayTimeText;
    public Image ExpBar;
    public Image Pause_Background;
    public Image HpBar;

    public GameObject ReviveButton;

    public GameObject LevelUp;
    public GameObject GameOver;

    public static UIManager instance;

    public LevelUpUI LevelUpUIScript;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != null) {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        LevelUpUIScript = LevelUp.GetComponent<LevelUpUI>();
        LevelUp.SetActive(false);
        Pause_Background.enabled = false;
        GameOver.SetActive(false);
        ExpBar.fillAmount = 0;
        UpdateLevel();
        GameManager.instance.SetEvent();
    }

    void Update()
    {
        TimerText.text = GameManager.instance.PlayTimeText;
    }

    public void ActiveGameOverUI()
    {
        PlayTimeText.text = "Score : " + ((int)GameManager.instance.score).ToString();

        if(GameManager.instance.isRevived == true)
        {
            ReviveButton.SetActive(false);
        }

        GameOver.SetActive(true);
    }

    public void DeactiveGameOverUI()
    {
        GameOver.SetActive(false);
    }

    public void UpdateLevel()
    {
        LevelText.text = "Level " + GameManager.instance.Level.ToString();
    }

    public void ActivePauseBG()
    {
        Pause_Background.enabled = true;
    }

    public void DeactivePauseBG()
    {
        Pause_Background.enabled = false;
    }

    public void ActiveLevelUpUI()
    {
        LevelUp.SetActive(true);
        LevelUpUIScript.RandomSelect(3);
    }

    public void DeactiveLevelUpUI()
    {
        LevelUp.SetActive(false);
    }

    public void UpdateHpUI()
    {
        HpBar.fillAmount = GameManager.instance.Hp / GameManager.instance.maxHp;
    }
}
