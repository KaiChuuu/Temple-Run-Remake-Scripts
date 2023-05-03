using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class GameUIScript : MonoBehaviour
{
    Animator animator;
    GameObject sceneHandler;

    //In game
    public GameObject totalScore;
    TextMeshProUGUI totalScoreText;

    public GameObject gemScore;
    TextMeshProUGUI gemScoreText;
    
    public GameObject gemBar;
    Image gemBarProgress;

    //End game
    public GameObject scoreResult;
    TextMeshProUGUI scoreResultText;

    public GameObject gemResult;
    TextMeshProUGUI gemResultText;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        sceneHandler = GameObject.Find("SceneHandler");

        totalScoreText = totalScore.GetComponent<TextMeshProUGUI>();
        gemScoreText = gemScore.GetComponent<TextMeshProUGUI>();
        gemBarProgress = gemBar.GetComponent<Image>();

        scoreResultText = scoreResult.GetComponent<TextMeshProUGUI>();
        gemResultText = gemResult.GetComponent<TextMeshProUGUI>();

        StartGameUI();
    }

    public void RestartGameAnimation()
    {
        WeatherHandler.Instance.RandomizeWeather();
        animator.SetTrigger("Show Gameover UI");
    }

    public void ReturnToMenu()
    {
        sceneHandler.GetComponent<SceneHandler>().SwitchScene(SceneState.MENU);
    }

    public void StartGameUI()
    {
        animator.SetTrigger("Show Game UI");
    }

    public void GameOverUI(int scoreResult, int gemResult)
    {
        scoreResultText.text = scoreResult.ToString();
        gemResultText.text = gemResult.ToString();

        animator.SetTrigger("Show Game UI");
        animator.SetTrigger("Show Gameover UI");
    }

    public void UpdateScoreCounter(int updatedScore)
    {
        totalScoreText.text = updatedScore.ToString();
    }

    public void UpdateGemCounter(int updatedGemScore)
    {
        gemScoreText.text = updatedGemScore.ToString();
    }

    public void IncreaseGemBar(int updatedGemScore)
    {
        if(updatedGemScore >= 0 || updatedGemScore <= 100)
        {
            gemBarProgress.fillAmount = updatedGemScore / 100f;
        }
    }

    public void ResetGemBar()
    {
        gemBarProgress.fillAmount = 0;
    }
}
