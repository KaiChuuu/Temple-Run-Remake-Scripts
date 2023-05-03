using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCollider : MonoBehaviour
{
    GameObject gameManager;
    GameObject playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        playerMovement = GameObject.Find("Player/Player Model");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            gameManager.GetComponent<GameManager>().PlayerIsDead();
            playerMovement.GetComponent<PlayerMovement>().PlayerDead(AnimationType.OBSTACLE_DEATH, 0.0f);
        }
        else if (other.CompareTag("Edge Obstacle"))
        {
            gameManager.GetComponent<GameManager>().PlayerIsDead();
            playerMovement.GetComponent<PlayerMovement>().PlayerDead(AnimationType.FALL_DEATH, 2.0f);
        }
    }
}
