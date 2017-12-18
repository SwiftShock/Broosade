using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScripting : MonoBehaviour {

    public GameObject destruction;
    public GameObject ultraBoulder;
    private Animator targetActivation;
    private bool targetActivated;

	// Use this for initialization
	void Start () {
        targetActivation = GetComponent<Animator>();
        targetActivated = false;
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(this.tag == "Target" && other.tag == "Bullet" && !targetActivated)
        {
            targetActivation.SetInteger("Activate", 1);
            targetActivated = true;
            Instantiate(destruction, ultraBoulder.transform.position, ultraBoulder.transform.rotation);
            Destroy(ultraBoulder);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
