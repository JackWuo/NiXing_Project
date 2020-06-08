using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    public AudioClip[] AudioClipArray;

    private static Dictionary<string, AudioClip> AudioDic = new Dictionary<string, AudioClip>();
    private static AudioSource audioBGM;
    private static AudioSource[] audioSources;


    public Slider volumeSlider;
    public float Volume { get; set; }
    public static AudioManager Instance { get => _instance; set => _instance = value; }

    void Awake()
    {
        Instance = this;
        foreach (var item in AudioClipArray)
        {
            AudioDic.Add(item.name, item);
        }
        //volumeSlider.onValueChanged.AddListener((value) => SetVolume());
        audioBGM = gameObject.AddComponent<AudioSource>();
        audioSources = GetComponents<AudioSource>();

        Volume = 50;
    }


    public void PlayEffect(string acName)
    {

        if (AudioDic.ContainsKey(acName) && !string.IsNullOrEmpty(acName))
        {
            AudioClip ac = AudioDic[acName];
            PlayEffect(ac);
        }
    }

    private void PlayEffect(AudioClip ac)
    {
        if (ac)
        {

            audioSources = gameObject.GetComponents<AudioSource>();

            for (int i = 1; i < audioSources.Length; i++)
            {
                if (!audioSources[i].isPlaying)
                {
                    audioSources[i].loop = false;
                    audioSources[i].clip = ac;
                    audioSources[i].volume = Volume;
                    audioSources[i].Play();
                    return;
                }
            }

            AudioSource newAs = gameObject.AddComponent<AudioSource>();
            newAs.loop = false;
            newAs.clip = ac;
            newAs.volume = Volume;
            newAs.Play();
        }
    }

    public void PlayBGM(string acName)
    {
        if (AudioDic.ContainsKey(acName) && !string.IsNullOrEmpty(acName))
        {
            AudioClip ac = AudioDic[acName];
            PlayBGM(ac);
        }
    }

    private void PlayBGM(AudioClip ac)
    {
        if (ac)
        {
            audioBGM.clip = ac;
            audioBGM.loop = true;
            audioBGM.volume = Volume;
            audioBGM.Play();
        }
    }

    public void StopPlayBGM()
    {
        audioBGM.Stop();
    }

    public void SetVolume()
    {
        Volume = volumeSlider.value;
        for (int i = 0; i < audioSources.Length; i++)
            audioSources[i].volume = Volume;
    }
}