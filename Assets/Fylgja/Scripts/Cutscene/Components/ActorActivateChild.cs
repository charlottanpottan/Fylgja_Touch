using UnityEngine;
using System.Collections;

public class ActorActivateChild : ActorSceneComponent
{
	public string actorName;
	public string gameObjectToActivate;
	public bool activationState = false;

	protected override void Act()
	{
		var actorObject = actingInScene.GetActor(actorName);
		
		Transform t = actorObject.transform.Find(gameObjectToActivate);
		
		t.gameObject.SetActive(activationState);
		
		ComponentDone();
	}

	public override void Skip()
	{
		ComponentDone();
	}
}