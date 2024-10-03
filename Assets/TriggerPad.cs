using UnityEngine;

public class TriggerPad : MonoBehaviour
{
    public GameManager gameManager;
    private bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box") && !isTriggered)
        {
            isTriggered = true;
            gameManager.CheckAllPads();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Box") && isTriggered)
        {
            isTriggered = false;
            gameManager.CheckAllPads();
        }
    }

    public bool IsTriggered()
    {
        return isTriggered;
    }
}
