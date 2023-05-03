using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    GameObject gameManager;
    GameObject playerMovement;

    float pitchValue = 1;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        playerMovement = GameObject.Find("Player/Player Model");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gem"))
        {
            AudioManager.Instance.PlaySFX("Gem", pitchValue, 1f);
            gameManager.GetComponent<GameManager>().UpdateGemCounter();
            other.gameObject.SetActive(false);

            pitchValue += 0.1f;
            if(pitchValue > 2.4f)
            {
                pitchValue = 1f;
            }
        } 
        else if (other.CompareTag("Obstacle"))
        {
            gameManager.GetComponent<GameManager>().PlayerIsDead();
            playerMovement.GetComponent<PlayerMovement>().PlayerDead(AnimationType.OBSTACLE_DEATH, 0.0f);
        } 
        else if(other.CompareTag("Edge Obstacle"))
        {
            gameManager.GetComponent<GameManager>().PlayerIsDead();
            playerMovement.GetComponent<PlayerMovement>().PlayerDead(AnimationType.FALL_DEATH, 2.0f);
        }
    }
}
