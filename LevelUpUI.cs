using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class LevelUpUI : MonoBehaviour
{
    List<string> SelectOptionList = new List<string>(); // 선택한 옵션 key를 가짐
    List<int> RandomIdxList = new List<int>(); // 랜덤한 idx를 가지고 있는 list

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
        UIDictKeys = KeysList; // 레벨업 선택지 목록

        for(int i = 0; i < UIDictKeys.Count; i++) // 최대 레벨 목록 초기화
        {
            MaxOptionLevel.Add(UIDictKeys[i], 0);
        }


        for (int i = 0; i < 8; i++) // 최대 레벨 몇인지 확인
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

    public void RandomSelect(int number) // 버튼에 표시할 랜덤한 텍스트를 뽑기
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
            if(SelectOptionList.Count >= number) // 선택지가 n개 보다 많을때는 랜덤하게 뽑기
            {
                for (int i = 0; i < number; i++) // 이미 선택한 목록에서 뽑기
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
                for (int i = 0; i < SelectOptionList.Count; i++) // 남은게 n개보다 적을때는 있는거 그대로 넣기
                {
                    RandomIdxList.Add(i);

                    ButtonList[SelectOptionList.Count].gameObject.SetActive(false);
                }
            }
        }
        else if(isMaximum == false) // 꽉찬게 아니라면 모든 목록에서 랜덤하게 뽑기
        {
            for (int i = 0; i < number; i++) // 랜덤한 n개의 선택지를 뽑기
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
            ButtonList[0].GetComponentInChildren<TextMeshProUGUI>().text = "Score<br>스코어 +15";
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
            if (SelectOptionList.Count > 3) // 선택지가 n개 보다 많을때는 랜덤하게 뽑기
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
                for (int i = 0; i < SelectOptionList.Count; i++) // 남은게 n개보다 적을때는 있는거 그대로 넣기
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

        //Debug.Log("현재 고른 수" + SelectOptionList.Count);
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
