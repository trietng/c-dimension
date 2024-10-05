using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public class WeatherScript : MonoBehaviour {
    public enum WeatherType {
        Clear,
        Rain,
        Snow
    }

    public Material rainSkyboxMaterial;
    private ParticleSystem rain;
    private Light lightning;
    private ParticleSystem snow;
    private Light sun;
    private GameObject player;
    private AudioSource[] audioSource;
    public WeatherType altWeatherType = WeatherType.Clear;
    public AudioClip rainSound;
    public AudioClip lightningSound;
    public AudioClip snowSound;
    public int RainChance = 35;
    public int SnowChance = 100;
    public bool followPlayer = true;

    void Start() {
        rain = transform.GetChild(0).GetComponent<ParticleSystem>();
		lightning = transform.GetChild(1).GetComponent<Light>();
        snow = transform.GetChild(2).GetComponent<ParticleSystem>();
        sun = GameObject.Find("Sun").GetComponent<Light>();
        player = GameObject.FindGameObjectWithTag("Player");
        audioSource = GetComponents<AudioSource>();
        rain.Stop();
        snow.Stop();
        ChangeWeather();
    }

    IEnumerator FollowPlayer(int y) {
        while (true) {
            transform.position = new Vector3(player.transform.position.x, y, player.transform.position.z);
            yield return new WaitForSeconds(1);
        }
    }

    void ChangeWeather()
    {
        // Weather always starts with a clear sky
        int dice = UnityEngine.Random.Range(1, 100);
        // 35% chance of rain
        // dice = 90; // DEBUG
        switch (altWeatherType) {
            case WeatherType.Rain:
                if (dice <= RainChance) {
                    Rain();
                }
                break;
            case WeatherType.Snow:
                if (dice <= SnowChance) {
                    Snow();
                }
                break;
            default:
                break;
        }
    }
    
    void Snow() {
        snow.Play();
        audioSource[0].loop = true;
        audioSource[0].clip = snowSound;
        audioSource[0].Play();
        sun.intensity = 0f;
        if (followPlayer)
        {
            StartCoroutine(FollowPlayer(12));
        }
    }

    void Rain() {
        rain.Play();
        audioSource[0].loop = true;
        audioSource[0].clip = rainSound;
        audioSource[0].Play();
        if (Camera.main.TryGetComponent(out Skybox skybox)) {
            skybox.material = rainSkyboxMaterial;
        }
        sun.intensity = 0f;
        if (followPlayer)
        {
            StartCoroutine(FollowPlayer(18));
        }
        StartCoroutine(Lightning());
    }

    IEnumerator Lightning() {
        while (true) {
            // Look at some random point below the current position
            lightning.transform.eulerAngles = new Vector3(
                UnityEngine.Random.Range(30, 150), 
                UnityEngine.Random.Range(0, 360),
                UnityEngine.Random.Range(0, 360)
            );
            lightning.enabled = true;
            audioSource[1].PlayOneShot(lightningSound);
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1.0f));
            lightning.enabled = false;
            yield return new WaitForSeconds(UnityEngine.Random.Range(10, 30));
        }
    }
}