using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool isPaused = false;

    void Start()
    {
        Time.timeScale = 1f;
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused)
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
        if (SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 3)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void toMenu()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) return;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        isPaused = false;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }
        pauseMenu.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        isPaused = false;
    }
}
