using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class TargetDisintegrationScript : MonoBehaviour {
    private Rigidbody[] fragments;
    private Rigidbody exploder;

    public float explosionForce = 250f;
    public float explosionRadius = 1f;
    public float timeToDestroy = 5f;
    public bool destructible = true;

    void Start() {
        fragments = GetComponentsInChildren<Rigidbody>();
        exploder = fragments.Where(fragment => fragment.gameObject.CompareTag("TargetExploder")).First();
    }

    public IEnumerator Disintegrate() {
        GetComponent<BoxCollider>().enabled = false;
        foreach (Rigidbody fragment in fragments) {
            fragment.constraints = RigidbodyConstraints.None;
            yield return null;
        }
        exploder.AddExplosionForce(explosionForce, transform.position - new Vector3(0, 0.5f, 0), explosionRadius);
        if (destructible) {
            Destroy(gameObject, timeToDestroy);
        }
    }
}