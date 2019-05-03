using UnityEngine;
using System.Collections;

public class ActorSetInteraction : ActorSceneComponent
{
	public string actorName;
	public bool enableInteraction;

	protected override void Act()
	{
		GameObject o = actingInScene.GetActor(actorName);

		var interactable = o.GetComponentInChildren<Interactable>();

		if (enableInteraction)
		{
			interactable.EnableInteraction();
		}
		else
		{
			interactable.DisableInteraction();
		}

		ComponentDone();
	}

	public override void Skip()
	{
		ComponentDone();
	}
}
