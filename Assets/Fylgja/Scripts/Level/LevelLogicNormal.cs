using UnityEngine;
using System.Collections.Generic;

public class LevelLogicNormal : MonoBehaviour
{
	public PlayerCheckpointManager checkpointManager;
	public GameObject characterAvatarToSpawn;
	public MinimapCamera minimapCamera;
	public Minimap minimap;
	public MinimapSurface minimapSurface;
	public QuestManager quests;


	void Start()
	{
		Global.levelId = null;

		if (IsOnlinePlay())
		{
			var networkObject = GameObject.Find("NetworkHost(Clone)");
			if (networkObject == null)
			{
				networkObject = GameObject.Find("NetworkClient(Clone)");
			}
			var networkLevel = networkObject.GetComponent<NetworkLevel>();
			networkLevel.OnLevelLoaded();
		}
	}



	public void OnPlayerEnter(Player player)
	{
		DebugUtilities.Assert(player != null, "Must have a valid player");
		DebugUtilities.Assert(player.playerStorage != null, "storage");
		DebugUtilities.Assert(player.playerStorage.playerData() != null, "playerData is null");
		Debug.Log("A player requested to play!");
		var checkpointId = player.playerStorage.playerData().startingCheckpointId;
		DebugUtilities.Assert(checkpointId != null, "Starting ChecckpointId");
		var checkpoint = checkpointManager.PlayerCheckpointFromId(checkpointId);
		if (checkpoint == null)
		{
			checkpoint = checkpointManager.DebugFirstCheckpoint();
		}
		Debug.Log("Spawning:" + characterAvatarToSpawn.name);



		var characterObject = InstantiateControlledAvatar(characterAvatarToSpawn, checkpoint.spawnTransform);
		// var characterObject = characterAvatarToSpawn;
		DebugUtilities.Assert(characterObject != null, "Couldn't spawn");
		var characterAvatar = characterObject.GetComponentInChildren<CharacterAvatar>();
		var avatarQuest = characterObject.GetComponentInChildren<AvatarQuest>();
		DebugUtilities.Assert(characterAvatar != null, "Couldn't find CharacterAvatar");
		player.AssignAvatar(characterAvatar);
		characterAvatar.AssignVehicle(characterAvatar.transform.root.GetComponentInChildren<CharacterWalking>());
		checkpoint.WarpCharacter(characterObject);
		var data = player.playerStorage.playerData();
		SetupQuestProgress(player, avatarQuest, data.startedQuests, data.completedQuestIds);
		var avatarQuestInventory = characterObject.GetComponentInChildren<QuestInventory>();
		SetupInventory(avatarQuestInventory, data.inventoryQuestItemIds);
	}

	void SetupQuestProgress(Player player, AvatarQuest avatarQuest, List<PlayerStorageDataStartedQuest> startedQuests, ICollection<string> completedQuests)
	{
		avatarQuest.SetCompletedQuests(completedQuests);

		if (startedQuests.Count == 0)
		{
			Debug.Log("You are not on a quest. Then I just fade up and hope for the best");
			player.playerInteraction.FadeUp();
			return;
		}

		foreach (var startedQuest in startedQuests)
		{
			Debug.Log("Putting you on the Quest:" + startedQuest.questId + " at part:" + startedQuest.questPart);
			var quest = quests.SpawnQuest(startedQuest.questId);
			quest.SkipToComponent(startedQuest.questPart);
			avatarQuest.SetQuestButDontReport(quest);
		}
	}

	void SetupInventory(QuestInventory inventory, ICollection<string> inventoryItems)
	{
		inventory.SetItems(inventoryItems);
	}

	GameObject InstantiateControlledAvatar(GameObject characterAvatarToSpawn, Transform spawnTransform)
	{
		GameObject characterObject;

		if (IsOnlinePlay())
		{
			int group = 0;
			characterObject = Network.Instantiate(characterAvatarToSpawn, spawnTransform.position, spawnTransform.rotation, group) as GameObject;
		}
		else
		{
			characterObject = Instantiate(characterAvatarToSpawn, spawnTransform.position, spawnTransform.rotation) as GameObject;
		} return characterObject;
	}

	bool IsOnlinePlay()
	{
		return Network.peerType != NetworkPeerType.Disconnected;
	}
}

