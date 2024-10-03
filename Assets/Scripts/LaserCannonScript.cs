using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class LaserCannonScript : MonoBehaviour
{
    private static readonly float MAX_DISTANCE = 1000f;
    private readonly RaycastHit[] hits = new RaycastHit[32];
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
        // Last hit is the closest hit
        int hitCount = Physics.RaycastNonAlloc(muzzle.position, -muzzle.forward, hits, MAX_DISTANCE);
        if (hits.Length == 0) {
            return;
        }
        Vector3 destination = hits.First().point;
        for (int j = hitCount - 1; j > 0; j--)
        {
            // TODO: Kill player if hit

            // If the hit target can block the laser, then the laser should stop at the hit point.
            if (hits[j].collider.gameObject.CompareTag("BlockLaser"))
            {
                destination = hits[j].point;
                break;
            }
        }
        Debug.DrawLine(muzzle.position, destination, Color.red, 1f);
        lineBehavior.StartPos = muzzle.localPosition;
        float distance = Vector3.Distance(muzzle.position, destination) / shot.transform.localScale.z;
        lineBehavior.EndPos = new Vector3(
            muzzle.localPosition.x,
            muzzle.localPosition.y,
            muzzle.localPosition.z - distance
        );
        shotCollider.center = new Vector3(
            shotCollider.center.x,
            shotCollider.center.y,
            - distance / 2
        );
        shotCollider.size = new Vector3(
            shotCollider.size.x,
            shotCollider.size.y,
            - distance
        );
    }
}