using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneState
{
    INGAME,
    MENU
}

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler Instance;

    SceneState currentState = SceneState.MENU;

    void Awake()
    {
        if(Instance != null & Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void SwitchScene(SceneState nextState)
    {
        currentState = nextState;

        switch (nextState)
        {
            case SceneState.INGAME:
                SceneManager.LoadScene(1);
                break;
            case SceneState.MENU:
                SceneManager.LoadScene(0);
                break;
        }

    }
}
