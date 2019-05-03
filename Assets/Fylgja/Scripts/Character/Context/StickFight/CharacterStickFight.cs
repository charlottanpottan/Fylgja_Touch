using UnityEngine;

public class CharacterStickFight : StickFighter
{
	public AnimationClip duck;
	public AnimationClip idle;
	public CharacterWalking walking;
	public GameObject[] objectsToActivate;
	public GameObject[] rootObjectsToActivate;
	public AudioHandler stickFightDuckAudioHandler;
	public float duckCooldown = 0.5f;
	private float duckTimer;

	bool hasFightStick;

	void Start()
	{
		animation[idle.name].layer = 0;
		animation[duck.name].layer = 3;
		animation[struckLeft.name].layer = 3;
		animation[struckRight.name].layer = 3;
		animation[hitLeft.name].layer = 3;
		animation[hitRight.name].layer = 3;
	}

	void Update()
	{
		if (hasFightStick)
		{
			CheckInput();
		}
		FighterUpdate();
	}

	void CheckInput()
	{
		StickFighterState state = GetState();

		if (state != StickFighterState.Unknown)
		{
			return;
		}
		bool wantsToDuck = Input.GetButtonDown("interact");
		bool wantsToHitLeft = Input.GetButton("left");
		bool wantsToHitRight = Input.GetButton("right");

		if (wantsToHitLeft)
		{
			WantsToHitLeft();
		}
		else if (wantsToHitRight)
		{
			WantsToHitRight();
		}
		else if (wantsToDuck && Time.time >= duckTimer)
		{
			WantsToDuck();
		}
	}

	void WantsToDuck()
	{
		duckTimer = Time.time + duck.length + duckCooldown;
		stickFightDuckAudioHandler.TriggerSound();
		//if (!IsHittingOrDucking()) {
		animation.Play(duck.name);
		//}
	}

	bool IsHitting()
	{
		return animation.IsPlaying(hitLeft.name) || animation.IsPlaying(hitRight.name);
	}

	bool IsDucking()
	{
		return animation.IsPlaying(duck.name);
	}

	bool IsHittingOrDucking()
	{
		return IsHitting() || IsDucking();
	}

	void WantsToHitLeft()
	{
		// if (!IsHittingOrDucking()) {
		PunchLeft();
		// }
	}

	void WantsToHitRight()
	{
		//if (!IsHittingOrDucking()) {
		PunchRight();
		//}
	}

	public override void OnReceivedHitFromLeft()
	{
		Debug.Log("Tyra: Received Hit From Left");
		if (IsDucking())
		{
			return;
		}
		ReceivedHitLeft();
	}

	public override void OnReceivedHitFromRight()
	{
		Debug.Log("Tyra: Received Hit From Right");
		if (IsDucking())
		{
			return;
		}
		ReceivedHitRight();
	}

	void OnStickFightMinigameStart(StickFightMinigame minigame)
	{
		SetToolsEnabled(true);
		animation.CrossFade(idle.name);
		
		GameObject go = GameObject.Instantiate(barObject) as GameObject;
		health = maxHealth;
		damageStar = 0;
		barAnim = go.animation;
		barAnim[barAnim.clip.name].normalizedSpeed = 0;
		barAnim[barAnim.clip.name].normalizedTime = 0;
		
		base.StickFightMinigameStart(minigame);
	}

	void OnStickFightMinigameClose()
	{
		Destroy(barAnim.gameObject);
		barAnim = null;
		walking.BlendToLocomotion();
		SetToolsEnabled(false);
	}

	void SetToolsEnabled(bool enabled)
	{
		hasFightStick = enabled;
		foreach (var o in objectsToActivate)
		{
			o.SetActiveRecursively(enabled);
		}
		foreach (var r in rootObjectsToActivate)
		{
			r.active = enabled;
		}
	}

	void OnStickFightMinigameEnd()
	{
		CloseStickFight();
	}

	void OnStickFightMinigameQuit()
	{
		CloseStickFight();
	}

	void CloseStickFight()
	{
		SetToolsEnabled(false);
	}

	public bool HasFightStick()
	{
		return hasFightStick;
	}

	public override void OnLostAllHealth()
	{
		stickFightGame.OnCharacterLost();
	}
}
