using UnityEngine;
using System.Collections;

public class ActorShowInventoryBag : ActorSceneComponent
{
	public string actorName;
	CharacterInventoryBag characterInventoryBag;

	public override bool AvatarAllowedToInteract()
	{
		return true;
	}

	protected override void Act()
	{
		var avatarObject = actingInScene.GetActor(actorName);
		characterInventoryBag = avatarObject.GetComponentInChildren<CharacterInventoryBag>();

		avatarObject.BroadcastMessage("OnInventoryBagOpen");
		characterInventoryBag.notifyOnClose += OnClosedInventoryBag;
	}

	public override void Dispose()
	{
		characterInventoryBag.Dispose();
	}

	public override void Skip()
	{
		var avatar = actingInScene.GetMainAvatar();
		Debug.Log("ActorShowInventoryBag: CLOSE");
		characterInventoryBag.Close(avatar);
		ComponentDone();
	}

	void OnClosedInventoryBag(CharacterInventoryBag inventoryBag)
	{
		ComponentDone();
	}
}

