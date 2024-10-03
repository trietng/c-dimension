using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject endScreenCanvas;
    public TriggerPad[] triggerPads;

    private void Start()
    {
        endScreenCanvas.SetActive(false);
    }

    public void CheckAllPads()
    {
        bool allTriggered = true;

        foreach (TriggerPad pad in triggerPads)
        {
            if (!pad.IsTriggered())
            {
                allTriggered = false;
                break;
            }
        }

        if (allTriggered)
        {
            endScreenCanvas.SetActive(true);
        }
        else
        {
            endScreenCanvas.SetActive(false);
        }
    }
}
