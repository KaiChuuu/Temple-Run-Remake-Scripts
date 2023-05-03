using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Connector Setting")]
public class ConnectorSettings : ScriptableObject
{
    public GameObject connectorPrefab;

    public CornerType cornerType;

    public enum CornerType
    {
        Left,
        Right
    }
}

