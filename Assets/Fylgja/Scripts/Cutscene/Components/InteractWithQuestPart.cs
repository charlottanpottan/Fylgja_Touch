using UnityEngine;

public class InteractWithQuestPart : ActorSceneComponent
{
	public string interactableName;
	public string description;

	Interactable goalObject;
	AvatarQuest avatarQuest;

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
		Debug.Log("Interactable: " + interactableName);

		goalObject = Interactable.GetInteractableFromName(interactableName);
		DebugUtilities.Assert(goalObject != null, "Couldn't find interactable component on:" + interactableName);

		var avatar = actingInScene.GetActor("Tyra");
		avatarQuest = avatar.GetComponentInChildren<AvatarQuest>();
		avatarQuest.AddGoalObject(goalObject.gameObject);
	}

	void Close()
	{
		DebugUtilities.Assert(avatarQuest != null, "Avatar quest is null, did you close it before Act?");
		DebugUtilities.Assert(goalObject != null, "Goal object is null, did it unspawn during interact?");
		Debug.Log("Closing Interact with:" + interactableName);
		avatarQuest.RemoveGoalObject(goalObject.gameObject);
		goalObject = null;
		ComponentDone();
	}

	public override void Skip()
	{
		Close();
	}

	public void OnInteractWith(Interactable interactable)
	{
		Debug.Log("OnInteractWith:" + interactable.name);
		if (interactable.name == interactableName)
		{
			Close();
		}
	}

}
