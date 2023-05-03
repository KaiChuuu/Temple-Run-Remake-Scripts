using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public enum AnimationType
{
    JUMP,
    ROLL,
    OBSTACLE_DEATH,
    FALL_DEATH,
    RESTART,
    JUMPTOROLL,
    ROLLTOJUMP,
    RUN
}

[RequireComponent(typeof(Animator))]
public class PlayerAnimationStateController : MonoBehaviour
{
    Animator animator;

    int jumpingHash;
    int rollingHash;

    int jumpToRollHash;
    int rollToJumpHash;

    int obstacleDeathHash;
    int fallDeathHash;

    int restartGameHash;

    public GameObject enemyModel;

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

    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Running") && !AudioManager.Instance.sfxPlayerSource.isPlaying)
        {
            AudioManager.Instance.PlayPlayerSFX("Running", 1f, 0.1f);
        }
    }

    void UpdatePlayerAnimation(bool playerInput, int animationHash)
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

    public void PerformAnimation(bool state, AnimationType type)
    {
        switch (type)
        {
            case AnimationType.JUMP:
                UpdatePlayerAnimation(state, jumpingHash);
                if(state)
                {
                    AudioManager.Instance.sfxPlayerSource.Stop();
                    AudioManager.Instance.PlayPlayerSFX("Jump Intro", 1f, 0.4f);
                }else if (!state)
                {
                    if(AudioManager.Instance.currPlayerSound.name != "Jump End")
                    {
                        AudioManager.Instance.sfxPlayerSource.Stop();
                        AudioManager.Instance.PlayPlayerSFX("Jump End", 1f, 0.4f);
                    }
                }
                enemyModel.GetComponent<EnemyAnimator>().UpdatePlayerAnimation(state, jumpingHash);
                break;
            case AnimationType.ROLL:
                if (state)
                {
                    AudioManager.Instance.sfxPlayerSource.Stop();
                    AudioManager.Instance.PlayPlayerSFX("Sliding", 1f, 0.5f);
                }
                UpdatePlayerAnimation(state, rollingHash);
                break;
            case AnimationType.OBSTACLE_DEATH:
                if (state)
                {
                    AudioManager.Instance.sfxPlayerSource.Stop();
                    AudioManager.Instance.PlayPlayerSFX("Crash", 1f, 0.4f);
                }
                enemyModel.GetComponent<EnemyAnimator>().FreezeEnemyMovement();
                UpdatePlayerAnimation(true, obstacleDeathHash);
                ReleaseAllAnimations();
                break;
            case AnimationType.FALL_DEATH:
                if (state)
                {
                    AudioManager.Instance.sfxPlayerSource.Stop();
                    AudioManager.Instance.PlayPlayerSFX("Falling", 1f, 0.4f);
                }
                enemyModel.GetComponent<EnemyAnimator>().FreezeEnemyMovement();
                UpdatePlayerAnimation(true, fallDeathHash);
                ReleaseAllAnimations();
                break;
            case AnimationType.RESTART:
                UpdatePlayerAnimation(true, restartGameHash);
                UpdatePlayerAnimation(false, obstacleDeathHash);
                UpdatePlayerAnimation(false, fallDeathHash);
                break;
            case AnimationType.JUMPTOROLL:
                UpdatePlayerAnimation(state, jumpToRollHash);
                break;
            case AnimationType.ROLLTOJUMP:
                UpdatePlayerAnimation(state, rollToJumpHash);
                break;
        }
    }

    void ReleaseAllAnimations()
    {
        UpdatePlayerAnimation(false, jumpingHash);
        UpdatePlayerAnimation(false, rollingHash);

        UpdatePlayerAnimation(false, jumpToRollHash);
        UpdatePlayerAnimation(false, rollToJumpHash);
    }
}
