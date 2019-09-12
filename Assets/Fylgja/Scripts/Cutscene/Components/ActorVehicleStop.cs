using UnityEngine;
using System.Collections;

public class ActorVehicleStop : ActorSceneComponent
{
    protected override void Act()
    {
        GameObject actor = actingInScene.GetActor("Horse");
        VehicleMoveToPoint vehicleMoveToPoint = actor.GetComponent<VehicleMoveToPoint>();
        vehicleMoveToPoint.ClearTarget();

        ComponentDone();
    }

    public override void Skip()
    {
        ComponentDone();
    }
}
