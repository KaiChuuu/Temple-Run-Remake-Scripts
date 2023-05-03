using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathMovementBounds : MonoBehaviour
{
    Collider pathCollider;
    GameObject playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        pathCollider = transform.GetComponent<BoxCollider>();
        playerMovement = GameObject.Find("Player/Player Model");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerMovement.GetComponent<PlayerMovement>().PlayerNewPathBounds(pathCollider);
        }
    }
}
