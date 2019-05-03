using UnityEngine;

public class PlayerInventoryBag : ActionArbitration
{
	bool canOpenBag = true;

	public override bool IsActionPossible(IAvatar avatar)
	{
		return canOpenBag;
	}

	void OnMinigameStart()
	{
		canOpenBag = false;
	}

	public void OnMinigameFailed()
	{
		canOpenBag = true;
	}

	public void OnMinigameAborted()
	{
		canOpenBag = true;
	}

	public void OnMinigameDone()
	{
		canOpenBag = true;
	}

	public void OnEnterVehicle(Vehicle vehicle)
	{
		if (vehicle.IsAvatar)
		{
			return;
		}
		canOpenBag = false;
	}

	public void OnLeaveVehicle()
	{
		canOpenBag = true;
	}
	
	public void SetCanOpenBagState(bool state)
	{
		canOpenBag = state;	
	}
	
	public override void ExecuteAction(IAvatar _avatar)
	{
		canOpenBag = false;
	}
}
