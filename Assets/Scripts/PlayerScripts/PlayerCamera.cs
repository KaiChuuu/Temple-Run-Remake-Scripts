using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PlayerCamera : MonoBehaviour
{
    GameObject playerModel;

    public Material easterEggSkybox;
    public Material nightSkybox;

    [SerializeField]
    Vector3 offset; 

    [SerializeField]
    Quaternion rotationOffset;

    public bool performingTurn = false;
    public bool followingPlayer = true;

    public GameObject currentConnector;
    public Vector3 rotationDirection;

    //Related to PlayerMovement
    //Do not tamper with values
    float timeCount;
    float cameraRotationSpeed = 30f; //Initial speed at which the camera rotates around the player.
    float dynamicRotationSpeed;
    float cameraRotationFallOffCap = 0.01f; //Cap to how low the rotational speed can get.

    bool enableTurn = false;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            //attach playerModel
            playerModel = GameObject.Find("Player/Player Model");
        }
        catch (Exception e)
        {
            Debug.Log("Can't find Player Model: \n" + e);
        }
        dynamicRotationSpeed = cameraRotationSpeed;
        transform.rotation = rotationOffset;
        transform.position = offset + playerModel.transform.position;

        if (WeatherHandler.Instance.gameWeather == WeatherType.SECRET)
        {
            RenderSettings.skybox = easterEggSkybox;
        }
        else if (WeatherHandler.Instance.gameWeather == WeatherType.NIGHT)
        {
            RenderSettings.skybox = nightSkybox;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (performingTurn)
        {
            RotateCamera();
        }
        if (followingPlayer)
        {
            UpdatePosition();
        }   
    }

    public void UpdateCameraRotation(GameObject connector, Vector3 rotationDir)
    {
        currentConnector = connector;
        rotationDirection = rotationDir;

        followingPlayer = false;
        performingTurn = true;
    }

    void RotateCamera()
    {
        UpdatePosition();
        
        if (!enableTurn)
        {
            return;
        }
       

        timeCount += Time.deltaTime * dynamicRotationSpeed;

        if (dynamicRotationSpeed > cameraRotationFallOffCap)
        {
            dynamicRotationSpeed /= 1.05f;
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, currentConnector.transform.rotation, timeCount);

        if (Quaternion.Angle(transform.rotation, currentConnector.transform.rotation) <= 0.1f)
        {
            dynamicRotationSpeed = cameraRotationSpeed;
            timeCount = 0.0f;
            performingTurn = false;
            followingPlayer = true;
            enableTurn = false;
        }
    }

    void UpdatePosition()
    {
        Vector3 updatedPosition = new Vector3(playerModel.transform.position.x, 0, playerModel.transform.position.z);
        transform.position = playerModel.transform.TransformVector(offset) + updatedPosition;
    }

    public void PlayerEnterTurn()
    {
        enableTurn = true;
    }

    public void PlayerIsDead()
    {
        timeCount = 0.0f;
        performingTurn = false;
        followingPlayer = false;
        enableTurn = false;
    }
}
