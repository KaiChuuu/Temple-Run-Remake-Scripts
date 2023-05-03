using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    Animator animator;

    int jumpingHash;
    int rollingHash;

    int jumpToRollHash;
    int rollToJumpHash;

    int obstacleDeathHash;
    int fallDeathHash;

    int restartGameHash;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        jumpingHash = Animator.StringToHash("isJumping");
        rollingHash = Animator.StringToHash("isRolling");

        obstacleDeathHash = Animator.StringToHash("obstacleDeath");
        fallDeathHash = Animator.StringToHash("fallDeath");

        restartGameHash = Animator.StringToHash("restartGame");

        jumpToRollHash = Animator.StringToHash("isJumpToRoll");
        rollToJumpHash = Animator.StringToHash("isRollToJump");
    }

    public void UpdatePlayerAnimation(bool playerInput, int animationHash)
    {
        if (playerInput)
        {
            animator.SetBool(animationHash, true);
        }

        if (!playerInput)
        {
            animator.SetBool(animationHash, false);
        }
    }

    public void ReleaseAllAnimations()
    {
        UpdatePlayerAnimation(false, jumpingHash);
        UpdatePlayerAnimation(false, rollingHash);

        UpdatePlayerAnimation(false, jumpToRollHash);
        UpdatePlayerAnimation(false, rollToJumpHash);
    }

    public void FreezeEnemyMovement()
    {
        transform.SetParent(null, true);
    }
}
