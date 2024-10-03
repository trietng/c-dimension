using UnityEngine;

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
        // Optionally initialize any other components or variables here if needed
    }

    public void CheckAllPads()
    {
        if (triggerCounts == 3)
        {
            StageComplete.Instance.SetUp();
        }
        else
        {
            StageComplete.Instance.endScreenCanvas.SetActive(false);
        }
    }
}
