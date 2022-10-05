using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class LevelUpUI : MonoBehaviour
{
    List<string> SelectOptionList = new List<string>(); // ������ �ɼ� key�� ����
    List<int> RandomIdxList = new List<int>(); // ������ idx�� ������ �ִ� list

    List<Dictionary<string, object>> LevelUpData;

    public List<Button> ButtonList = new List<Button>();

    public List<string> UIDictKeys = new List<string>();
    public Dictionary<string, int> OptionLevel = new Dictionary<string, int>();
    public Dictionary<string, int> MaxOptionLevel = new Dictionary<string, int>();

    public TextMeshProUGUI OptionLevelText;

    int OptionMaximum = 5;
    bool isMaximum = false;
    bool isSelectLimit = false;

    private void Start()
    {
        LevelUpData = CSVReader.Read("LevelUpData");

        List<string> KeysList = new List<string>(LevelUpData[0].Keys);
        UIDictKeys = KeysList; // ������ ������ ���

        for(int i = 0; i < UIDictKeys.Count; i++) // �ִ� ���� ��� �ʱ�ȭ
        {
            MaxOptionLevel.Add(UIDictKeys[i], 0);
        }


        for (int i = 0; i < 8; i++) // �ִ� ���� ������ Ȯ��
        {
            foreach (var tmp in LevelUpData[i])
            {
                if(tmp.Value.ToString() != "")
                {
                    MaxOptionLevel[tmp.Key]++;
                }
            }
        }

        OptionLevelText.text = "";
    }

    public void RandomSelect(int number) // ��ư�� ǥ���� ������ �ؽ�Ʈ�� �̱�
    {
        Debug.Log("isSelectLimit : " + isSelectLimit);
        if (isSelectLimit)
        {
            ButtonList[0].gameObject.SetActive(true);
            SetButtonText();
            return;
        }

        RandomIdxList.Clear();

        if(isMaximum == true)
        {
            if(SelectOptionList.Count >= number) // �������� n�� ���� �������� �����ϰ� �̱�
            {
                for (int i = 0; i < number; i++) // �̹� ������ ��Ͽ��� �̱�
                {
                    int idx = Random.Range(0, SelectOptionList.Count);
                    while (RandomIdxList.Contains(idx))
                    {
                        idx = Random.Range(0, SelectOptionList.Count);
                    }
                    RandomIdxList.Add(idx);
                }
            }
            else if(SelectOptionList.Count < number)
            {
                for (int i = 0; i < SelectOptionList.Count; i++) // ������ n������ �������� �ִ°� �״�� �ֱ�
                {
                    RandomIdxList.Add(i);

                    ButtonList[SelectOptionList.Count].gameObject.SetActive(false);
                }
            }
        }
        else if(isMaximum == false) // ������ �ƴ϶�� ��� ��Ͽ��� �����ϰ� �̱�
        {
            for (int i = 0; i < number; i++) // ������ n���� �������� �̱�
            {
                int idx = Random.Range(0, UIDictKeys.Count);
                while (RandomIdxList.Contains(idx))
                {
                    idx = Random.Range(0, UIDictKeys.Count);
                }
                RandomIdxList.Add(idx);
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
                string key = UIDictKeys[RandomIdxList[i]];
                if (OptionLevel.ContainsKey(key))
                {
                    ButtonList[i].GetComponentInChildren<TextMeshProUGUI>().text =
                        key + "<br>" + LevelUpData[OptionLevel[key]][key].ToString();
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
            if (SelectOptionList.Count > 3) // �������� n�� ���� �������� �����ϰ� �̱�
            {
                for (int i = 0; i < 3; i++)
                {
                    string key = SelectOptionList[RandomIdxList[i]];
                    ButtonList[i].GetComponentInChildren<TextMeshProUGUI>().text =
                        key + "<br>" + LevelUpData[OptionLevel[key]][key].ToString();
                }
            }
            else if (SelectOptionList.Count <= 3)
            {
                for (int i = 0; i < SelectOptionList.Count; i++) // ������ n������ �������� �ִ°� �״�� �ֱ�
                {
                    string key = SelectOptionList[i];
                    ButtonList[i].GetComponentInChildren<TextMeshProUGUI>().text =
                        key + "<br>" + LevelUpData[OptionLevel[key]][key].ToString();
                }
            }
        }
    }

    public void ButtonClick()
    {
        GameObject clickObject = EventSystem.current.currentSelectedGameObject;
        string option = clickObject.GetComponentInChildren<TextMeshProUGUI>().text.Split("<br>")[0];
        // Debug.Log(option);
        GameManager.instance.RunMethod(option + "Increase");

        if (isSelectLimit == true)
        {
            ButtonList[0].gameObject.SetActive(false);
            return;
        }

        SelectLevelOption(option);

        if (SelectOptionList.Count == OptionMaximum)
        {
            isMaximum = true;
        }

        OptionLevelText.text = "";

        foreach(var keyValue in OptionLevel)
        {
            OptionLevelText.text += keyValue.Key + " : " + keyValue.Value + "<br>";
        }

        //Debug.Log("���� �� ��" + SelectOptionList.Count);
    }

    private void SelectLevelOption(string option)
    {
        if (OptionLevel.ContainsKey(option))
        {
            OptionLevel[option]++;

            if(OptionLevel[option] == MaxOptionLevel[option])
            {
                SelectOptionList.Remove(option);

                if(OptionLevel.Count == 5 && SelectOptionList.Count == 0)
                {
                    isSelectLimit = true;
                }
            }
        }
        else
        {
            OptionLevel.Add(option, 1);
            SelectOptionList.Add(option);
        }
    }
}
