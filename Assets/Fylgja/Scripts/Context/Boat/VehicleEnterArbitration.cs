using UnityEngine;
using System.Collections;

public class VehicleEnterArbitration : ActionArbitration
{
	public Vehicle vehicleToArbitrate;

	public override bool IsActionPossible(IAvatar avatar)
	{
		return !vehicleToArbitrate.HasControllingAvatar();
	}

	public override void ExecuteAction(IAvatar avatar)
	{
		if (avatar.avatarQuest.IsLookingFor(this))
		{
			avatar.transform.parent.BroadcastMessage("OnInteractWith", GetComponentInChildren<Interactable>());
		}
		vehicleToArbitrate.SetControllingAvatar(avatar);
	}
}
