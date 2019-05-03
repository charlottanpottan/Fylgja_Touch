using UnityEngine;
using System.Collections;

public class ActorAddQuestItemToInventory : ActorSceneComponent
{
	public string actorName;
	public string[] questItemNames;

	protected override void Act()
	{
		GameObject o = actingInScene.GetActor(actorName);
		var questInventory = o.GetComponentInChildren<QuestInventory>();

		foreach (var questItemName in questItemNames)
		{
			questInventory.Add(questItemName);
		}

		Close();
	}

	void Close()
	{
		ComponentDone();
	}

	public override void Skip()
	{
		Close();
	}
}

