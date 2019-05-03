using UnityEngine;
using System.Collections;

public class ActorStartQuest : ActorSceneComponent
{
	public string actorName;
	public Quest quest;

	protected override void Act()
	{
		var avatarObject = actingInScene.GetActor(actorName);
		var avatarQuest = avatarObject.GetComponentInChildren<AvatarQuest>();
		avatarQuest.CreateQuest(quest);
		ComponentDone();
	}

	public override void Skip()
	{
		ComponentDone();
	}
}
