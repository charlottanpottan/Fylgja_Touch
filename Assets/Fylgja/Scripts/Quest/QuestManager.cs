using UnityEngine;
using System.Collections;

public class QuestManager : MonoBehaviour
{
	public Quest[] quests;

	public Quest SpawnQuest(string questName)
	{
		foreach (var quest in quests)
		{
			if (quest.questName == questName)
			{
				var instantiatedQuestObject = Instantiate(quest) as Quest;
				DebugUtilities.Assert(instantiatedQuestObject != null, "Object does not implement Quest:" + questName);
				return instantiatedQuestObject;
			}
		}
		Debug.LogError("Couldn't find quest: " + questName);
		return null;
	}
}

