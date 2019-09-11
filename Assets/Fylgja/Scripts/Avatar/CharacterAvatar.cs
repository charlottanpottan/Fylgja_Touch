using UnityEngine;
using System;

public class CharacterAvatar : IAvatar
{
	private bool inspectingBelongings;
	public CharacterWalking characterWalking;
	public VehicleMoveToPoint walkToPoint;
	public CharacterBlacksmith characterBlacksmithing;
	public Minigame activeMinigame;
	public CharacterVehicle characterVehicle;
	public CharacterStickFight characterStickFight;
	public float interactionDistanceThreshold = 0.4f;

	private Action actionToWalkTo;

	private Vector3 targetInteractPosition;
	private Quaternion targetInteractRotation;

	void Start()
	{
	}

	void Update()
	{
		if (actionToWalkTo != null && !IsWalkingToTarget() && CloseEnoughToPerformAction())
		{
			PerformAction(actionToWalkTo);
		}
	}

	public override void WalkToTarget(Vector3 position, Quaternion rotation)
	{
		walkToPoint.MoveToTarget(position, rotation);
	}

	public override bool IsWalkingToTarget()
	{
		return walkToPoint.IsWalkingToTarget();
	}

	public override void Climb(Transform transform)
	{
	}

	public bool IsClimbing()
	{
		return false;
	}

	public override void OpenInventoryBag()
	{
		transform.parent.BroadcastMessage("OnInventoryBagOpen");
	}

	public override void CloseInventoryBag()
	{
		transform.parent.BroadcastMessage("OnInventoryBagClose");
	}

	public override void ShowQuestParchment()
	{
		transform.parent.BroadcastMessage("OnQuestParchmentOpen");
	}

	public override void CloseQuestParchment()
	{
		transform.parent.BroadcastMessage("OnQuestParchmentClose");
	}

	public override void IgniteFirePit(FirePit pit)
	{
		transform.parent.BroadcastMessage("OnFirePitIgnite", pit);
	}

	public override void FanFirePit(FirePit pit)
	{
		transform.parent.BroadcastMessage("OnFanRequested", pit);
	}

	public override void SnapTo(Transform t)
	{
		transform.parent.BroadcastMessage("OnSnapTo", t);
	}

	public void OnMinigameStart(Minigame game)
	{
		Debug.Log("Avatar is playing minigame!");
		activeMinigame = game;
		playerNotifications.OnMinigameStart(game);
	}

	public void OnMinigameDone(Minigame game)
	{
		activeMinigame = null;
		playerNotifications.OnMinigameDone(game);
	}

	public void OnMinigameFailed(Minigame game)
	{
		activeMinigame = null;
		playerNotifications.OnMinigameFailed(game);
	}

	public void OnMinigameAborted(Minigame game)
	{
		activeMinigame = null;
		playerNotifications.OnMinigameAborted(game);
	}

	public override bool IsPlayingMinigame()
	{
		return(activeMinigame != null);
	}

	public override void QuitMinigame()
	{
		Debug.Log("Avatar quit minigame");
		activeMinigame.QuitMinigame();
	}

	public override void BlendToLocomotion()
	{
		characterWalking.BlendToLocomotion();
	}

	public void TurnOffLocomotion()
	{
		characterWalking.TurnOffLocomotion();
	}

	public override bool HasBlacksmithTools()
	{
		return true;
	}

	protected override void SetAllowedToMove(bool allowedToMove)
	{
		Debug.Log("SetAllowedToMove:" + allowedToMove + " on " + name + " for vehicle:" + characterVehicle.name);
		walkToPoint.ClearTarget();
		characterVehicle.SetAllowedToMove(allowedToMove);
		playerNotifications.OnAllowedToMove(allowedToMove);
	}

	protected override void SetAllowedToInteract(bool interact)
	{
		Debug.Log("SetAllowedToInteract:" + interact + " on " + name);
		playerNotifications.OnAllowedToInteract(interact);
	}

	public override void StartAimingSledgehammer()
	{
		characterBlacksmithing.StartAimingSledgehammer();
	}

	public override bool IsInVehicleOutsideAvatar()
	{
		return characterVehicle.IsControllingVehicleOutsideAvatar();
	}

	public override bool IsSmithing()
	{
		return characterBlacksmithing.IsSmithing();
	}

