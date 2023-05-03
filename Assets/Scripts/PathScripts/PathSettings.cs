using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Path Setting")]
public class PathSettings : ScriptableObject
{
    public GameObject pathPrefab;

    public List<GameObject> coinGenerations;
}
