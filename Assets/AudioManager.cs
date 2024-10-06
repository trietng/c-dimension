using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clips")]
    public AudioClip backgroundMusic;
    public AudioClip movementSFX;
    public AudioClip deadSFX;

    [SerializeField] AudioClip hoverButtonSource;

    [SerializeField] AudioClip clickButtonSource;

    private void Start()
    {
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        // SFXSource.clip = clip;
        SFXSource.PlayOneShot(clip);
    }

    public void playHoverButton () {
        SFXSource.PlayOneShot(hoverButtonSource);
    }

    public void playClickButton () {
        SFXSource.PlayOneShot(clickButtonSource);
    }
}
