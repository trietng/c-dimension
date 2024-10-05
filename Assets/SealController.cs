using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SealController : MonoBehaviour
{
    void Update()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");
        if (targets.Length == 0)
        {
            Destroy(gameObject);
        }
    }
}
