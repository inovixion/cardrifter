using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class AIVehicle : MonoBehaviour
{

    public float carRestTime = 5.0f;

    public float forwardSpeed = 1.0f;
    public float steerSpeed = 1.0f;
    public float nextNodeDistance = 5;

    public Transform frontPoint;

    [HideInInspector]
    public Transform currentNode, nextNode, wrongNode;
    [HideInInspector]
    public int currentLap = 0;

    [HideInInspector]
    public float playerCurrentTime, playerBestTime, playerLastTime = 0.0f;
    [HideInInspector]
    public float AIAccel, AISteer = 0.0f;
    [HideInInspector]
    public bool AIBrake = false;

    private VehicleControl vehicleControl;

    private bool goNextNode = true;
    private bool getLap = false;

    private int carPreviousNodes = 0;
    private float targetAngle;
    private float restTimeer = 0.0f;
    int y;

    void Start()
    {
        y = SceneManager.GetActiveScene().buildIndex;
        restTimeer = carRestTime;
        currentLap = 0;
        vehicleControl = transform.GetComponent<VehicleControl>();
        currentNode = nextNode;
    }

    void Update()
    {
        if (GameUI.manage.gameStarted)
            playerCurrentTime += Time.deltaTime;

        AICarControl();
    }


    //AIWayPoints//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    void AICarControl()
    {


        if (AIControl.manage.firstAINode == wrongNode && !getLap)
        {
            currentLap++;
            playerLastTime = playerCurrentTime;

            if (y == 1)
            {
                if (currentLap == 1 && GameUI.manage.totalDriftCoins > PlayerPrefs.GetInt("track1"))
                {
                    GameUI.manage.gameFinished = true;
                    PlayerPrefs.SetInt("track1", PlayerPrefs.GetInt("track1") + 50);
                }
                else if (currentLap == 1 && GameUI.manage.totalDriftCoins < PlayerPrefs.GetInt("track1"))
                {
                    GameUI.manage.gameFailed = true;
                }
            }
            else if (y == 2)
            {
                if (currentLap == 1 && GameUI.manage.totalDriftCoins > PlayerPrefs.GetInt("track2"))
                {
                    GameUI.manage.gameFinished = true;
                }
                else if (currentLap == 1 && GameUI.manage.totalDriftCoins < PlayerPrefs.GetInt("track2"))
                {
                    GameUI.manage.gameFailed = true;
                }
            }
            else if (y == 3)
            {
                if (currentLap == 1 && GameUI.manage.totalDriftCoins > PlayerPrefs.GetInt("track3"))
                {
                    GameUI.manage.gameFinished = true;
                }
                else if (currentLap == 1 && GameUI.manage.totalDriftCoins < PlayerPrefs.GetInt("track3"))
                {
                    GameUI.manage.gameFailed = true;
                }
            }
            else if (y == 4)
            {
                if (currentLap == 1 && GameUI.manage.totalDriftCoins > PlayerPrefs.GetInt("track4"))
                {
                    GameUI.manage.gameFinished = true;
                }
                else if (currentLap == 1 && GameUI.manage.totalDriftCoins < PlayerPrefs.GetInt("track4"))
                {
                    GameUI.manage.gameFailed = true;
                }
            }
            else if (y == 5)
            {
                if (currentLap == 1 && GameUI.manage.totalDriftCoins > PlayerPrefs.GetInt("track5"))
                {
                    GameUI.manage.gameFinished = true;
                }
                else if (currentLap == 1 && GameUI.manage.totalDriftCoins < PlayerPrefs.GetInt("track5"))
                {
                    GameUI.manage.gameFailed = true;
                }
            }
            else if (y == 6)
            {
                if (currentLap == 1 && GameUI.manage.totalDriftCoins > PlayerPrefs.GetInt("track6"))
                {
                    GameUI.manage.gameFinished = true;
                }
                else if (currentLap == 1 && GameUI.manage.totalDriftCoins < PlayerPrefs.GetInt("track6"))
                {
                    GameUI.manage.gameFailed = true;
                }
            }

            if (playerBestTime == 0.0f || playerBestTime > playerCurrentTime) playerBestTime = playerCurrentTime;

            playerCurrentTime = 0.0f;
            getLap = true;
        }
        else if (AIControl.manage.startPoint != currentNode)
        {
            getLap = false;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////	

        Vector3 CurrentNodeForward = currentNode.TransformDirection(Vector3.forward);
        Vector3 CurrentNodetoOther = currentNode.position - frontPoint.position;

        Vector3 NextNodeForward = nextNode.TransformDirection(Vector3.forward);
        Vector3 NextNodetoOther = nextNode.position - frontPoint.position;

        carPreviousNodes = Mathf.Clamp(carPreviousNodes, 0, 6);


        if (Mathf.Abs(Quaternion.Dot(nextNode.rotation, frontPoint.rotation)) < 0.5f && !GameUI.manage.gameFinished)
            GameUI.manage.carWrongWay = true;
        else
            GameUI.manage.carWrongWay = false;


        if (nextNode)
        {

            if (vehicleControl.vehicleMode == VehicleMode.AICar)
            {
                if (Vector3.Distance(frontPoint.position, nextNode.position) < nextNodeDistance && nextNode != currentNode)
                {
                    currentNode = nextNode;
                    wrongNode = nextNode;
                    goNextNode = true;
                }

            }
            else if (vehicleControl.vehicleMode == VehicleMode.Player)
            {

                if (Vector3.Dot(NextNodeForward, NextNodetoOther) < 0.0f)
                {

                    carPreviousNodes--;
                    currentNode = nextNode;

                    if (wrongNode != null && wrongNode.GetComponent<AINode>().nextNode == nextNode) wrongNode = nextNode;

                    goNextNode = true;
                }
                else if (Vector3.Dot(CurrentNodeForward, CurrentNodetoOther) > 0.0f && currentNode.GetComponent<AINode>().previousNode != currentNode)
                {
                    carPreviousNodes++;
                    currentNode = currentNode.GetComponent<AINode>().previousNode;

                    if (carPreviousNodes == 5)
                    {
                        carPreviousNodes = 0;
                        transform.GetComponent<Rigidbody>().Sleep();
                        transform.rotation = wrongNode.rotation;
                        transform.position = wrongNode.position + Vector3.up;

                        currentNode = wrongNode;
                    }

                    goNextNode = true;
                }
            }

            if (Vector3.Distance(frontPoint.position, nextNode.position) > nextNodeDistance * 5)
            {
                carPreviousNodes = 0;
                transform.GetComponent<Rigidbody>().Sleep();
                transform.rotation = wrongNode.rotation;
                transform.position = wrongNode.position + Vector3.up;

                currentNode = wrongNode;
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////	

        GameUI.manage.carBrakeWarning = false;

        if (currentNode.GetComponent<AINode>())
        {
            AINode Nodescript = currentNode.GetComponent<AINode>();

            if (Mathf.Abs(AISteer) > 0.0f && vehicleControl.speed > 70.0f && Nodescript.nodeSetting.brakeing)
            {
                AIAccel = -Mathf.Abs(AISteer);
                GameUI.manage.carBrakeWarning = true;
            }
            else
            {
                AIAccel = 0.2f;
            }

            if (goNextNode)
            {
                nextNode = Nodescript.nextNode;
                goNextNode = false;
            }
        }

        var relativeTarget = transform.InverseTransformPoint(nextNode.position);

        targetAngle = Mathf.Atan2(relativeTarget.x, relativeTarget.z);
        targetAngle *= Mathf.Rad2Deg;
        targetAngle = Mathf.Clamp(targetAngle, -65, 65);

        AISteer = Mathf.SmoothStep(AISteer, targetAngle / 60, steerSpeed / 3.0f);
    }

}


