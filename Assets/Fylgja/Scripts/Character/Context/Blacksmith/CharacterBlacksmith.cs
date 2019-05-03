using UnityEngine;
using System.Collections;

public class CharacterBlacksmith : MonoBehaviour
{
	public AnimationClip swordArmAnimation;
	public AnimationClip hitSledgehammer;
	public AnimationClip idleAnimation;
	public BlacksmithSword swordPrefab;
	public BlacksmithSledgehammer sledgePrefab;
	public Transform swordTransformInHand;
	public Transform sledgeTransformInHand;
	public float mouseSensitivity = 0.001f;
	public ActorScene[] swordDoneScenes;
	public ActorScene pickupSwordScene;
	public IAvatar characterAvatar;

	int finishedSwordCount;
	float swordHandPosition;
	bool isSmithing;
	bool isInSledgehammerMode;
	bool punchIsPossible;
	float currentSwordDirection;
	bool isHittingWithHammer;
	bool waitingForSwordStop;
	float timeForSwordStop;
	bool swordIsMoving;

	BlacksmithSword sword;
	BlacksmithSledgehammer sledge;
	IBlacksmithSwordNotifications swordNotifications;

	bool pickingUpNewSword;
	bool reachingForNewSword;

	void Start()
	{
	}

	void Update()
	{
		if (!isInSledgehammerMode || !isSmithing)
		{
			return;
		}
		UpdateSwordHand();
		UpdateSledgehammerMovement();
		CheckIfPunchDone();
	}

	void CheckIfPunchDone()
	{
		if (isHittingWithHammer && !GetComponent<Animation>().IsPlaying(hitSledgehammer.name))
		{
			OnPunchIsDone();
		}
	}

	void OnPunchIsDone()
	{
		isHittingWithHammer = false;
		isInSledgehammerMode = false;
	}

	public void SetSwordNotifications(IBlacksmithSwordNotifications notifications)
	{
		swordNotifications = notifications;
	}

	void UpdateSledgehammerMovement()
	{
		if (!punchIsPossible || !isInSledgehammerMode)
		{
			return;
		}
		bool wantsToHitWithHammer = Input.GetButtonUp("interact");

		if (wantsToHitWithHammer)
		{
			StopSwordMovement();
			sledge.canHit = true;
			GetComponent<Animation>().Play(hitSledgehammer.name);
			punchIsPossible = false;
			isHittingWithHammer = true;
		}
	}

	public bool IsAimingSledgehammer()
	{
		return isInSledgehammerMode;
	}

	public void OnSledgehammerHitSword(bool swordIsDone)
	{
		if (swordIsDone)
		{
			SwordDone();
			finishedSwordCount++;
		}
		//punchIsPossible = true;
	}

	void PickupSword()
	{
		Debug.Log("Picking up a new sword");
		var scene = ActorSceneUtility.CreateSceneWithAvatar(pickupSwordScene.gameObject, characterAvatar);
		scene.endOfSceneNotification += OnPickupSwordSceneEnded;
//		swordNotifications.PauseTimer();
		if(sword != null)
		{
			sword.IsPaused(true);
		}
		scene.PlayScene(characterAvatar.playerNotifications);
	}

	void OnPickupSwordSceneEnded(ActorScene scene)
	{
		Debug.Log("Pickup Sword scene ended!");
		scene.endOfSceneNotification -= OnPickupSwordSceneEnded;
//		swordNotifications.ResumeTimer();
		if(sword != null)
		{
			sword.IsPaused(false);
		}
	}

	void SwordDone()
	{
		Debug.Log("Sword reported as done");
		var scene = ActorSceneUtility.CreateSceneWithAvatarAndInteractable(swordDoneScenes[finishedSwordCount].gameObject, characterAvatar, sword.gameObject);
		scene.endOfSceneNotification += OnSwordDoneSceneEnded;
//		swordNotifications.PauseTimer();
		if(sword != null)
		{
			sword.IsPaused(true);
		}
		scene.PlayScene(characterAvatar.playerNotifications);
	}

	void OnSwordDoneSceneEnded(ActorScene scene)
	{
//		swordNotifications.ResumeTimer();
		Debug.Log("OnSwordDoneScene ended...");
		scene.endOfSceneNotification -= OnSwordDoneSceneEnded;
		var gameIsComplete = swordNotifications.OnSwordDone(sword);
		if (!gameIsComplete)
		{
			PickupSword();
		}
	}

	public void StartAimingSledgehammer()
	{
		isInSledgehammerMode = true;
		punchIsPossible = true;
	}

	public void StopAimingSledgehammer()
	{
		sledge.canHit = false;
		isInSledgehammerMode = false;
		punchIsPossible = false;
	}


	void BlacksmithAnnealSword()
	{
		sword.AnnealSword();
	}

