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
		GetComponent<Animation>()[idle.name].layer = 0;
		GetComponent<Animation>()[duck.name].layer = 3;
		GetComponent<Animation>()[struckLeft.name].layer = 3;
		GetComponent<Animation>()[struckRight.name].layer = 3;
		GetComponent<Animation>()[hitLeft.name].layer = 3;
		GetComponent<Animation>()[hitRight.name].layer = 3;
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
		GetComponent<Animation>().Play(duck.name);
		//}
	}

	bool IsHitting()
	{
		return GetComponent<Animation>().IsPlaying(hitLeft.name) || GetComponent<Animation>().IsPlaying(hitRight.name);
	}

	bool IsDucking()
	{
		return GetComponent<Animation>().IsPlaying(duck.name);
	}

	bool IsHittingOrDucking()
	{
		return IsHitting() || IsDucking();
	}

    public void WantsToHitLeft()
	{
		// if (!IsHittingOrDucking()) {
		PunchLeft();
		// }
	}

    public void WantsToHitRight()
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
		GetComponent<Animation>().CrossFade(idle.name);
		
		GameObject go = GameObject.Instantiate(barObject) as GameObject;
		health = maxHealth;
		damageStar = 0;
		barAnim = go.GetComponent<Animation>();
		barAnim[barAnim.clip.name].normalizedSpeed = 0;
		barAnim[barAnim.clip.name].normalizedTime = 0;
		
		base.StickFightMinigameStart(minigame);
	}

	void OnStickFightMinigameClose()
	{
		Destroy(barAnim.gameObject);
		barAnim = null;
        walking.StopMoving();
		walking.BlendToLocomotion();
		SetToolsEnabled(false);
	}

	void SetToolsEnabled(bool enabled)
	{
		hasFightStick = enabled;
		foreach (var o in objectsToActivate)
		{
			o.SetActiveRecursively1(enabled);
		}
		foreach (var r in rootObjectsToActivate)
		{
			r.SetActive(enabled);
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
