using UnityEngine;
using System.Collections.Generic;

public class IAvatar : MonoBehaviour
{
	public AvatarQuest avatarQuest;
	public Player player;
	public AvatarToPlayerNotifications playerNotifications;
	Vehicle vehicle;

	List<AllowedToMoveModifier> allowedToMoveModifiers = new List<AllowedToMoveModifier>();
	List<AllowedToInteractModifier> allowedToInteractModifiers = new List<AllowedToInteractModifier>();

	public void AddAllowedToMoveModifier(AllowedToMoveModifier modifier)
	{
		if (allowedToMoveModifiers.Count == 0)
		{
			Debug.Log("Adding move modifier:" + modifier + ". You are NOT allowed to move.");
			SetAllowedToMove(false);
		}
		else
		{
			Debug.Log("Adding move modifier:" + modifier + ". Someone else has restricted already.");
		}
		allowedToMoveModifiers.Add(modifier);
	}

	public void RemoveAllowedToMoveModifier(AllowedToMoveModifier modifier)
	{
		allowedToMoveModifiers.Remove(modifier);
		if (allowedToMoveModifiers.Count == 0)
		{
			Debug.Log("Removing move modifier:" + modifier + ". You are allowed to move!");
			SetAllowedToMove(true);
		}
		else
		{
			Debug.Log("Removed move modifier:" + modifier + ". But not allowed to move!");
		}
	}

	public void AddAllowedToInteractModifier(AllowedToInteractModifier modifier)
	{
		if (allowedToInteractModifiers.Count == 0)
		{
			SetAllowedToInteract(false);
		}
		allowedToInteractModifiers.Add(modifier);
	}

	public void RemoveAllowedToInteractModifier(AllowedToInteractModifier modifier)
	{
		allowedToInteractModifiers.Remove(modifier);

		if (allowedToInteractModifiers.Count == 0)
		{
			SetAllowedToInteract(true);
		}
	}
	
	public virtual void OnSubtitleStart(string text)
	{
		if (playerNotifications != null)
		{
			playerNotifications.OnSubtitleStart(text);
		}
	}
	public virtual void OnSubtitleStop()
	{
		if (playerNotifications != null)
		{
			playerNotifications.OnSubtitleStop();
		}
	}

	public virtual void OnCutsceneStart()
	{
		if (playerNotifications != null)
		{
			playerNotifications.OnCutsceneStart();
		}
	}

	public virtual void OnCutsceneEnd()
	{
		if (playerNotifications != null)
		{
			playerNotifications.OnCutsceneEnd();
		}
	}

	public virtual void Climb(Transform transform)
	{
	}
	public virtual void Pickup(Item item)
	{
	}
	public virtual void TalkTo(Npc character)
	{
	}
	public virtual bool IsInspectingBelongings()
	{
		return false;
	}
	public virtual void OnInspectingBelongingsOpen()
	{
	}
	public virtual void OnInspectingBelongingsClose()
	{
	}
	public virtual void OpenInventoryBag()
	{
	}
	public virtual void CloseInventoryBag()
	{
	}
	public virtual void ShowQuestParchment()
	{
	}
	public virtual void CloseQuestParchment()
	{
	}
	public virtual void IgniteFirePit(FirePit pit)
	{
	}
	public virtual void FanFirePit(FirePit pit)
	{
	}
	public virtual bool IsIgniting()
	{
		return false;
	}
	public virtual void SnapTo(Transform t)
	{
	}
	public virtual void OnFirePitFullFlame()
	{
	}
	public virtual void PickupFirePitTools()
	{
	}
	public virtual void QuitMinigame()
	{
	}
	public virtual bool IsPlayingMinigame()
	{
		return false;
	}
	public virtual void BlendToLocomotion()
	{
	}
	public virtual bool HasBlacksmithTools()
	{
		return false;
	}
	protected virtual void SetAllowedToMove(bool allowedToMove)
	{
	}
	protected virtual void SetAllowedToInteract(bool allowedToInteract)
	{
	}
	public virtual bool IsSmithing()
	{
		return false;
	}
	public virtual void StartAimingSledgehammer()
	{
	}
	public virtual bool IsInVehicleOutsideAvatar()
	{
		return true;
	}
	public virtual bool HasFightStick()
	{
		return false;
	}
	public virtual bool NeedsToPickup(QuestItem questItem)
	{
		return false;
	}
	public virtual void PassedCheckpoint(CheckpointId id)
	{
	}
	public virtual bool IsWalkingToTarget()
	{
		return false;
	}

	public virtual void WalkToTarget(Vector3 position, Quaternion rotation)
	{
	}

	public void TryToPerformPrimaryAction(GameObject obj, Vector3 targetPosition, Quaternion targetRotation)
	{
		var action = ActionUtility.FindPrimaryAction(obj, this);
		if (!action)
		{
			Debug.Log("No primary action found on:" + obj.name);
			return;
		}
		TryToPerformAction(action, targetPosition, targetRotation);
	}
	public virtual void TryToPerformAction(Action interactable, Vector3 targetPosition, Quaternion targetRotation)
	{
	}

	public void PerformPrimaryAction(GameObject obj)
	{
		var action = ActionUtility.FindPrimaryAction(obj, this);
		if (!action)
		{
			Debug.Log("No primary action found on:" + obj.name);
			return;
		}
		PerformAction(action);
	}

	public virtual void PerformAction(Action interactable)
	{
	}

	public void AssignVehicle(Vehicle vehicleToControl)
	{
		vehicle = vehicleToControl;
		Debug.Log("Avatar is assigned new vehicle:" + vehicle);
		transform.root.BroadcastMessage("OnEnterVehicle", vehicle);
		playerNotifications.OnAssignedVehicle(vehicle);
	}

	public Vehicle ControlledVehicle()
	{
		return vehicle;
	}


	public virtual void ActionRequestDenied(ActionArbitration arbitrator)
	{
	}
	public virtual void ActionRequestAccepted(ActionArbitration arbitrator)
	{
	}
}

