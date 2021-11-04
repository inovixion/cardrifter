using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VehicleMode { Player = 0, AICar = 1 }
public enum ControlMode { Simple = 1, Mobile = 2 }

public class AIControlDaim : MonoBehaviour
{

    public static AIControlDaim manage;
    public static VehicleControlDaim CurrentVehicle;

    public ControlMode controlMode = ControlMode.Simple;

    public Transform firstAINode;
    public Transform startPoint;

    public GameObject[] CarsPrefabs;

    void Awake()
    {
        manage = this;
    }

    void Start()
    {

        GameObject InstantiatedCar = Instantiate(CarsPrefabs[PlayerPrefs.GetInt("CurrentVehicle")], startPoint.position, startPoint.rotation) as GameObject;

        InstantiatedCar.GetComponent<AIVehicleDaim>().nextNode = firstAINode;

        CurrentVehicle = InstantiatedCar.GetComponent<VehicleControlDaim>();

        VehicleCameraDaim.manage.target = InstantiatedCar.transform;
        VehicleCameraDaim.manage.cameraSwitchView = CurrentVehicle.cameraView.cameraSwitchView;

        VehicleCameraDaim.manage.distance = CurrentVehicle.cameraView.distance;
        VehicleCameraDaim.manage.height = CurrentVehicle.cameraView.height;
        VehicleCameraDaim.manage.angle = CurrentVehicle.cameraView.angle;
    }

}
