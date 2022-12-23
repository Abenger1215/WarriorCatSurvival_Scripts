using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class LevelUpUI : MonoBehaviour
{
    [SerializeField]
    private List<string> SelectSkillList = new List<string>(); // 선택한 스킬 목록
    private List<string> RandomIdxList = new List<string>(); // 랜덤한 idx를 가지고 있는 list

    [SerializeField]
    private List<Dictionary<string, object>> LevelUpData;

    public List<Button> ButtonList = new List<Button>();

    public List<string> UIDictKeys = new List<string>();
    public List<string> activeKeys = new List<string>();
    public List<string> passiveKeys = new List<string>();

    public Dictionary<string, int> SkillLevel = new Dictionary<string, int>(); // 선택한 스킬 레벨
    public Dictionary<string, int> MaxSkillLevel = new Dictionary<string, int>(); // 선택한 스킬 최대 레벨

    public TextMeshProUGUI SkillLevelText;

    const int MAXCOUNT = 3;
    private bool isMaximum = false;
    private bool isSelectLimit = false;

    [SerializeField]
    private int activeCount = -1;
    [SerializeField]
    private int passiveCount = 0;

    private void Awake()
    {
        LevelUpData = CSVReader.Read("LevelUpData");

        List<string> keys = new List<string>(LevelUpData[0].Keys);
        UIDictKeys = keys; // 레벨업 선택지 목록

        foreach(string key in keys)
        {
            if ((string)LevelUpData[8][key] == "P") // acitve, passive 구분
            {
                passiveKeys.Add(key);
            }
            else
            {
                activeKeys.Add(key);
            }

            MaxSkillLevel.Add(key, (int)LevelUpData[9][key]); // 최대 레벨 목록 초기화
        }

        SkillLevelText.text = "";
    }

    public void RandomSelect(int number) // 버튼에 표시할 랜덤한 텍스트를 뽑기
    {
        if (isSelectLimit)
        {
            ButtonList[0].gameObject.SetActive(true);
            SetButtonText();
            return;
        }

        RandomIdxList.Clear();

        if(isMaximum == true)
        {
            if(SelectSkillList.Count >= number) // 선택지가 n개 보다 많을때는 랜덤하게 뽑기
            {
                for (int i = 0; i < number; i++) // 이미 선택한 목록에서 뽑기
                {
                    string key = SelectSkillList[Random.Range(0, SelectSkillList.Count)];

                    while (RandomIdxList.Contains(key))
                    {
                        key = SelectSkillList[Random.Range(0, SelectSkillList.Count)];
                    }

                    RandomIdxList.Add(key);
                }
            }
            else if(SelectSkillList.Count < number)
            {
                for (int i = 0; i < SelectSkillList.Count; i++) // 남은게 n개보다 적을때는 있는거 그대로 넣기
                {
                    RandomIdxList.Add(SelectSkillList[i]);

                    ButtonList[SelectSkillList.Count].gameObject.SetActive(false);
                }
            }
        }
        else if(isMaximum == false) // 꽉찬게 아니라면 모든 목록에서 랜덤하게 뽑기
        {
            for (int i = 0; i < number; i++) // 랜덤한 n개의 선택지를 뽑기
            {
                List<string> targetSkill;
                if (activeCount == MAXCOUNT)
                {
                    targetSkill = passiveKeys;
                }
                else if(passiveCount == MAXCOUNT)
                {
                    targetSkill = activeKeys;
                }
                else
                {
                    if (Random.Range(0, activeKeys.Count + passiveKeys.Count - 1) > passiveKeys.Count) // passive 뽑는 경우
                    {
                        targetSkill = passiveKeys;
                    }
                    else
                    {
                        targetSkill = activeKeys;
                    }
                }

                string key = targetSkill[Random.Range(0, targetSkill.Count)];

                while (RandomIdxList.Contains(key))
                {
                    key = targetSkill[Random.Range(0, targetSkill.Count)];
                }

                RandomIdxList.Add(key);
            }
        }

        SetButtonText();
    }

    private void SetButtonText()
    {
        if(isSelectLimit == true)
        {
            ButtonList[0].GetComponentInChildren<TextMeshProUGUI>().text = "Score<br>스코어 +15";
            return;
        }

        if(isMaximum == false)
        {
            for (int i = 0; i < 3; i++)
            {
                string key = RandomIdxList[i];
                if (SkillLevel.ContainsKey(key))
                {
                    ButtonList[i].GetComponentInChildren<TextMeshProUGUI>().text =
                        key + "<br>" + LevelUpData[SkillLevel[key]][key].ToString();
                }
                else
                {
                    ButtonList[i].GetComponentInChildren<TextMeshProUGUI>().text =
                        key + "<br>" + LevelUpData[0][key].ToString();
                }
            }
        }
        else if(isMaximum == true)
        {
            if (SelectSkillList.Count > 3) // 선택지가 n개 보다 많을때는 랜덤하게 뽑기
            {
                for (int i = 0; i < 3; i++)
                {
                    string key = RandomIdxList[i];
                    ButtonList[i].GetComponentInChildren<TextMeshProUGUI>().text =
                        key + "<br>" + LevelUpData[SkillLevel[key]][key].ToString();
                }
            }
            else if (SelectSkillList.Count <= 3)
            {
                for (int i = 0; i < SelectSkillList.Count; i++) // 남은게 n개보다 적을때는 있는거 그대로 넣기
                {
                    string key = SelectSkillList[i];
                    ButtonList[i].GetComponentInChildren<TextMeshProUGUI>().text =
                        key + "<br>" + LevelUpData[SkillLevel[key]][key].ToString();
                }
            }
        }
    }

    public void ButtonClick()
    {
        GameObject clickObject = EventSystem.current.currentSelectedGameObject;
        string skill = clickObject.GetComponentInChildren<TextMeshProUGUI>().text.Split("<br>")[0];
        LevelUpManager.instance.SendMessage(skill + "Increase");

        if (isSelectLimit == true)
        {
            ButtonList[0].gameObject.SetActive(false);
            return;
        }

        SkillLevelUp(skill);

        //Debug.Log("현재 고른 수" + SelectOptionList.Count);
    }

    public void SkillLevelUp(string Skill)
    {
        if (SkillLevel.ContainsKey(Skill))
        {
            SkillLevel[Skill]++;

            if(SkillLevel[Skill] == MaxSkillLevel[Skill])
            {
                SelectSkillList.Remove(Skill);

                if(SkillLevel.Count == activeCount + passiveCount && SelectSkillList.Count == 0)
                {
                    isSelectLimit = true;
                }
            }
        }
        else
        {
            SkillLevel.Add(Skill, 1);
            SelectSkillList.Add(Skill);

            if (Skill.Contains("Attack"))
            {
                activeCount++;
            }
            else
            {
                passiveCount++;
            }
        }

        if (activeCount == MAXCOUNT && passiveCount == MAXCOUNT)
        {
            isMaximum = true;
        }

        SkillLevelText.text = "";

        foreach (var keyValue in SkillLevel)
        {
            SkillLevelText.text += keyValue.Key + " : " + keyValue.Value + "<br>";
        }
    }
}
