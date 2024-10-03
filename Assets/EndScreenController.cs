using UnityEngine;
using UnityEngine.SceneManagement;

public class StageComplete : MonoBehaviour
{
    
    public int mainMenuSceneIndex;
    public int nextLevelSceneIndex;

    public void SetUp()
    {
        gameObject.SetActive(true);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(mainMenuSceneIndex);
    }

    public void RestartStage()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        if (nextLevelSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextLevelSceneIndex);
        }
        else
        {
            Debug.LogWarning("Next level not found! Returning to Main Menu.");
            SceneManager.LoadScene(mainMenuSceneIndex);
        }
    }
}
