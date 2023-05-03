using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    Animator animator;
    GameObject sceneHandler;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        sceneHandler = GameObject.Find("SceneHandler");
        MenuAnimation();
    }

    public void MenuAnimation()
    {
        animator.SetTrigger("Show UI");
    }

    public void PlayGame()
    {
        sceneHandler.GetComponent<SceneHandler>().SwitchScene(SceneState.INGAME);
    }
}
