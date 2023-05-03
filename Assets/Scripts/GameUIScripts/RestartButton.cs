using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RestartButton : MonoBehaviour
{
    public GameObject UIAnimations;

    private void OnMouseDown()
    {
        UIAnimations.GetComponent<GameUIScript>().RestartGameAnimation();
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
