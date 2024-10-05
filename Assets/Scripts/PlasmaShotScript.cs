using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaShotScript : MonoBehaviour
{
    // Update is called once per frame
    void Update () 
    {
		transform.position += 10f * Time.deltaTime * transform.forward;
	}

    // void OnCollisionEnter(Collision collision)
    // {
    //     if (!collision.gameObject.CompareTag("Cannon") && !collision.gameObject.CompareTag("Projectile"))
    //     {
    //         Destroy(gameObject);
    //     }

    // }
}
