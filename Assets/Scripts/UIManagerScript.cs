using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManagerScript : MonoBehaviour
{
    // add callbacks in the inspector like for buttons
    [SerializeField] public Canvas mainMenuCanvas;
    [SerializeField] public Canvas levelSelectionCanvas;

    [SerializeField] public Canvas settingsPanel;

    [SerializeField] public GameObject worldCube;

    [SerializeField] public Vector3 initialPosition = new Vector3(50, 0, -100);
    
    [SerializeField] public Camera camera;

    [SerializeField] public float movingSpeed = 2f;

    private Vector3 targetPosition;

    void Start() {
        targetPosition = initialPosition;
        resetCamera();
    }

    void Update () {
        if (Vector3.Distance(targetPosition, camera.transform.position) >= 1E-3f) {
            Vector3 dir = (targetPosition - camera.transform.position) * Time.deltaTime * movingSpeed;
            camera.transform.Translate(dir.x, dir.y, dir.z);
        }
    }

    public void resetCamera () {
        setCameraPos(initialPosition);
    }

    public void setCameraPos (Vector3 pos) {
        Vector3 path = pos - camera.transform.position;
        camera.transform.Translate(path.x, path.y, path.z);
    }

    public void moveCameraTo (Vector3 position) {
        targetPosition = position;
    }
}