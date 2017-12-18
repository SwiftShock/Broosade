using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour {

    private ParticleSystem theseParts;

	// Use this for initialization
	void Start () {
        theseParts = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        Destroy(gameObject, theseParts.main.duration);
	}
}
