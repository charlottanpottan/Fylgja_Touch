using UnityEngine;
using System.Collections;

public class PlayerQuestParchment : ActionArbitration
{
	bool canOpenParchment = true;

	public override bool IsActionPossible(IAvatar avatar)
	{
		return canOpenParchment;
	}

	void OnMinigameStart()
	{
		canOpenParchment = false;
	}

	public void OnMinigameFailed()
	{
		canOpenParchment = true;
	}

	public void OnMinigameAborted()
	{
		canOpenParchment = true;
	}

	public void OnMinigameDone()
	{
		canOpenParchment = true;
	}

	public void OnEnterVehicle(Vehicle vehicle)
	{
		if (vehicle.IsAvatar)
		{
			return;
		}

		canOpenParchment = false;
	}

	public void OnLeaveVehicle()
	{
		canOpenParchment = true;
	}
	
	public void SetCanOpenParchmentState(bool state)
	{
		canOpenParchment = state;	
	}
	
	public override void ExecuteAction(IAvatar _avatar)
	{
		canOpenParchment = false;
	}
}

