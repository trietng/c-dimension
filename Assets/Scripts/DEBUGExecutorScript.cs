using System;
using System.Collections;
using UnityEngine;

public class DEBUGExecutorScript : MonoBehaviour {
    public GameObject debugObject;
    public float delay = 1f;

    void Start() {
        Debug.Log("DEBUGExecutorScript is running");
        StartCoroutine(Execute());
    }

    // Perform some debugging operations
    IEnumerator Execute() {
        yield return new WaitForSeconds(delay);
        StartCoroutine(debugObject.GetComponent<TargetDisintegrationScript>().Disintegrate());
    }
}