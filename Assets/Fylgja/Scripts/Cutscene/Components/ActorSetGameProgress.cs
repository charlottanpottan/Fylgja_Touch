using UnityEngine;
using System.Collections;

public class ActorSetGameProgress : ActorSceneComponent
{
	public string actorName;
	public Quest quest;
	public int checkpointId;

	protected override void Act()
	{
		var avatarObject = actingInScene.GetActor(actorName);
		var avatar = avatarObject.GetComponentInChildren<IAvatar>();

		DebugUtilities.Assert(avatar != null, "No real avatar:" + actorName);
		DebugUtilities.Assert(avatar.player != null, "No player attached to:" + actorName);
		DebugUtilities.Assert(avatar.player.playerStorage != null, "No storage connected to player: " + actorName);
		var storage = avatar.player.playerStorage;

		storage.SetStartCheckpoint(new CheckpointId(checkpointId));
		if (quest != null)
		{
			storage.AddStartedQuest(quest.questName);
			storage.SetStartedQuestPart(quest.questName, string.Empty);
		}

		Global.levelId = storage.LevelIdFromCheckpoint(storage.playerData().startingCheckpointId);
		Debug.Log("Loading level: " + Global.levelId);
		ComponentDone();
	}

	public override void Skip()
	{

	}
}

