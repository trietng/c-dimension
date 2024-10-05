using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaShotScript : MonoBehaviour
{
    public GameObject bulletHoleProjector;
    public float timeToDestroy = 3f;

    // Update is called once per frame
    void Update()
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
            GameObject bulletHole = Instantiate(bulletHoleProjector, transform.position, transform.rotation);
            Destroy(bulletHole, 3f);
            Destroy(gameObject);
        }
    }
}
