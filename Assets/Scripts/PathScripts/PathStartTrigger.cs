using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathStartTrigger : MonoBehaviour
{
    GameObject pathGenerator;
    GameObject playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        pathGenerator = GameObject.Find("Path Generator");
        playerMovement = GameObject.Find("Player/Player Model");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerMovement.GetComponent<PlayerMovement>().PlayerEnterPath(transform.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pathGenerator.GetComponent<PathGenerator>().GenerateNextConnectorAndPath();
        }
    }
}
