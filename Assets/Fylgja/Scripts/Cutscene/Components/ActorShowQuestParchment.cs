using UnityEngine;
using System.Collections;

public class ActorShowQuestParchment : ActorSceneComponent
{
	public string actorName;

	protected CharacterQuestParchment characterQuestParchment;

	public override bool AvatarAllowedToInteract()
	{
		return true;
	}

	protected override void Act()
	{
		var avatarObject = actingInScene.GetActor(actorName);
		characterQuestParchment = avatarObject.GetComponentInChildren<CharacterQuestParchment>();

		avatarObject.BroadcastMessage("OnQuestParchmentOpen");
		characterQuestParchment.notifyOnClose += OnClosedQuestParchment;
	}

	public override void Dispose()
	{
		characterQuestParchment.Dispose();
	}

	void Close()
	{
		ComponentDone();
	}

	public override void Skip()
	{
		Close();
	}

	protected void OnClosedQuestParchment(CharacterQuestParchment questParchment)
	{
		questParchment.notifyOnClose -= OnClosedQuestParchment;
		Close();
	}
}
