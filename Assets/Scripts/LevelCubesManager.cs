using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCubesManager : MonoBehaviour
{
    [SerializeField] int focusLevel = -1;
    [SerializeField] float randomSpeed = 20f;
    [SerializeField] float periodTime = 5f; // 5 seconds

    private Vector3 lastAngle = Vector3.zero;
    private float lastCheckedTime = -1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Rotate (Vector3 vct) {
        transform.Rotate(vct.x, vct.y, vct.z, Space.Self);
    }

    private void RandomizeAngle () {
        int choice = Random.Range(1, 4); // 1 --> 3
        switch (choice) {
            case 1: // x
                lastAngle = new Vector3(randomSpeed, 0, 0);
                break;
            case 2: // y
                lastAngle = new Vector3(0, randomSpeed, 0);
                break;
            case 3:
                lastAngle = new Vector3(0, 0, randomSpeed);
                break;
        }
        lastCheckedTime = Time.time;
    }

    private void RotateToSurface (int levelID) {
        if (focusLevel == levelID) return;
        focusLevel = levelID;
        if (levelID == -1) {
            RandomizeAngle();
            return;
        }

        // TODO: add surface rotation
    }

    // Update is called once per frame
    void Update()
    {
        // if current not focusing on any surfaces, doing random rotation
        if (focusLevel == -1) {
            if (lastCheckedTime == -1f || Time.time - lastCheckedTime > periodTime) RandomizeAngle();
            Rotate(lastAngle * Time.deltaTime);
        }
    }
}
