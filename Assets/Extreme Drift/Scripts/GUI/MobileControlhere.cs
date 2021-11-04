using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileControlhere : MonoBehaviour
{
    ////////////////////////////////////////////// TouchMode (Control) ////////////////////////////////////////////////////////////////////

    public void CarAccelForward(float amount)
    {
        AIControlDaim.CurrentVehicle.accelFwd = amount;
    }
    public void CarAccelBack(float amount)
    {
        AIControlDaim.CurrentVehicle.accelBack = amount;
    }
    public void CarSteer(float amount)
    {
        AIControlDaim.CurrentVehicle.steerAmount = amount;
    }
    public void CarHandBrake(bool HBrakeing)
    {
        AIControlDaim.CurrentVehicle.brake = HBrakeing;
    }
    public void CarShift(bool Shifting)
    {
        AIControlDaim.CurrentVehicle.shift = Shifting;
    }

    public void NitroVibrate(bool nitrovib)
    {
        AIControlDaim.CurrentVehicle.nitrovibrate = nitrovib;
    }
}
