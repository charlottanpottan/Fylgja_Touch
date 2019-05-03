using UnityEngine;

public class QuestItem : Item
{
	public override void ExecuteAction(IAvatar avatar)
	{
		avatar.avatarQuest.questInventory.Add(name);
		base.ExecuteAction(avatar);
	}

	public override bool IsActionPossible(IAvatar avatar)
	{
		return avatar.NeedsToPickup(this);
	}
}
