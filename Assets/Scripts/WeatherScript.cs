using System;
using System.Collections;
using UnityEngine;

public class WeatherScript : MonoBehaviour {
    public Material rainSkyboxMaterial;
    private ParticleSystem rain;
    private Light lightning;
    private Light sun;
    private GameObject player;
    private AudioSource[] audioSource;
    public AudioClip rainSound;
    public AudioClip lightningSound;

    void Start() {
        rain = transform.GetChild(0).GetComponent<ParticleSystem>();
		lightning = transform.GetChild(1).GetComponent<Light>();
        sun = GameObject.Find("Sun").GetComponent<Light>();
        player = GameObject.FindGameObjectWithTag("Player");
        audioSource = GetComponents<AudioSource>();
        rain.Stop();
        ChangeWeather();
    }

    IEnumerator FollowPlayer() {
        while (true) {
            transform.position = new Vector3(player.transform.position.x, 18, player.transform.position.z);
            yield return new WaitForSeconds(1);
        }
    }

    void ChangeWeather()
    {
        // Weather always starts with a clear sky
        int dice = UnityEngine.Random.Range(1, 100);
        // 35% chance of rain
        // dice = 90; // DEBUG
        if (dice > 65) {
            Rain();
        }
    }

    void Rain() {
        rain.Play();
        audioSource[0].loop = true;
        audioSource[0].clip = rainSound;
        audioSource[0].Play();
        sun.intensity = 0f;
        Camera.main.GetComponent<Skybox>().material = rainSkyboxMaterial;
        StartCoroutine(FollowPlayer());
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