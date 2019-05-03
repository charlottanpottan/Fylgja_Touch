using UnityEngine;
using System.Collections;

public class ActorShowMinigameParchment : ActorShowQuestParchment
{
	public GameObject parchmentPrefab;


	protected override void Act()
	{
		var avatarObject = actingInScene.GetActor(actorName);
		characterQuestParchment = avatarObject.GetComponentInChildren<CharacterQuestParchment>();
		characterQuestParchment.SpawnSpecificParchment(parchmentPrefab);
		characterQuestParchment.notifyOnClose += OnClosedQuestParchment;
	}

}

