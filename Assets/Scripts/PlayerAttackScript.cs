using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackScript : MonoBehaviour
{
    public Transform muzzle;
    public GameObject shotPrefab;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Fire at the mouse position
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                GameObject go = Instantiate(shotPrefab, muzzle.position, Quaternion.identity);
                go.transform.LookAt(hit.point);
                Destroy(go, 30f);
                audioSource.Play();
            }
        }
    }
}