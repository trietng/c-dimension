using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCubesManager : MonoBehaviour
{
    [SerializeField] int focusLevel = -1;
    [SerializeField] float randomSpeed = 20f;
    [SerializeField] float periodTime = 5f; // 5 seconds

    [SerializeField] float focusSpeed = 5f;

    [SerializeField] Vector3[] faces = new Vector3[6] {
        new Vector3(45, 0, 0),
        new Vector3(0, -90, -45),
        new Vector3(-45, 0, 0),
        new Vector3(-45, 0, 180),
        new Vector3(0, 90, 45),
        new Vector3(90, 0, 0)
    };

    private Vector3 lastAngle = Vector3.zero;
    private float lastCheckedTime = -1f;

    private Vector3 lastRotDelta = Vector3.zero;
    private Quaternion startRotation = Quaternion.Euler(0, 0, 0);

    private float rotationProgress = -1;

    private Quaternion targetRotation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Rotate (Vector3 vct) {
        transform.Rotate(vct.x, vct.y, vct.z, Space.Self);
    }

    private void RandomizeAngle () {
        int choice = UnityEngine.Random.Range(1, 4); // 1 --> 3
        float speed = UnityEngine.Random.Range(0, 1) == 0 ? randomSpeed : -randomSpeed;
        switch (choice) {
            case 1: // x
                lastAngle = new Vector3(speed, 0, 0);
                break;
            case 2: // y
                lastAngle = new Vector3(0, speed, 0);
                break;
            case 3: // z
                lastAngle = new Vector3(0, 0, speed);
                break;
        }
        lastCheckedTime = Time.time;
    }

    public void setTarget (Vector3 vct) {
        targetRotation = Quaternion.Euler(vct.x, vct.y, vct.z);
        startRotation = transform.rotation;
        rotationProgress = 0;
    }

    public void RotateToSurface (int levelID) {
        if (focusLevel == levelID) return;
        focusLevel = levelID;
        if (levelID == -1) {
            RandomizeAngle();
            return;
        }

        setTarget(faces[focusLevel]);
    }

    // Update is called once per frame
    void Update()
    {
        // if current not focusing on any surfaces, doing random rotation
        if (focusLevel == -1) {
            if (lastCheckedTime == -1f || Time.time - lastCheckedTime > periodTime) RandomizeAngle();
            Rotate(lastAngle * Time.deltaTime);
        }
        else if (0 <= rotationProgress && rotationProgress < 1) {
            rotationProgress += Time.deltaTime * focusSpeed;
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, rotationProgress);

            // Rotate(actualRot);
        }
    }
}
