using UnityEngine;
using System.Collections;

public class ActorMessage : ActorSceneComponent
{
	public string actorName;
	public string message;
	public float floatParameter = 1;
	bool sentMessage;

	protected override void Act()
	{
		var actorObject = actingInScene.GetActor(actorName);

		actorObject.transform.root.BroadcastMessage(message, floatParameter, SendMessageOptions.RequireReceiver);
		sentMessage = true;
	}

	public void Update()
	{
		if (sentMessage)
		{
			ComponentDone();
			sentMessage = false;
		}
	}

	public override void Skip()
	{
	}
}