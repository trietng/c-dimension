using System;
using System.Collections;
using UnityEngine;

public class FloatingControllerScript : MonoBehaviour
{
    public float amplitude = 0.01f;
    public float frequency = 3f;
    private int seed;
    private Transform firstChildTransform;

    void Start()
    {
        seed = UnityEngine.Random.Range(0, 59);
        firstChildTransform = transform.parent.GetChild(0);
    }

    void FixedUpdate()
    {
        firstChildTransform.position += new Vector3(0, Mathf.Sin((Time.time + seed) * frequency) * amplitude, 0);
    }
}