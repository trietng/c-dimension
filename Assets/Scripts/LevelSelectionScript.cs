using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class LevelsInformation {
    public string name = "Coming Soon";
    public string description = "Stay tuned for updates!";

    public bool playable = false;
}

public class LevelSelectionScript : MonoBehaviour, ISerializationCallbackReceiver
{
    [SerializeField] string[] levelNames = new string[6];
    [SerializeField] string[] levelDescriptions = new string[6];
    [SerializeField] bool[] levelPlayable = new bool[6];

    [SerializeField] int currentLevel = 0;

    [SerializeField] GameObject gameMaster;

    [SerializeField] public bool inactive = false;
    
    public LevelsInformation[] levels = new LevelsInformation[6];

    private Button[] levelButtons = new Button[6];

    private Button playButton;

    private TextMeshProUGUI levelNameElement, levelDescriptionElement;

    public void OnBeforeSerialize()
    {

    }

    public void OnAfterDeserialize()
    {
        levels = new LevelsInformation[6];

        for (int i = 0; i < 6; ++i) {
            LevelsInformation level = new LevelsInformation();
            if (levelNames[i] != "") level.name = levelNames[i];
            if (levelDescriptions[i] != "") level.description = levelDescriptions[i];
            level.playable = levelPlayable[i];

            levels[i] = level;
        }
    }

    private void loadLevelInfo (int levelID) {
        if (levelID < 0 || levelID >= 6) return;
        currentLevel = levelID;

        foreach (Button button in levelButtons) {
            button.GetComponentInChildren<TextMeshProUGUI>().fontStyle = TMPro.FontStyles.Normal;
        }

        levelButtons[currentLevel].GetComponentInChildren<TextMeshProUGUI>().fontStyle = TMPro.FontStyles.Bold;

        levelNameElement.GetComponentInChildren<TextMeshProUGUI>().text = levels[currentLevel].name;
        levelDescriptionElement.GetComponentInChildren<TextMeshProUGUI>().text = levels[currentLevel].description;
        
        if (levels[currentLevel].playable) {
            playButton.GetComponentInChildren<TextMeshProUGUI>().text = "PLAY LEVEL";
            playButton.GetComponent<ButtonAnchorScript>().unclickable = false;
        }
        else {
            playButton.GetComponentInChildren<TextMeshProUGUI>().text = "LOCKED";
            playButton.GetComponent<ButtonAnchorScript>().unclickable = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // get level buttons
        Button[] buttons = transform.GetComponentsInChildren<Button>();
        for (int i = 0; i < 6; ++i) {
            int _this = i;
            Button button = Array.Find(buttons, x => x.name.Contains("C0" + (i + 1).ToString() + "Button"));
            button.onClick.AddListener(() => {
                if (inactive) return;
                loadLevelInfo(_this);
            });
            levelButtons[i] = button;
        }

        // get texts
        TextMeshProUGUI[] texts = transform.GetComponentsInChildren<TextMeshProUGUI>();
        levelNameElement = Array.Find(texts, x => x.name.Contains("LevelName"));
        levelDescriptionElement = Array.Find(texts, x => x.name.Contains("LevelDescription"));

        // get the play button
        playButton = Array.Find(buttons, x => x.name.Contains("PlayButton"));
        playButton.onClick.AddListener(() => {
            if (inactive) return;
            if (!levels[currentLevel].playable) return;
            Debug.Log("Selected level: " + currentLevel.ToString());
        });

        // get back button
        Button backButton = Array.Find(buttons, x => x.name.Contains("BackButton"));
        backButton.onClick.AddListener(() => {
            if (inactive) return;
            inactive = true;
            StartCoroutine(backToMenu());
        });

        // load level 1
        loadLevelInfo(0);
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
        
    }
}
