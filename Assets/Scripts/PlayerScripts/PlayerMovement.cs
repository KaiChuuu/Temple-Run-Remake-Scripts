using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum Action
    {
        REPOSITIONING,
        ATTEMPTING_TURN,
        TURNING,
        MOVING,
        DEAD
    }

    Action currentAction = Action.MOVING;

    //TOGGLES
    public bool repositioningToggle = true;

    Rigidbody playerBody;

    Collider movementBounds;
    Vector3 newPosition;

    GameObject playerCamera;

    [SerializeField]
    float playerPositionSpeed,
          playerForwardSpeed,
          playerRepositionSpeed,
          playerJumpForce;

    //Related to PlayerCamera
    //Do not tamper with values
    float playerRotationSpeed = 0.5f;
    float dynamicPlayerRotationSpeed;
    float cameraRotationFallOffCap = 0.01f;

    int speedAmpAmount = 8;
    float speedAmpTimeDelay = 30f;
    float speedAmpTimeCount = 0.0f;

    GameObject currentConnector;
    float connectorDistance;

    GameObject connectorCameraAngle;

    bool enableTurn = false;

    float timeCount;

    GameObject playerAnimation;
    AnimationType playerCurrentAnimation = AnimationType.RUN;

    Vector3 repositionCenter;

    bool isGrounded = true;
    bool isFalling = false;
    int inAir = 1;
    bool isRolling = false;

    float turnTimeCount = 0.0f;
    float turnTimeDelay = 0.5f; //avg required time 0.345

    float deathDelayTime = 0.0f;
    float deathDelayCount = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimation = GameObject.Find("Player/Player Model");
        playerBody = transform.GetComponent<Rigidbody>();
        playerCamera = GameObject.Find("Player/CM Game Camera");

        dynamicPlayerRotationSpeed = playerRotationSpeed;
    }

    // Update is called once per frame
    void Update() 
    {
        if (currentAction == Action.DEAD)
        {
            if(deathDelayCount <= deathDelayTime)
            {
                transform.Translate(Vector3.forward * Time.deltaTime * playerForwardSpeed);
                UpdateMovement();
                deathDelayCount += Time.deltaTime;
            }
            return;
        }
        else
        {
            if (speedAmpAmount > 0 && speedAmpTimeCount >= speedAmpTimeDelay)
            {
                speedAmpTimeCount = 0f;
                playerForwardSpeed += 1f;
                speedAmpAmount--;
            }
            else if (speedAmpAmount > 0)
            {
                speedAmpTimeCount += Time.deltaTime;
            }
        }

        //Move Player Forward
        if (playerCurrentAnimation != AnimationType.OBSTACLE_DEATH)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * playerForwardSpeed);
        }

        if (isFalling)
        { 
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.2f))
            {
                if (hit.collider.CompareTag("Platform") && inAir == 1)
                {
                    CompleteJumpAnimation();
                }
            }           
        }

        switch (currentAction)
        {
            case Action.MOVING:
                {
                    UpdateMovement();
                    break;
                }
            case Action.ATTEMPTING_TURN:
                {
                    UpdateMovement();
                    CheckRotationInput();
                    break;
                }
            case Action.TURNING:
                {
                    TurnInterpolation();
                    break;
                }
            case Action.REPOSITIONING:
                {
                    CenterInterpolation();
                    break;
                }
        }
    }
   
    void UpdateMovement()
    {
        bool moveUp = false;
        bool moveDown = false;
        bool moveLeft = false;
        bool moveRight = false;
        if (Input.GetKeyDown("w") || Input.GetKeyDown("up"))
        {
            moveUp = true;
        }
        if(Input.GetKeyDown("s") || Input.GetKeyDown("down")) 
        { 
            moveDown = true;
        }
        if(Input.GetKey("a") || Input.GetKey("left"))
        {
            moveLeft = true;
        }
        if(Input.GetKey("d") || Input.GetKey("right"))
        {
            moveRight = true;
        }
        

        if (!movementBounds)
        {
            //If not space is set
            return;
        }

        if (moveUp && isGrounded)
        {
            if (movementBounds.bounds.Contains(transform.position)){
                playerAnimation.GetComponent<PlayerAnimationStateController>().PerformAnimation(true, AnimationType.JUMP);
                isGrounded = false;
                isFalling = false;
                inAir = 1;
            }
        }

        if (moveDown && isGrounded)
        {
            if (movementBounds.bounds.Contains(transform.position))
            {
                playerAnimation.GetComponent<PlayerAnimationStateController>().PerformAnimation(true, AnimationType.ROLL);
                isGrounded = false;
                isRolling = true;
            }
        }

        if (moveDown && isFalling)
        {
            if (movementBounds.bounds.Contains(transform.position))
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, Vector3.down, out hit, 4f))
                {
                    if (hit.collider.CompareTag("Platform"))
                    {
                        isFalling = false;
                        playerAnimation.GetComponent<PlayerAnimationStateController>().PerformAnimation(false, AnimationType.ROLLTOJUMP);
                        playerAnimation.GetComponent<PlayerAnimationStateController>().PerformAnimation(false, AnimationType.JUMP);
                        playerAnimation.GetComponent<PlayerAnimationStateController>().PerformAnimation(true, AnimationType.ROLL);
                        playerAnimation.GetComponent<PlayerAnimationStateController>().PerformAnimation(true, AnimationType.JUMPTOROLL);
                        isGrounded = false;
                        isRolling = true;
                        playerBody.AddForce(new Vector3(0, -1, 0) * playerJumpForce, ForceMode.Impulse);
                    }
                }
            }
        }

        if (moveUp && isRolling)
        {
            if (movementBounds.bounds.Contains(transform.position))
            {
                RaycastHit hit;
                if (!Physics.Raycast(transform.position + new Vector3(0, 1, 0), Vector3.up, out hit, 4f))
                {
                    playerAnimation.GetComponent<PlayerAnimationStateController>().PerformAnimation(false, AnimationType.JUMPTOROLL);
                    playerAnimation.GetComponent<PlayerAnimationStateController>().PerformAnimation(true, AnimationType.JUMP);
                    playerAnimation.GetComponent<PlayerAnimationStateController>().PerformAnimation(true, AnimationType.ROLLTOJUMP);
                    playerAnimation.GetComponent<PlayerAnimationStateController>().PerformAnimation(false, AnimationType.ROLL);
                    isGrounded = false;
                    isRolling = false;
                    isFalling = false;
                    inAir = 1;
                }
            }
        }

        if (moveLeft)
        {
            newPosition = transform.position + transform.TransformDirection(Vector3.left) * playerPositionSpeed * Time.deltaTime;
            CanMove(newPosition, Vector3.left * playerPositionSpeed * Time.deltaTime);
        }
        else if (moveRight)
        {
            newPosition = transform.position + transform.TransformDirection(Vector3.right) * playerPositionSpeed * Time.deltaTime;
            CanMove(newPosition, Vector3.right * playerPositionSpeed * Time.deltaTime);  
        }
    }

    void CanMove(Vector3 desiredPos, Vector3 direction)
    {
        if (movementBounds.bounds.Contains(desiredPos))
        {
            transform.Translate(direction);
        }
    }
    
    void CenterInterpolation()
    {
        timeCount += playerRepositionSpeed * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, repositionCenter, timeCount);

        if (Vector3.Distance(transform.position, repositionCenter) <= 0.5f)
        {
            timeCount = 0.0f;
            currentAction = Action.MOVING;
        }
    }
    

    void TurnInterpolation()
    {
        if (!enableTurn)
        { 
            return;
        }

        timeCount += dynamicPlayerRotationSpeed * Time.deltaTime;

        if (dynamicPlayerRotationSpeed > cameraRotationFallOffCap)
        {
            dynamicPlayerRotationSpeed /= 1.05f;
        }
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, currentConnector.transform.forward, timeCount, 0.001f);
        transform.rotation = Quaternion.LookRotation(newDirection);

        turnTimeCount += Time.deltaTime;

        if(turnTimeCount > turnTimeDelay)
        {
            turnTimeCount = 0f;
            dynamicPlayerRotationSpeed = playerRotationSpeed;
            timeCount = 0.0f;
            currentAction = Action.MOVING;
            enableTurn = false;
        }
    }

    float getConnectorRange(GameObject path)
    {
        Mesh planeMesh = path.GetComponentInChildren<MeshFilter>().mesh;
        Bounds planeBounds = planeMesh.bounds;

        Vector3 planeLength = new Vector3(0, 0, planeBounds.size.x * path.transform.parent.localScale.x / 2);
        Vector3 connectorRange = path.transform.position - planeLength;

        return Vector3.Distance(path.transform.position, connectorRange);
    }

    public void CheckRotationInput()
    {
        bool moveLeft = false;
        bool moveRight = false;
        if (Input.GetKeyDown("a") || Input.GetKeyDown("left"))
        {
            moveLeft = true;
        }
        if(Input.GetKeyDown("d") || Input.GetKeyDown("right"))
        {
            moveRight = true;
        }

        Quaternion expectedRotation;
        if (moveRight && !moveLeft)
        {
            expectedRotation = transform.rotation * Quaternion.AngleAxis(90, Vector3.up);  
            if (Mathf.Abs(Quaternion.Dot(currentConnector.transform.rotation, expectedRotation)) > 0.9f)
            {
                currentAction = Action.TURNING;
                playerCamera.GetComponent<PlayerCamera>().UpdateCameraRotation(connectorCameraAngle, Vector3.right);
            }
        }
        else if (moveLeft && !moveRight)
        {
            expectedRotation = transform.rotation * Quaternion.AngleAxis(90, Vector3.down);
            if (Mathf.Abs(Quaternion.Dot(currentConnector.transform.rotation, expectedRotation)) > 0.9f)
            {
                currentAction = Action.TURNING;
                playerCamera.GetComponent<PlayerCamera>().UpdateCameraRotation(connectorCameraAngle, Vector3.left);
            }
        }
    }

    public void PlayerEnterConnector(GameObject connector, GameObject cameraAngle)
    {
        connectorCameraAngle = cameraAngle;
        currentConnector = connector;
        connectorDistance = getConnectorRange(currentConnector);
        currentAction = Action.ATTEMPTING_TURN;
    }

    public void PlayerEnterPath(GameObject path)
    {
        if (!repositioningToggle)
        {
            float boxEndZ = path.GetComponentInChildren<BoxCollider>().center.z + path.GetComponentInChildren<BoxCollider>().size.z / 2;
            repositionCenter = path.transform.TransformPoint(new Vector3(0, 0, boxEndZ));

            currentAction = Action.REPOSITIONING;
        }
    }

    public void PlayerEnterTurn()
    {
        enableTurn = true;
    }

    public void PlayerNewPathBounds(Collider currentPathBounds)
    {
        movementBounds = currentPathBounds;
    }

    public void PlayerDead(AnimationType deathType, float delayDeath)
    {
        if(currentAction == Action.DEAD)
        {
            return;
        }

        currentAction = Action.DEAD;
        playerCurrentAnimation = deathType;
        deathDelayTime = delayDeath;
        deathDelayCount = 0.0f;
        playerCamera.GetComponent<PlayerCamera>().PlayerIsDead();
        playerAnimation.GetComponent<PlayerAnimationStateController>().PerformAnimation(true, deathType);
    }

    public void ApplyJumpForce()
    {
        playerBody.AddForce(new Vector3(0, 1, 0) * playerJumpForce, ForceMode.Impulse);
    }

    public void CompletedRollAnimation()
    {
        playerAnimation.GetComponent<PlayerAnimationStateController>().PerformAnimation(false, AnimationType.JUMPTOROLL);
        playerAnimation.GetComponent<PlayerAnimationStateController>().PerformAnimation(false, AnimationType.ROLL);
        isRolling = false;
    }

    public void CompleteJumpAnimation()
    {
        isFalling = false;
        inAir = 0;
        playerAnimation.GetComponent<PlayerAnimationStateController>().PerformAnimation(false, AnimationType.ROLLTOJUMP);
        playerAnimation.GetComponent<PlayerAnimationStateController>().PerformAnimation(false, AnimationType.JUMP);
    }

    public void IsGrounded()
    {
        isGrounded = true;
    }

    public void IsFalling()
    {
        isFalling = true;
    }

    public void IncreaseSpeed()
    {
        playerForwardSpeed++;
    }

    public float GetPlayerSpeed()
    {
        return playerForwardSpeed;
    }
}
