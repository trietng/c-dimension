using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class TargetDisintegrationScript : MonoBehaviour {
    private Rigidbody[] fragments;
    private Rigidbody[] exploders;

    public float explosionForce = 250f;
    public float explosionRadius = 0.5f;
    public float timeToDestroy = 30f;
    public bool destructible = true;

    void Start() {
        fragments = GetComponentsInChildren<Rigidbody>();
        exploders = fragments.Where(fragment => fragment.gameObject.CompareTag("TargetExploder")).ToArray();
    }

    public IEnumerator Disintegrate() {
        GetComponent<BoxCollider>().enabled = false;
        // This loop cannot be parallelized
        foreach (Rigidbody fragment in fragments) {
            fragment.constraints = RigidbodyConstraints.None;
        }
        foreach (Rigidbody exploder in exploders) {
            exploder.AddExplosionForce(explosionForce, exploder.transform.position, explosionRadius);
        }
        foreach (Rigidbody fragment in fragments) {
            fragment.useGravity = true;
        }
        if (destructible) {
            Destroy(gameObject, timeToDestroy);
        }
        yield return null;
    }
}