	public override bool HasFightStick()
	{
		return characterStickFight.HasFightStick();
	}

	public override bool NeedsToPickup(QuestItem questItem)
	{
		return avatarQuest.IsLookingFor(questItem);
	}

	public override void PassedCheckpoint(CheckpointId id)
	{
		// transform.parent.BroadcastMessage("OnPassedCheckpoint", id);
		Debug.Log("Passed checkpoint:" + id.CheckpointIdValue());
		if (playerNotifications)
		{
			Debug.Log("Notifying Player about checkpoint");
			playerNotifications.SendMessage("OnPassedCheckpoint", id);
		}
	}

	void OnInventoryRemove(string itemName)
	{
		if (playerNotifications)
		{
			playerNotifications.OnInventoryRemove(itemName);
		}
	}

	void OnInventoryAdd(string itemName)
	{
		if (playerNotifications)
		{
			playerNotifications.OnInventoryAdd(itemName);
		}
	}

	void OnQuestClosed(string questName)
	{
		if (playerNotifications != null)
		{
			playerNotifications.OnQuestClosed(questName);
		}
	}

	void OnQuestCompleted(string questName)
	{
		if (playerNotifications != null)
		{
			playerNotifications.OnQuestCompleted(questName);
		}
	}

	void OnStartedQuest(string questName)
	{
		if (playerNotifications != null)
		{
			playerNotifications.OnStartedQuest(questName);
		}
	}

	void OnStartedQuestPart(QuestPartNotification notification)
	{
		if (playerNotifications != null)
		{
			playerNotifications.OnStartedQuestPart(notification.quest, notification.part);
		}
	}

	private bool CloseEnoughToPerformAction()
	{
		Vector3 targetPosition = new Vector3(targetInteractPosition.x, 0, targetInteractPosition.z);
		Vector3 avatarPosition = new Vector3(characterWalking.transform.position.x, 0, characterWalking.transform.position.z);
		float distanceToInteractionPoint = (targetPosition - avatarPosition).magnitude;
		
		//float deltaY = Mathf.Abs(Angle.AngleDiff(targetInteractRotation.eulerAngles.y, characterWalking.transform.eulerAngles.y));
//		Debug.Log("RotationDiff:" + deltaY + " target:" + targetInteractRotation.eulerAngles.y + " source:" + characterWalking.transform.eulerAngles.y);
		//if (deltaY > 5)
		//{
        //    Debug.Log("DeltaY > 5");
		//	return false;
		//}
        Debug.Log("distanceToInteractionPoint " + distanceToInteractionPoint + " interactionDistanceThreshold " + interactionDistanceThreshold);

        return distanceToInteractionPoint < interactionDistanceThreshold;
	}

	public override void PerformAction(Action action)
	{
		if (action.preCondition)
		{
			SetActionPreCondition(action.preCondition);
		}
		actionToWalkTo = null;
		action.arbitration.ActionRequest(this);
	}

	void SetActionPreCondition(ActionPreCondition precondition)
	{
		var preConditionTransform = precondition.AvatarInteractTransform(this);

		if (!preConditionTransform)
		{
			return;
		}
		transform.position = preConditionTransform.position;
		transform.rotation = preConditionTransform.rotation;
	}


	public override void TryToPerformAction(Action action, Vector3 targetPosition, Quaternion targetRotation)
	{
		if (action.preCondition != null && !action.preCondition.AvatarCanWalkToAction(this))
		{
			Debug.Log("You can not walk to the Interactable, so I interact right away");
			PerformAction(action);
		}
		else
		{
			if (action.preCondition == null)
			{
				// walkToPoint.MoveToTarget(walkPosition, walkRotation);
				targetInteractPosition = targetPosition;
				targetInteractRotation = targetRotation;
			}
			else
			{
				var walkToTransform = action.preCondition.AvatarInteractTransform(this);
				targetInteractPosition = walkToTransform.position;
				targetInteractRotation = walkToTransform.rotation;
			}
			
			if (CloseEnoughToPerformAction())
			{
				PerformAction(action);
			}
			else
			{
				walkToPoint.MoveToTarget(targetInteractPosition, targetInteractRotation);
				actionToWalkTo = action;
			}
		}
	}
}
