using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class SettingsPanelScript : MonoBehaviour
{
    [SerializeField] public bool inactive = false;

    [SerializeField] public GameObject gameMaster;

    [SerializeField] public Button backButton;

    // Start is called before the first frame update
    void Start()
    {
        // get back button
        backButton.onClick.AddListener(() => {
            if (inactive) return;
            setInactive();
            StartCoroutine(backToMenu());
        });
    }

    public void setActive () {
        inactive = false;
        Slider[] sliders = transform.GetComponentsInChildren<Slider>(true);
        foreach (Slider sl in sliders) sl.gameObject.SetActive(true);
    }

    public void setInactive () {
        inactive = true;
        Slider[] sliders = transform.GetComponentsInChildren<Slider>(true);
        foreach (Slider sl in sliders) sl.gameObject.SetActive(false);
    }

    IEnumerator backToMenu () {
        FadingEffectsScript mainMenuScript = transform.GetComponent<FadingEffectsScript>();
        mainMenuScript.hide();
        yield return new WaitUntil(() => !mainMenuScript.visible);
        gameMaster.GetComponent<UIManagerScript>().mainMenuCanvas.GetComponent<FadingEffectsScript>().show();
        gameMaster.GetComponent<UIManagerScript>().mainMenuCanvas.GetComponent<MainMenuScript>().inactive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!inactive && Input.GetKeyDown(KeyCode.Escape)) {
            setInactive();
            StartCoroutine(backToMenu());
        }
    }
}
