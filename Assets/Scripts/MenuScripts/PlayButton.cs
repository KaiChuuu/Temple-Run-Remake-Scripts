using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayButton : MonoBehaviour
{
    public GameObject UIAnimations;

    private void OnMouseDown()
    {
        UIAnimations.GetComponent<MenuUI>().MenuAnimation();
    }

    private void OnMouseEnter()
    {
        transform.localScale += new Vector3(-1f, -1f, -1f); 
    }

    private void OnMouseExit()
    {
        transform.localScale += new Vector3(1f, 1f, 1f);
    }
}
