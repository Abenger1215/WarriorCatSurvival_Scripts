using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class LevelUpUI : MonoBehaviour
{
    [SerializeField]
    private List<string> SelectSkillList = new List<string>(); // ������ ��ų ���
    private List<string> RandomIdxList = new List<string>(); // ������ idx�� ������ �ִ� list

    [SerializeField]
    private List<Dictionary<string, object>> LevelUpData;

    public List<Button> ButtonList = new List<Button>();

    public List<string> UIDictKeys = new List<string>();
    public List<string> activeKeys = new List<string>();
    public List<string> passiveKeys = new List<string>();

    public Dictionary<string, int> SkillLevel = new Dictionary<string, int>(); // ������ ��ų ����
    public Dictionary<string, int> MaxSkillLevel = new Dictionary<string, int>(); // ������ ��ų �ִ� ����

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
        UIDictKeys = keys; // ������ ������ ���

        foreach(string key in keys)
        {
            if ((string)LevelUpData[8][key] == "P") // acitve, passive ����
            {
                passiveKeys.Add(key);
            }
            else
            {
                activeKeys.Add(key);
            }

            MaxSkillLevel.Add(key, (int)LevelUpData[9][key]); // �ִ� ���� ��� �ʱ�ȭ
        }

        SkillLevelText.text = "";
    }

    public void RandomSelect(int number) // ��ư�� ǥ���� ������ �ؽ�Ʈ�� �̱�
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
            if(SelectSkillList.Count >= number) // �������� n�� ���� �������� �����ϰ� �̱�
            {
                for (int i = 0; i < number; i++) // �̹� ������ ��Ͽ��� �̱�
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
                for (int i = 0; i < SelectSkillList.Count; i++) // ������ n������ �������� �ִ°� �״�� �ֱ�
                {
                    RandomIdxList.Add(SelectSkillList[i]);

                    ButtonList[SelectSkillList.Count].gameObject.SetActive(false);
                }
            }
        }
        else if(isMaximum == false) // ������ �ƴ϶�� ��� ��Ͽ��� �����ϰ� �̱�
        {
            for (int i = 0; i < number; i++) // ������ n���� �������� �̱�
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
                    if (Random.Range(0, activeKeys.Count + passiveKeys.Count - 1) > passiveKeys.Count) // passive �̴� ���
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
            ButtonList[0].GetComponentInChildren<TextMeshProUGUI>().text = "Score<br>���ھ� +15";
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
            if (SelectSkillList.Count > 3) // �������� n�� ���� �������� �����ϰ� �̱�
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
                for (int i = 0; i < SelectSkillList.Count; i++) // ������ n������ �������� �ִ°� �״�� �ֱ�
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

        //Debug.Log("���� �� ��" + SelectOptionList.Count);
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
