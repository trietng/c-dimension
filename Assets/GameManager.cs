using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int triggerCounts = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey("LevelUnlocked"))
        {
            PlayerPrefs.SetInt("LevelUnlocked", 1);
        }
    }

    public void CheckAllPads()
    {
        if (triggerCounts == 3)
        {
            saveProgress();
            StageComplete.Instance.SetUp();
        }
        else
        {
            StageComplete.Instance.endScreenCanvas.SetActive(false);
        }
    }

    public void saveProgress()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        int unlockedLevel = PlayerPrefs.GetInt("LevelUnlocked", 1);

        PlayerPrefs.SetInt("LevelUnlocked", Math.Max(currentLevel + 1, unlockedLevel));
    }

    public void deleteProgress()
    {
        PlayerPrefs.DeleteAll();
    }
}
