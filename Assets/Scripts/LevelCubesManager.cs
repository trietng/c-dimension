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
        new Vector3(-45, 180, 0),
        new Vector3(0, 0, 0),
        new Vector3(0, 0, 0),
        new Vector3(0, 0, 0)
    };

    private Vector3 lastAngle = Vector3.zero;
    private float lastCheckedTime = -1f;

    private Vector3 lastRotDelta = Vector3.zero;

    private Vector3 targetRotation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Rotate (Vector3 vct) {
        transform.Rotate(vct.x, vct.y, vct.z, Space.Self);
    }

    private void RandomizeAngle () {
        int choice = UnityEngine.Random.Range(1, 4); // 1 --> 3
        switch (choice) {
            case 1: // x
                lastAngle = new Vector3(randomSpeed, 0, 0);
                break;
            case 2: // y
                lastAngle = new Vector3(0, randomSpeed, 0);
                break;
            case 3: // z
                lastAngle = new Vector3(0, 0, randomSpeed);
                break;
        }
        lastCheckedTime = Time.time;
    }

    private float floatModulus (float n, float d) {
        int q = (int)(n / d);
        return n - d * q;
    }

    private float toRange (float val) {
        // [-180, 180)
        return floatModulus(floatModulus(val, 360f) + 360f, 360f) - 180f;
    }

    private float closestAngle (float val1, float val2, float lastVal) {
        val1 = toRange(val1);
        val2 = toRange(val2);
        float diff = val1 - val2;
        if (diff < -180f) diff += 360f;
        if (diff >= 180f) diff -= 360f;

        return diff;
    }

    public void setTarget (Vector3 vct) {
        targetRotation = vct;
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
        else if (Vector3.Distance(targetRotation, transform.rotation.eulerAngles) >= 1E-3f) {
            Vector3 rotDist = new Vector3 (
                closestAngle(targetRotation.x, transform.rotation.eulerAngles.x, lastRotDelta.x),
                closestAngle(targetRotation.y, transform.rotation.eulerAngles.y, lastRotDelta.y),
                closestAngle(targetRotation.z, transform.rotation.eulerAngles.z, lastRotDelta.z)
            );
            Vector3 actualRot = rotDist * Time.deltaTime * focusSpeed;

            if (Math.Abs(actualRot.x) > Math.Abs(rotDist.x)) actualRot.x = rotDist.x;
            if (Math.Abs(actualRot.y) > Math.Abs(rotDist.y)) actualRot.y = rotDist.y;
            if (Math.Abs(actualRot.z) > Math.Abs(rotDist.z)) actualRot.z = rotDist.z;
            
            Rotate(actualRot);
        }
    }
}