	void BlacksmithSpawnNewSword()
	{
		UnspawnSword();
		SpawnSword();
	}

	void UpdateSwordHand()
	{
		float deltaMovement = Input.GetAxis("camera_horizontal") * mouseSensitivity * Time.deltaTime;
		bool wantsToMoveSword = Mathf.Abs(deltaMovement) > 0.001f;

		if (wantsToMoveSword)
		{
			if (punchIsPossible)
			{
				waitingForSwordStop = false;
				if (!swordIsMoving)
				{
					sword.BroadcastMessage("OnSwordMovementStarted");
					swordIsMoving = true;
				}
				float newSwordDirection = Mathf.Sign(deltaMovement);
				if (newSwordDirection != currentSwordDirection)
				{
					sword.BroadcastMessage("OnSwordChangedMoveDirection");
					currentSwordDirection = newSwordDirection;
				}
				swordHandPosition += deltaMovement;
				swordHandPosition = Mathf.Clamp(swordHandPosition, 0, 1);
				SetSwordHandPosition(swordHandPosition);
			}
		}
		else if (swordIsMoving)
		{
			if (waitingForSwordStop)
			{
				if (Time.time >= timeForSwordStop)
				{
					StopSwordMovement();
				}
			}
			else
			{
				timeForSwordStop = Time.time + 0.3f;
				waitingForSwordStop = true;
			}
		}
	}

	void StartIdleAnimation()
	{
		Animation anim = GetComponent<Animation>();
		anim.CrossFade(idleAnimation.name);
		AnimationState state = anim[idleAnimation.name];
		state.layer = -2;
	}

	void StopIdleAnimation()
	{
		GetComponent<Animation>().Stop(idleAnimation.name);
	}

	void StopSwordMovement()
	{
		sword.BroadcastMessage("OnSwordMovementStopped");
		currentSwordDirection = 0;
		swordIsMoving = false;
		waitingForSwordStop = false;
	}

	void SetSwordHandPosition(float percentage)
	{
		Animation anim = GetComponent<Animation>();

		AnimationState state = anim[swordArmAnimation.name];

		state.layer = 1;
		state.normalizedTime = percentage;
		state.speed = 0;
		state.wrapMode = WrapMode.ClampForever;
		GetComponent<Animation>().CrossFade(swordArmAnimation.name);
	}


	GameObject SpawnTool(GameObject prefab, Transform parentTransform)
	{
		var tool = Instantiate(prefab) as GameObject;
		tool.transform.parent = parentTransform;
		tool.transform.localPosition = new Vector3();
		tool.transform.localRotation = new Quaternion();

		return tool;
	}

	void SpawnSword()
	{
		Debug.Log("Spawning sword!");
		DebugUtilities.Assert(sword == null, "You already have a sword, can not spawn a new one");
		sword = SpawnTool(swordPrefab.gameObject, swordTransformInHand).GetComponentInChildren<BlacksmithSword>();
		SetSwordHandPosition(swordHandPosition);
	}

	void SpawnSledge()
	{
		Debug.Log("Spawning sledgehammer!");
		DebugUtilities.Assert(sledge == null, "You already have a sword, can not spawn a new one");
		sledge = SpawnTool(sledgePrefab.gameObject, sledgeTransformInHand).GetComponentInChildren<BlacksmithSledgehammer>();
		sledge.smither = this;
	}

	void UnspawnSledge()
	{
		Debug.Log("UNspawning sledgehammer!");
		Destroy(sledge.gameObject);
		sledge = null;
	}

	void UnspawnSword()
	{
		if (sword == null)
		{
			return;
		}
		Debug.Log("UNspawning sword!");
		Destroy(sword.gameObject);
		sword = null;
	}

	void CloseMinigame()
	{
		isSmithing = false;
		GetComponent<Animation>().Stop(swordArmAnimation.name);
		GetComponent<Animation>().Stop(hitSledgehammer.name);
		StopIdleAnimation();
		SetActivateBlacksmithTools(false);
	}

	public bool IsSmithing()
	{
		return isSmithing;
	}

	void SetActivateBlacksmithTools(bool on)
	{
		if (on)
		{
			// SpawnSword();
			SpawnSledge();
		}
		else
		{
			UnspawnSword();
			UnspawnSledge();
		}
	}

	void OnBlacksmithMinigameStart()
	{
		isSmithing = true;
		SetSwordHandPosition(0.0f);
		SetActivateBlacksmithTools(true);
		StartIdleAnimation();
		finishedSwordCount = 0;
		PickupSword();
	}

	void OnBlacksmithMinigameFailed()
	{
		CloseMinigame();
	}

	public void OnBlacksmithMinigameAborted()
	{
		CloseMinigame();
	}

	public void OnBlacksmithMinigameDone()
	{
		CloseMinigame();
	}
}
