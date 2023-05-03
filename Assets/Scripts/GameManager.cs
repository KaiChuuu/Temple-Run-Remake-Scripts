using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class GameManager : MonoBehaviour
{
    public enum PlayerState
    {
        ALIVE,
        DEAD
    }

    PlayerState currentState = PlayerState.ALIVE;

    [SerializeField]
    private CinemachineVirtualCamera IntroCamera; // 11 -> 9
    [SerializeField]
    private CinemachineVirtualCamera GameCamera; // 10

    public GameObject gameCanvas;
    public GameObject gameCanvasEffects;

    public GameObject player;

    //Current game statistics
    float scoreTimer;
    int playerTotalScore;
    int playerTotalGemCount;
    int playerGemCount;

    float distanceTime;
    int playerTotalDistance;

    void Start()
    {
        if(WeatherHandler.Instance.gameWeather == WeatherType.SECRET)
        {
            AudioManager.Instance.PlayMusic("Easter Egg", 0.3f);
        }
        else
        {
            AudioManager.Instance.PlayMusic("Theme", 0.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case PlayerState.ALIVE:
                {
                    PlayerAliveUpdates();
                    break;
                }
            case PlayerState.DEAD:
                {
                    break;
                }
        }
    }
    
    void PlayerAliveUpdates()
    {
        float playerCurSpeed = player.GetComponent<PlayerMovement>().GetPlayerSpeed();

        scoreTimer += Time.deltaTime;

        if (scoreTimer > 0.05f)
        {
            if(playerCurSpeed == 16f)
            {
                playerTotalScore += 3;
            }
            else if(playerCurSpeed >= 13f)
            {
                playerTotalScore += 2;

            }
            else
            {
                playerTotalScore++;
            }

            gameCanvas.GetComponent<GameUIScript>().UpdateScoreCounter(playerTotalScore);
            
            scoreTimer = 0f;
        }

        distanceTime += Time.deltaTime * playerCurSpeed * 0.5f;
        if(distanceTime >= 100f)
        {
            distanceTime = 0f;
            playerTotalDistance += 500;
            gameCanvasEffects.GetComponent<GameUIEffects>().DisplayDistance(playerTotalDistance);
        }
    }

    public void SwitchToGameCamera()
    {
        IntroCamera.Priority = 9; 
    }

    public void UpdateGemCounter()
    {
        playerTotalGemCount++;
        playerGemCount++;

        gameCanvas.GetComponent<GameUIScript>().UpdateGemCounter(playerTotalGemCount);
        gameCanvas.GetComponent<GameUIScript>().IncreaseGemBar(playerGemCount);

        if (playerGemCount == 100)
        {
            AudioManager.Instance.PlaySFX2("Full Meter", 1f, 0.5f);
            gameCanvas.GetComponent<GameUIScript>().ResetGemBar();
            gameCanvasEffects.GetComponent<GameUIEffects>().PlayFullMeterAnimation();
            playerGemCount = 0;
        }
    }

    public void PlayerIsDead()
    {
        AudioManager.Instance.musicSource.Stop();
        currentState = PlayerState.DEAD;
        gameCanvas.GetComponent<GameUIScript>().GameOverUI(playerTotalScore, playerTotalGemCount);
    }

    public void FullMeterBonusGems()
    {
        playerTotalScore += 100;
    }
}
