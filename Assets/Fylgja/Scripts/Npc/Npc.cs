using UnityEngine;
using System;

public class Npc : ActionArbitration
{
	public QuestGiver questGiver;

	void Update()
	{
		Physics.sleepThreshold = 0;
	}

	public override void ExecuteAction(IAvatar avatar)
	{
		if (avatar.avatarQuest.IsLookingFor(this))
		{
			avatar.transform.parent.BroadcastMessage("OnInteractWith", GetComponentInChildren<Interactable>());
		}
		else if (questGiver != null)
		{
			questGiver.AvatarWantsNewQuest(avatar.avatarQuest);
		}
	}

	public override bool IsActionPossible(IAvatar avatar)
	{
		if (avatar.avatarQuest.IsLookingFor(this))
		{
			return true;
		}
		else if (questGiver != null)
		{
			var questToOffer = questGiver.QuestForAvatar(avatar.avatarQuest);
			if (questToOffer != null)
			{
				return true;
			}
		}

		return false;
	}
}
