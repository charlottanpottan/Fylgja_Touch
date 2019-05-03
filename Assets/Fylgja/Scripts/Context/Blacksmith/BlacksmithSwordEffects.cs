using UnityEngine;

public class BlacksmithSwordEffects : GenericEffect
{
	public GameObject sledgehammerMissed;
	public GameObject sledgehammerHit;
	public GameObject sledgehammerHitPerfect;
	public GameObject sledgehammerHitColdSword;
	public GameObject swordTurnedCold;
	private Transform effectTransform;
	public AnimationClip swordAnnealedAnimation;
	public AudioHandler scratchStartHandler;
	public AudioHandler scratchingHandler;
	public AudioHandler scratchStopHandler;
	public float perfectHitThreshold = 0.5f;

	void Start()
	{
		effectTransform = GameObject.Find("SmithPoint").transform;
		SetAnnealedAnimationTime(0);
	}

	void Update()
	{
	}

	void OnSledgeHammerMissedDent()
	{
		Debug.Log("Effects: SledgeHammerMissed");
		TriggerEffect(sledgehammerMissed);
	}

	void OnSledgeHammerHitDent(float perfectHitFactor)
	{
		if (perfectHitFactor > perfectHitThreshold)
		{
			TriggerEffect(sledgehammerHitPerfect);
		}
		else
		{
			TriggerEffect(sledgehammerHit);
		}
	}

	void OnSledgeHammerHitColdSword()
	{
		Debug.Log("Effects: SledgeHammerHitColdSword");
		TriggerEffect(sledgehammerHitColdSword);
	}

	void OnBlacksmithSwordCold()
	{
		Debug.Log("Effects: Blacksmith Sword is cold");
		TriggerEffect(swordTurnedCold);
	}

	void OnBlacksmithSwordAnnealedChanged(float normalizedAnnealedTemperature)
	{
		SetAnnealedAnimationTime(normalizedAnnealedTemperature);
	}

	void OnSwordChangedMoveDirection()
	{
		// Debug.Log("Effects: Switched direction");
	}

	void OnSwordMovementStarted()
	{
		Debug.Log("Effects: Sword movement started");
		scratchStartHandler.TriggerSound();
		scratchingHandler.TriggerSound();
	}

	void OnSwordMovementStopped()
	{
		Debug.Log("Effects: Sword movement stopped");
		scratchingHandler.StopSound();
		scratchStopHandler.TriggerSound();
	}

	void SetAnnealedAnimationTime(float t)
	{
		var anim = animation;

		AnimationState state = anim[swordAnnealedAnimation.name];

		state.normalizedTime = t;
		state.wrapMode = WrapMode.ClampForever;
		state.speed = 0;
		anim.CrossFade(swordAnnealedAnimation.name);
	}

	void TriggerEffect(GameObject o)
	{
		TriggerEffect(o, effectTransform);
	}
}
