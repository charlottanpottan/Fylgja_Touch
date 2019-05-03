using UnityEngine;
using System.Collections.Generic;

public class QuestGiver : ItemGiver
{
	public Quest[] quests;

	private Quest FirstQuestNotCompletedOrStarted(ICollection<string> questsCompleted, ICollection<string> questsStarted)
	{
		foreach (var quest in quests)
		{
			if (!questsCompleted.Contains(quest.questName) && !questsStarted.Contains(quest.questName) && (quest.priorQuest == null || questsCompleted.Contains(quest.priorQuest.questName)))
			{
				return quest;
			}
		}
		return null;
	}

	public Quest QuestForAvatar(AvatarQuest avatar)
	{
		return FirstQuestNotCompletedOrStarted(avatar.CompletedQuests(), avatar.StartedQuests());
	}

	public void AvatarWantsNewQuest(AvatarQuest avatarQuest)
	{
		Quest quest = QuestForAvatar(avatarQuest);

		avatarQuest.CreateQuest(quest);
	}
}
