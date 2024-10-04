using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class StageComplete : MonoBehaviour
{
    public static StageComplete Instance { get; private set; }

    public TMP_Text warningText;
    public GameObject endScreenCanvas;
    public float warningDuration = 2f;
    public float fadeDuration = 1f;

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
        endScreenCanvas.SetActive(false);
        warningText.gameObject.SetActive(false);
    }

    public void SetUp()
    {
        Time.timeScale = 0f;
        endScreenCanvas.SetActive(true);
    }

    public void GoToMenu()
    {
        GameManager.Instance.triggerCounts = 0;
        Time.timeScale = 1f;
        StartCoroutine(loadScene(0));
    }

    public void RestartStage()
    {
        GameManager.Instance.triggerCounts = 0;
        Time.timeScale = 1f;
        
        StartCoroutine(loadScene(SceneManager.GetActiveScene().buildIndex));
    }

    public void NextLevel()
    {
        GameManager.Instance.triggerCounts = 0;
        Time.timeScale = 1f;
        int nextLevelSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextLevelSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            StartCoroutine(loadScene(nextLevelSceneIndex));
        }
        else
        {
            StartCoroutine(ShowWarning());
        }
    }

    IEnumerator loadScene (int ID) {
        GameObject loadingScreen = Array.Find(GameObject.FindObjectsOfType<GameObject>(true), s => s.name.Contains("LoadingScreen"));
        LoadingCanvasScript loadScript = loadingScreen.GetComponent<LoadingCanvasScript>();
        loadScript.show();

        yield return new WaitUntil(() => loadScript.visible);

        // load main menu (but still keep loading screen)
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(ID);
        asyncLoad.completed += (e) => {
            // hide loading screen
            endScreenCanvas.SetActive(false);
            loadScript.hide();
            // loadScript.hide();
        };
    }

    private IEnumerator ShowWarning()
    {
        warningText.gameObject.SetActive(true);
        SetTextAlpha(1f);
        yield return new WaitForSeconds(warningDuration);
        yield return StartCoroutine(FadeOutText());
        warningText.gameObject.SetActive(false);
    }

    private IEnumerator FadeOutText()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            SetTextAlpha(alpha);
            yield return null;
        }
        SetTextAlpha(0f);
    }

    private void SetTextAlpha(float alpha)
    {
        Color color = warningText.color;
        color.a = alpha;
        warningText.color = color;
    }
}
