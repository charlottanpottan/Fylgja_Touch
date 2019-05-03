using UnityEngine;

public class VehicleLeaveAction : ActionArbitration
{
	public override bool IsActionPossible(IAvatar avatar)
	{
		return avatar.IsInVehicleOutsideAvatar();
	}

	public override void ExecuteAction(IAvatar avatar)
	{
		avatar.transform.root.BroadcastMessage("OnLeaveVehicle");
		avatar.AssignVehicle(avatar.transform.root.GetComponentInChildren<CharacterWalking>());
	}
}
