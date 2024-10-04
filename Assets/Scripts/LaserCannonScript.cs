using System;
using System.Collections;
using UnityEngine;

public class LaserCannonScript : MonoBehaviour
{
    private static readonly float MAX_DISTANCE = 1000f;
    private RaycastHit[] hits = new RaycastHit[32];
    public Transform muzzle;
    public GameObject shotPrefab;
    private GameObject shot;
    private VolumetricLines.VolumetricLineBehavior lineBehavior;
    private BoxCollider shotCollider;

    void Start()
    {
        shot = Instantiate(shotPrefab, muzzle.position, transform.rotation);
        lineBehavior = shot.GetComponent<VolumetricLines.VolumetricLineBehavior>();
        shotCollider = shot.GetComponent<BoxCollider>();
    }

    void FixedUpdate()
    {
        Fire();
    }

    public void Fire()
    {
        // Perform raycast and get the number of hits
        int hitCount = Physics.RaycastNonAlloc(muzzle.position, -muzzle.forward, hits, MAX_DISTANCE);

        // If there are no hits, return early
        if (hitCount == 0)
        {
            return;
        }

        Vector3 destination = hits[0].point;

        // Iterate over the hits from the closest to farthest
        for (int j = 0; j < hitCount; j++)
        {
            // If the hit object has the "BlockLaser" tag, stop the laser at the hit point
            if (hits[j].collider.gameObject.CompareTag("BlockLaser"))
            {
                destination = hits[j].point;
                break;
            }
        }

        // Debug line to visualize the ray
        Debug.DrawLine(muzzle.position, destination, Color.red, 1f);

        // Set laser line visual positions
        lineBehavior.StartPos = muzzle.localPosition;
        float distance = Vector3.Distance(muzzle.position, destination) / shot.transform.localScale.z;
        lineBehavior.EndPos = new Vector3(
            muzzle.localPosition.x,
            muzzle.localPosition.y,
            muzzle.localPosition.z - distance
        );

        // Update collider size and position to match the laser
        shotCollider.center = new Vector3(
            shotCollider.center.x,
            shotCollider.center.y,
            -distance / 2
        );
        shotCollider.size = new Vector3(
            shotCollider.size.x,
            shotCollider.size.y,
            distance
        );
    }
}
