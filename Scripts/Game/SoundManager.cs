using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    [SerializeField] AudioClip[] bgmClips;
    [SerializeField] AudioClip[] sfxClips;
    Dictionary<string, AudioClip> bgmClipsDict = new Dictionary<string, AudioClip>();
    Dictionary<string, AudioClip> sfxClipsDict = new Dictionary<string, AudioClip>();

    [SerializeField] private AudioSource bgmPlayer;
    [SerializeField] private AudioSource sfxPlayer;


    public static SoundManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        foreach(AudioClip bgmClip in bgmClips)
        {
            bgmClipsDict.Add(bgmClip.name, bgmClip);
        }
        foreach (AudioClip sfxClip in sfxClips)
        {
            sfxClipsDict.Add(sfxClip.name, sfxClip);
        }

    }

    public void PlaySFXSound(string name, float volume = 1f)
    {
        if(sfxClipsDict.ContainsKey(name + "SFX") == false)
        {
            Debug.Log(name + " SFX is not Cotained sfxDict");
            return;
        }
        sfxPlayer.PlayOneShot(sfxClipsDict[name + "SFX"], volume);
    }

    public void PlayBGMSound(string name, float volume = 1f)
    {

        bgmPlayer.loop = true;

        if(name == "GameOver")
        {
            bgmPlayer.loop = false;
        }

        bgmPlayer.volume = volume;

        bgmPlayer.clip = bgmClipsDict[name + "BGM"];
        bgmPlayer.Play();
    }

    public void PauseBGM()
    {
        bgmPlayer.Pause();
    }

    public void UnPauseBGM()
    {
        bgmPlayer.UnPause();
    }
}
