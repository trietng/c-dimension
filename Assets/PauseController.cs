using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject tutorial;

    public bool isPaused = false;

    void Start()
    {
        Time.timeScale = 1f;
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }

        if (tutorial != null)
        {
            tutorial.SetActive(false);
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (tutorial.activeSelf)
                {
                    BackToPauseMenu();
                }
                else if (isPaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }
    }

    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        pauseMenu.SetActive(true);

        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        if (SceneManager.GetActiveScene().buildIndex > 1)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void ShowTutorial()
    {
        if (tutorial != null)
        {
            tutorial.SetActive(true);
            pauseMenu.SetActive(false);
        }
    }

    public void BackToPauseMenu()
    {
        if (pauseMenu != null && tutorial != null)
        {
            pauseMenu.SetActive(true);
            tutorial.SetActive(false);
        }
    }

    public void toMenu()
    {
        GameManager.Instance.triggerCounts = 0;
        if (SceneManager.GetActiveScene().buildIndex == 0) return;
        StartCoroutine(loadScene(0));
    }

    IEnumerator loadScene (int ID) {
        Time.timeScale = 1f;
        if (pauseMenu != null) pauseMenu.SetActive(false);
        GameObject loadingScreen = Array.Find(GameObject.FindObjectsOfType<GameObject>(true), s => s.name.Contains("LoadingScreen"));
        LoadingCanvasScript loadScript = loadingScreen.GetComponent<LoadingCanvasScript>();
        loadScript.show();

        yield return new WaitUntil(() => loadScript.visible);

        // load main menu (but still keep loading screen)
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(ID);
        asyncLoad.completed += (e) => {
            // hide loading screen
            loadScript.hide();
            // loadScript.hide();
        };
        isPaused = false;
    }

    public void Restart()
    {
        GameManager.Instance.triggerCounts = 0;
        StartCoroutine(loadScene(SceneManager.GetActiveScene().buildIndex));
    }
}
