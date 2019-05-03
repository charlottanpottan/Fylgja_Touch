using UnityEngine;
using System.Collections;

public class PickupItemsQuestPart : ActorSceneComponent
{
	public QuestItem[] itemsToPickup;

	AvatarQuest avatarQuest;

	void Start()
	{
		foreach (var item in itemsToPickup)
		{
			DebugUtilities.Assert(item != null, "We have null items in quest part:" + name);
		}
	}

	public override bool AvatarAllowedToInteract()
	{
		return true;
	}

	public override bool AvatarAllowedToMove()
	{
		return true;
	}

	protected override void Act()
	{
		var avatar = actingInScene.GetActor("Tyra");
		avatarQuest = avatar.GetComponentInChildren<AvatarQuest>();
		foreach (var questItem in itemsToPickup)
		{
			avatarQuest.AddInteractableTypeToLookFor(questItem.name);
		}
	}

	void Close()
	{
		foreach (var questItem in itemsToPickup)
		{
			avatarQuest.RemoveInteractableTypeToLookFor(questItem.name);
		}
		ComponentDone();
	}

	public override void Skip()
	{
		Close();
	}

	void OnInventoryAdd(string itemName)
	{
		if (avatarQuest.questInventory.HasAllItems(itemsToPickup))
		{
			Close();
		}
	}
}

