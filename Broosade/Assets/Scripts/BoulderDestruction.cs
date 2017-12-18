using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderDestruction : MonoBehaviour {

    public GameObject destruction;

    // Use this for initialization
    void Start () {
	}
	
	void OnTriggerEnter2D (Collider2D other)
    {
		if(this.tag == "Boulder" && other.tag == "Dash Attack" || other.tag == "Melee Attack" || other.tag == "Aerial Attack")
        {
            Vamoose(); 
        }
        else if (this.tag == "Boulder-S" && other.tag=="Melee Attack")
        {
            Vamoose();
        }
	}

    void Vamoose()
    {
        Instantiate(destruction, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
