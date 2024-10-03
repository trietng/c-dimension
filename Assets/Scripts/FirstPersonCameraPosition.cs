using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCameraPosition : MonoBehaviour
{
    public Transform camPos;

    void Update()
    {
        transform.position = camPos.position;
    }
}
