using UnityEngine;
using System.Collections;

public class ActorMessageVector2 : ActorSceneComponent
{
	public string actorName;
	public string message;
	public Vector2 vectorParameter = Vector2.zero;
	bool sentMessage;

	protected override void Act()
	{
		var actorObject = actingInScene.GetActor(actorName);

		actorObject.transform.root.BroadcastMessage(message, vectorParameter, SendMessageOptions.RequireReceiver);
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