using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCtrl : MonoBehaviour
{
    public void Start()
    {
        Scene scene = SceneManager.GetActiveScene();

        if(scene.name == "MainMenuScene")
        {
            SoundManager.instance.PlayBGMSound("Menu", 0.25f);
        }
        else if(scene.name == "GameScene")
        {
            SoundManager.instance.PlayBGMSound("Stage", 0.25f);
        }
    }

    public void LoadGameScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
        SoundManager.instance.PlayBGMSound("Stage", 0.25f);
    }

    public void LoadMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScene");
        SoundManager.instance.PlayBGMSound("Menu", 0.25f);
    }
}
