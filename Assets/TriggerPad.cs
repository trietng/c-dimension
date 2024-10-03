using UnityEngine;

public class TriggerPad : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            GameManager.Instance.triggerCounts++;
            GameManager.Instance.CheckAllPads();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            GameManager.Instance.triggerCounts--;
            GameManager.Instance.CheckAllPads();
        }
    }
}
