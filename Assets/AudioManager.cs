using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clips")]
    public AudioClip[] backgroundMusic;
    public AudioClip movementSFX;
    public AudioClip deadSFX;

    [SerializeField] AudioClip hoverButtonSource;

    [SerializeField] AudioClip clickButtonSource;

    [SerializeField] float volumeFadingSpeed = 0.8f;

    private bool inLower = false;

    private void Start()
    {
        // make sure there is only one audio manager running at a time
        GameObject existingAudioManager = GameObject.Find("Audio Manager");
        if (existingAudioManager != transform.gameObject) {
            Destroy(transform.gameObject);
            return;
        }
        
        DontDestroyOnLoad(transform.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        startMusic();
    }

    private void Update () {
        if (inLower && musicSource != null && musicSource.volume > 0) {
            musicSource.volume -= Time.deltaTime * volumeFadingSpeed;
        }
    }

    void OnSceneLoaded (Scene scene, LoadSceneMode mode) {
        startMusic();
    }

    private void startMusic () {
        musicSource.clip = backgroundMusic[UnityEngine.Random.Range(0, backgroundMusic.Length)];
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

    public void fadeMusic () {
        inLower = true;
    }
}
