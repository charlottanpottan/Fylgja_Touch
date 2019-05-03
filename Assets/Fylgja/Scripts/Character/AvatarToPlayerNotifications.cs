using UnityEngine;
using System.Collections;

public class AvatarToPlayerNotifications : MonoBehaviour
{
	public Player player;

	void Start()
	{
	}

	void Update()
	{
	}

	public void FadeUp()
	{
		player.playerInteraction.FadeUp();
	}

	public PlayerInteraction.ListenerStackItem AttachListener(Transform attachTransform)
	{
		return player.playerInteraction.AttachListenerToTransform(attachTransform);
	}

	public void DetachListener(PlayerInteraction.ListenerStackItem item)
	{
		player.playerInteraction.DetachListener(item);
	}

	public PlayerInteraction.CameraItem AddCameraToStack(LogicCamera camera, string cameraName)
	{
		return player.playerInteraction.AddCameraToStack(camera, cameraName);
	}

	public void RemoveCameraFromStack(PlayerInteraction.CameraItem cameraItem)
	{
		player.playerInteraction.RemoveCameraFromStack(cameraItem);
	}

	public void OnSubtitleStart(string text)
	{
		player.playerInteraction.OnSubtitleStart(text);
	}
	
	public void OnSubtitleStop()
	{
		player.playerInteraction.OnSubtitleStop();
	}
	
	public void OnAssignedVehicle(Vehicle vehicle)
	{
		player.playerInteraction.OnAssignedVehicle(vehicle);
	}

	public void OnCutsceneStart()
	{
		player.playerInteraction.OnCutsceneStart();
	}

	public void OnCutsceneEnd()
	{
		player.playerInteraction.OnCutsceneEnd();
	}

	void OnPassedCheckpoint(CheckpointId id)
	{
		bool isNewCheckpointForMe = player.playerStorage.SetStartCheckpoint(id);

		if (isNewCheckpointForMe)
		{
			Debug.Log("Checkpoint triggered: Yes, this is a new checkpoint");
			player.playerStorage.Save();
		}
		else
		{
			Debug.Log("Checkpoint triggered: I have been here before");
		}
	}


	public void OnStartedQuest(string questName)
	{
		player.playerStorage.AddStartedQuest(questName);
	}


	public void OnQuestCompleted(string questName)
	{
		player.playerStorage.CompletedQuest(questName);
	}

	public void OnQuestClosed(string questName)
	{
		player.playerStorage.RemoveStartedQuest(questName);
	}

	public void OnStartedQuestPart(Quest quest, ActorSceneComponent component)
	{
		player.playerStorage.SetStartedQuestPart(quest.questName, component.name);
	}

	public void OnInventoryAdd(string name)
	{
		player.playerStorage.AddInventoryQuestItem(name);
	}

	public void OnInventoryRemove(string name)
	{
		player.playerStorage.RemoveInventoryQuestItem(name);
	}

	public void OnAllowedToMove(bool allowed)
	{
		if (player == null)
		{
			return;
		}
		player.playerInteraction.OnAllowedToMove(allowed);
	}

	public void OnAllowedToInteract(bool interact)
	{
		if (player == null)
		{
			return;
		}
		player.playerInteraction.OnAllowedToInteract(interact);
	}

	public void OnMinigameStart(Minigame game)
	{
		if (player == null)
		{
			return;
		}
		player.playerInteraction.OnMinigameStart(game);
	}

	public void OnMinigameDone(Minigame game)
	{
		if (player == null)
		{
			return;
		}
		player.playerInteraction.OnMinigameDone(game);
	}

	public void OnMinigameFailed(Minigame game)
	{
		if (player == null)
		{
			return;
		}
		player.playerInteraction.OnMinigameFailed(game);
	}

	public void OnMinigameAborted(Minigame game)
	{
		if (player == null)
		{
			return;
		}
		player.playerInteraction.OnMinigameAborted(game);
	}
}
