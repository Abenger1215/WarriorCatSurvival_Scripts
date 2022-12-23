using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
    public void ButtonClick()
    {
        GameManager.instance.characterName = gameObject.name;
    }
}
