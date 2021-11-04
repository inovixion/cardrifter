using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMaphere : MonoBehaviour {

    public Transform mapPlane;
    public Transform miniMapCamera;

    void Update()
    {
        miniMapCamera.position = new Vector3(AIControlDaim.CurrentVehicle.transform.position.x, mapPlane.position.y+25, AIControlDaim.CurrentVehicle.transform.position.z);
        miniMapCamera.eulerAngles = new Vector3(90, AIControlDaim.CurrentVehicle.transform.eulerAngles.y, 0);
    }
}
