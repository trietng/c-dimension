using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaCannonScript : MonoBehaviour
{
    public Transform muzzle;

    public GameObject shotPrefab;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
		{
			GameObject go = Instantiate(shotPrefab, muzzle.position, muzzle.rotation);
			Destroy(go, 30f);
		}
    }
}
