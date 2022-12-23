using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField]
    private GameObject SelectCharacter;

    public void ActiveSelectUI()
    {
        SelectCharacter.SetActive(true);
    }
}
