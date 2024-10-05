using System;
using System.Collections;
using UnityEngine;

public class FloatingGoalControllerScript : MonoBehaviour
{
    public float amplitude = 0.001f;
    public float frequency = 3f;

    private Transform gemTransform;

    void Start()
    {
        gemTransform = transform.parent.GetChild(0);
    }

    void FixedUpdate()
    {
        gemTransform.position += new Vector3(0, Mathf.Sin(Time.time * frequency) * amplitude, 0);
    }
}