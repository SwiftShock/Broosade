using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject player;
    public float offset;
    private Vector3 playerPosition;
    public float offsetSmoothing;
    private Vector3 prevCamPosition;
    private Vector3 newCamPosition;

    void Update () {
        playerPosition = new Vector3(player.transform.position.x, player.transform.position.y + 2.0f, transform.position.z);
        positionXUpdate();
        transform.position = Vector3.Lerp(transform.position, playerPosition, offsetSmoothing * Time.deltaTime);
        prevCamPosition = transform.position;
        ShootCam();
    }

    void positionXUpdate ()
    {
        if (player.transform.localScale.x > 0f)
        {
            offset = 2.0f;
        }
        else
        {
            offset = -2.0f;
        }
        playerPosition = new Vector3(playerPosition.x + offset, playerPosition.y, playerPosition.z);
    }

    void ShootCam()
    {
        if (Input.GetKey(KeyCode.H))
        {
            if (Input.GetKey(KeyCode.U))
            {
                newCamPosition = new Vector3(transform.position.x + offset, transform.position.y + 1.0f, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, newCamPosition, offsetSmoothing * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.N))
            {
                newCamPosition = new Vector3(transform.position.x + offset, transform.position.y - 1.0f, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, newCamPosition, 3 * Time.deltaTime);
            }
            else
            {
                newCamPosition = new Vector3(transform.position.x + offset, transform.position.y, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, newCamPosition, offsetSmoothing * Time.deltaTime);
            }
        }
        else if (Input.GetKeyUp(KeyCode.H))
        {
            transform.position = Vector3.Lerp(transform.position, prevCamPosition, offsetSmoothing * Time.deltaTime);
        }
    }
}
