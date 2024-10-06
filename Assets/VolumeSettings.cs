using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer musicMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {   
        initialize();
        musicSlider.onValueChanged.AddListener((v) => {
            SetMusicVolume(v);
        });

        sfxSlider.onValueChanged.AddListener((v) => {
            SetSFXVolume(v);
        });
    }
    public void SetMusicVolume(float volume = -1)
    {
        if (volume < 0) volume = musicSlider.value;
        musicMixer.SetFloat("musicParam", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    private void OnEnable () {
        initialize();
    }

    private void initialize () {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadMusicVolume();
        }
        else
        {
            musicSlider.value = 0.75f;
            SetMusicVolume();
        }

        if (PlayerPrefs.HasKey("sfxVolume"))
        {
            LoadSFXVolume();
        }
        else
        {
            sfxSlider.value = 0.75f;
            SetSFXVolume();
        }
    }

    public void SetSFXVolume(float volume = -1)
    {
        if (volume < 0) volume = sfxSlider.value;
        musicMixer.SetFloat("sfxParam", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    private void LoadMusicVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        SetMusicVolume();
    
    }

    public void LoadSFXVolume()
    {
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        SetSFXVolume();
    }

}
