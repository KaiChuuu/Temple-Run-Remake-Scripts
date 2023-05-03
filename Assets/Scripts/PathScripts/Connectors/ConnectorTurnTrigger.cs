using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectorTurnTrigger : MonoBehaviour
{
    GameObject playerMovement;
    GameObject cameraAngle;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GameObject.Find("Player/Player Model");
        cameraAngle = transform.Find("CameraAngle").gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerMovement.GetComponent<PlayerMovement>().PlayerEnterConnector(transform.gameObject, cameraAngle);
        }
    }
}
