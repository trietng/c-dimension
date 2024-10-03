using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject endScreenCanvas;
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
        endScreenCanvas.SetActive(false);
    }

    public void CheckAllPads()
    {
        if (triggerCounts == 3)
        {
            endScreenCanvas.SetActive(true);
        }
        else
        {
            endScreenCanvas.SetActive(false);
        }
    }
}
