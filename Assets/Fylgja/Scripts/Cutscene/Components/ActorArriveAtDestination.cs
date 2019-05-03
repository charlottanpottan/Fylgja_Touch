using UnityEngine;
using System.Collections;

public class ActorArriveAtDestination : ActorSceneComponent
{
	public string areaName;

	AvatarQuest avatarQuest;
	GameObject goalObject;
	
	public override bool AvatarAllowedToMove()
	{
		return true;
	}

	public override bool AvatarAllowedToInteract()
	{
		return true;
	}
	
	protected override void Act()
	{
		Debug.Log("Starting Arrive At Destination: " + areaName);
		goalObject = GameObject.Find(areaName);
		DebugUtilities.Assert(goalObject != null, "Couldn't find object:" + areaName);

		var avatar = actingInScene.GetActor("Tyra");
		avatarQuest = avatar.GetComponentInChildren<AvatarQuest>();
		avatarQuest.AddGoalObject(goalObject);
	}

	public override void Skip()
	{
	}

	void Close()
	{
		var avatar = actingInScene.GetActor("Tyra");
		avatarQuest = avatar.GetComponentInChildren<AvatarQuest>();
		avatarQuest.RemoveGoalObject(goalObject);
		ComponentDone();
	}

	void OnTriggeredArea(string reachedArea)
	{
		if (reachedArea == areaName)
		{
			Debug.Log("Player reached area:" + areaName);
			Close();
		}
	}
}

