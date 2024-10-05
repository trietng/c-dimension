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

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            if (other.gameObject.TryGetComponent<TargetDisintegrationScript>(out var target))
            {
                StartCoroutine(target.Disintegrate());
            }
        }
        if (!other.gameObject.CompareTag("PlayerObj") && !other.gameObject.CompareTag("Projectile"))
        {
            Destroy(gameObject);
        }
    }
}
