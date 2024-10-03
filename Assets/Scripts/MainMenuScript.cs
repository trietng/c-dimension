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
    }

    IEnumerator toSelectionScreen () {
        FadingEffectsScript mainMenuScript = transform.GetComponent<FadingEffectsScript>();
        mainMenuScript.hide();
        yield return new WaitUntil(() => !mainMenuScript.visible);
        gameMaster.GetComponent<UIManagerScript>().levelSelectionCanvas.GetComponent<FadingEffectsScript>().show();
        gameMaster.GetComponent<UIManagerScript>().levelSelectionCanvas.GetComponent<LevelSelectionScript>().inactive = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
