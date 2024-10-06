using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class ProgressResetMenuScript : MonoBehaviour
{
    [SerializeField] public bool inactive = false;

    [SerializeField] public GameObject gameMaster;

    [SerializeField] public Button yesButton, noButton;

    // Start is called before the first frame update
    void Start()
    {
        noButton.onClick.AddListener(() => {
            if (inactive) return;
            setInactive();
            StartCoroutine(backToMenu());
        });

        yesButton.onClick.AddListener(() => {
            if (inactive) return;
            setInactive();
            PlayerPrefs.SetInt("LevelUnlocked", 1);
            StartCoroutine(toSelectionScreen());
        });
    }

    public void setActive () {
        inactive = false;
    }

    public void setInactive () {
        inactive = true;
    }

    IEnumerator backToMenu () {
        FadingEffectsScript mainMenuScript = transform.GetComponent<FadingEffectsScript>();
        mainMenuScript.hide();
        yield return new WaitUntil(() => !mainMenuScript.visible);
        gameMaster.GetComponent<UIManagerScript>().mainMenuCanvas.GetComponent<FadingEffectsScript>().show();
        gameMaster.GetComponent<UIManagerScript>().mainMenuCanvas.GetComponent<MainMenuScript>().inactive = false;
    }

    IEnumerator toSelectionScreen () {
        FadingEffectsScript mainMenuScript = transform.GetComponent<FadingEffectsScript>();
        mainMenuScript.hide();
        yield return new WaitUntil(() => !mainMenuScript.visible);
        gameMaster.GetComponent<UIManagerScript>().levelSelectionCanvas.GetComponent<FadingEffectsScript>().show();
        gameMaster.GetComponent<UIManagerScript>().levelSelectionCanvas.GetComponent<LevelSelectionScript>().setActive();
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
