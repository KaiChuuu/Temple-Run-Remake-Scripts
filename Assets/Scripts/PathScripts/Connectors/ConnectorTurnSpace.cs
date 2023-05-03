using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectorTurnSpace : MonoBehaviour
{
    GameObject playerMovement;
    GameObject playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GameObject.Find("Player/Player Model");
        playerCamera = GameObject.Find("Player/CM Game Camera");
    }

    private void OnTriggerEnter(Collider other)
    {
        playerMovement.GetComponent<PlayerMovement>().PlayerEnterTurn();
        playerCamera.GetComponent<PlayerCamera>().PlayerEnterTurn();      
    }
}
