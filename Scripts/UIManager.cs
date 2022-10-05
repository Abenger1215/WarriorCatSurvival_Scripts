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

        LevelUpUIScript = GetComponent<LevelUpUI>();
    }
    private void Start()
    {
        LevelUp.SetActive(false);
        Pause_Background.enabled = false;
        GameOver.SetActive(false);
        UpdateLevel();
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
        LevelUpUIScript.RandomSelect(3);
        LevelUp.SetActive(true);
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
