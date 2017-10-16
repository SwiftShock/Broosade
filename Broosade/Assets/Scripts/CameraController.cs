using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform player;
    private Vector3 moveTemp;
    public float speed = 3;
    public float xDifference;
    public float yDifference;
    public float playerDistance = 3;

	void Update () {

        if (player.transform.position.x > transform.position.x)
        {
            xDifference = player.transform.position.x - transform.position.x;
        }
        else
        {
            xDifference = transform.position.x - player.transform.position.x;
        }

        if (player.transform.position.y > transform.position.y)
        {
            yDifference = player.transform.position.y - transform.position.y;
        }
        else
        {
            yDifference = transform.position.y - player.transform.position.y;
        }

        if (xDifference >= playerDistance || yDifference >= playerDistance)
        {
            moveTemp = player.transform.position;
            moveTemp.z = -10;

            transform.position = Vector3.MoveTowards(transform.position, moveTemp, speed * Time.deltaTime);
        }
        
	}
}
