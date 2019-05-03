using UnityEngine;
using System.Collections;

public class ActorClearInventory : ActorSceneComponent
{
	public string actorName;

	protected override void Act()
	{
		GameObject o = actingInScene.GetActor(actorName);

		var questInventory = o.GetComponentInChildren<QuestInventory>();

		questInventory.ClearAllItems();
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

