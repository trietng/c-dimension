using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] GameObject gameMaster;
    [SerializeField] Button playButton;

    [SerializeField] Button tutorialButton;
    [SerializeField] Button settingsButton;
    [SerializeField] Button quitButton;

    [SerializeField] public bool inactive = false;

    // Start is called before the first frame update
    void Start()
    {
        playButton.onClick.AddListener(() => {
            if (inactive) return;
            inactive = true;
            StartCoroutine(toSelectionScreen());
        });

        tutorialButton.onClick.AddListener(() => {
            if (inactive) return;
            inactive = true;
            StartCoroutine(toTutorialScreen());
        });

        settingsButton.onClick.AddListener(() => {
            if (inactive) return;
            inactive = true;
            StartCoroutine(showSettings());
        });

        quitButton.onClick.AddListener(() => {
            if (inactive) return;
            inactive = true;
            StartCoroutine(byeBye());
        });
    }

    IEnumerator showSettings () {
        FadingEffectsScript mainMenuScript = transform.GetComponent<FadingEffectsScript>();
        mainMenuScript.hide();
        yield return new WaitUntil(() => !mainMenuScript.visible);
        gameMaster.GetComponent<UIManagerScript>().settingsPanel.GetComponent<FadingEffectsScript>().show();
        gameMaster.GetComponent<UIManagerScript>().settingsPanel.GetComponent<SettingsPanelScript>().setActive();
    }

    IEnumerator toSelectionScreen () {
        FadingEffectsScript mainMenuScript = transform.GetComponent<FadingEffectsScript>();
        mainMenuScript.hide();
        yield return new WaitUntil(() => !mainMenuScript.visible);
        gameMaster.GetComponent<UIManagerScript>().levelSelectionCanvas.GetComponent<FadingEffectsScript>().show();
        gameMaster.GetComponent<UIManagerScript>().levelSelectionCanvas.GetComponent<LevelSelectionScript>().setActive();
    }

    IEnumerator toTutorialScreen () {
        FadingEffectsScript mainMenuScript = transform.GetComponent<FadingEffectsScript>();
        mainMenuScript.hide();
        yield return new WaitUntil(() => !mainMenuScript.visible);
        gameMaster.GetComponent<UIManagerScript>().tutorialCanvas.GetComponent<FadingEffectsScript>().show();
        gameMaster.GetComponent<UIManagerScript>().tutorialCanvas.GetComponent<SettingsPanelScript>().setActive();
    }

    IEnumerator byeBye () {
        // hide menu
        FadingEffectsScript mainMenuScript = transform.GetComponent<FadingEffectsScript>();
        mainMenuScript.hide();

        // start fading volume
        GameObject.Find("Audio Manager").GetComponent<AudioManager>().fadeMusic();

        // go away from the cube
        UIManagerScript script = gameMaster.GetComponent<UIManagerScript>();
        script.moveCameraTo(new Vector3(script.worldCube.transform.position.x, script.initialPosition.y, script.initialPosition.z));
        yield return new WaitUntil(() => !mainMenuScript.visible);
        yield return new WaitForSeconds(0.5f);
        // enable back if also move z
        // script.moveCameraTo(new Vector3(0, 0, script.initialPosition.z));
        // yield return new WaitForSeconds(1.0f);

        // move far away from cube in z axis + start showing black screen
        script.byeBye.GetComponent<FadingEffectsScript>().show();
        script.movingSpeed = 0.1f;
        script.moveCameraTo(new Vector3(script.worldCube.transform.position.x, script.worldCube.transform.position.y, -6969));

        yield return new WaitForSeconds(1f);
        
        // yeet
        Application.Quit();
    }

    public void ChangeStatusActive () {
        inactive = !inactive;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
