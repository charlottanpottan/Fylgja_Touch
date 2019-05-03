using UnityEngine;
using System.Collections;

public class ActorEnterVehicle : ActorSceneComponent
{
	public string vehicleName;
	public string actorName;

	protected override void Act()
	{
		var avatarObject = actingInScene.GetActor(actorName);
		var vehicleObject = actingInScene.GetActor(vehicleName);

		var vehicle = vehicleObject.GetComponentInChildren<Vehicle>();
		var avatar = avatarObject.GetComponentInChildren<IAvatar>();

		avatar.AssignVehicle(vehicle);

		ComponentDone();
	}

	public override void Skip()
	{
		ComponentDone();
	}
}

