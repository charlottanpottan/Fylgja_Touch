using UnityEngine;
using System.Collections;

public class ActorMessageTemplate<T> : ActorSceneComponent
{
	public string actorName;
	public string message;
    public T value;
	bool sentMessage;

	protected override void Act()
	{
		var actorObject = actingInScene.GetActor(actorName);

		actorObject.transform.root.BroadcastMessage(message, value, SendMessageOptions.RequireReceiver);
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