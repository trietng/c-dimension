using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LaserCannonScript : MonoBehaviour
{
    public static float MAX_DISTANCE = 1000f;
    public Transform muzzle;
    public GameObject shotPrefab;
    private VolumetricLines.VolumetricLineStripBehavior lineStrip;

    void Start()
    {
        lineStrip = shotPrefab.GetComponent<VolumetricLines.VolumetricLineStripBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
		{
			GameObject go = Instantiate(shotPrefab, muzzle.position, muzzle.rotation);
			Destroy(go, 30f);
		}
    }

    void Fire(Vector3 origin, Vector3 dir, int reflectCount)
    {
        var lineVertices = lineStrip.LineVertices;
        // lineRenderer.positionCount = reflectCount + 1;
        // Set the start position of the laser
        for (int i = 0; i < reflectCount; ++i) {
            // Ignore the Confiner object
            RaycastHit[] hits = new RaycastHit[100];
            Physics.RaycastNonAlloc(origin, dir, hits, MAX_DISTANCE);
            if (hits.Length == 0) {
                continue;
            }
            RaycastHit destination = hits.Last();
            for (int j = 0; j < hits.Length - 1; j++)
            {
                // Do something here
            }
            // lineRenderer.SetPosition(i + 1, destination.point);
            // Instantiate a light spot at the destination point if it doesn't already exist
            // TODO: Implement this
            origin = new Vector3(destination.point.x, destination.point.y, destination.point.z);
            dir = Vector3.Reflect(dir, destination.normal);
        }
    }
}