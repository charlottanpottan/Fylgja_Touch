using UnityEngine;
using System.Collections;

public class ActorExitVehicle : ActorSceneComponent
{
	public string vehicleName;
	public string actorName;

	protected override void Act()
	{
		var avatarObject = actingInScene.GetActor(actorName);
		var vehicleObject = actingInScene.GetActor(vehicleName);

		Debug.Log("Found vehicle:" + vehicleObject.name);

		var vehicle = vehicleObject.GetComponentInChildren<Vehicle>();

		Debug.Log("Found vehicle:" + vehicle.name);

		var avatar = avatarObject.GetComponentInChildren<IAvatar>();
		var avatarAsVehicle = avatarObject.GetComponentInChildren<CharacterWalking>();

		avatar.transform.root.BroadcastMessage("OnLeaveVehicle");
		avatar.AssignVehicle(avatar.transform.root.GetComponentInChildren<CharacterWalking>());


		avatar.AssignVehicle(avatarAsVehicle);

		ComponentDone();
	}

	public override void Skip()
	{
		ComponentDone();
	}
}
