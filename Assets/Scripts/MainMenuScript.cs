using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] Button settingsButton;
    [SerializeField] Button QuitButton;
    // Start is called before the first frame update

    private Button[] buttons = new Button[3];
    private string[] text = {"PLAY", "SETTINGS", "QUIT"};
    void Start()
    {
        buttons = new Button[3] {playButton, settingsButton, QuitButton};
        for (int i = 0; i < buttons.Length; ++i) {
            int _thisContext = i;
            buttons[i].onClick.AddListener(() => {
                Debug.Log("Clicked: " + text[_thisContext]);
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
