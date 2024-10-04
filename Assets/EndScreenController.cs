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
        Time.timeScale = 1f;
        endScreenCanvas.SetActive(false);
        SceneManager.LoadScene(0);
    }

    public void RestartStage()
    {
        Time.timeScale = 1f;
        endScreenCanvas.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        int nextLevelSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextLevelSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            endScreenCanvas.SetActive(false);
            SceneManager.LoadScene(nextLevelSceneIndex);
        }
        else
        {
            StartCoroutine(ShowWarning());
        }
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
