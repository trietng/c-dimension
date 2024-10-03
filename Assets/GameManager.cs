using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject endScreenCanvas;
    public int triggerCounts = 0;

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
