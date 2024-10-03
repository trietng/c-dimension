using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackScript : MonoBehaviour
{
    public Transform muzzle;

    public GameObject shotPrefab;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Fire at the mouse position
            if (Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit hit))
            {
                GameObject go = Instantiate(shotPrefab, muzzle.position, muzzle.rotation);
                go.transform.LookAt(hit.point);
                Destroy(go, 30f);
            }
        }
    }
}