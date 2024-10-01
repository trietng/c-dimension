using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShotScript : MonoBehaviour
{
    // Update is called once per frame
    void Update () 
    {
		transform.position += 1f * Time.deltaTime * transform.forward;
	}

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Cannon"))
        {
            Destroy(gameObject);
        }

    }
}
