using UnityEngine;
using System.Collections;

public class StickFighterEffects : MonoBehaviour
{
	public GameObject[] stars;
	public GameObject[] enableObjectsOnFirstStar;
	public GameObject receivedHitLeft;
	public GameObject receivedHitRight;
	public GameObject receivedHitCombination;
	public GameObject[] enableObjectsOnRightPunch;
	public GameObject[] enableObjectsOnLeftPunch;

	public GameObject leftStickEnd;
	public GameObject rightStickEnd;

	public float leftEnableStickAtTime;
	public float leftDisableStickAtTime;

	public float rightEnableStickAtTime;
	public float rightDisableStickAtTime;


	public GameObject blockLeft;
	public GameObject blockRight;
	public AudioHandler targetAudioHandler;
	private AudioSource targetAudioSource;

	float leftEnableAtTime = -1;
	float leftDisableAtTime = -1;

	float rightEnableAtTime = -1;
	float rightDisableAtTime = -1;

	public Transform effectTransform;

	void Awake()
	{
		targetAudioSource = targetAudioHandler.gameObject.GetComponent<AudioSource>();
	}

	void Start()
	{
	}

	void Update()
	{
		if (leftEnableAtTime > 0 && Time.time >= leftEnableAtTime)
		{
			EnableStickEnd(leftStickEnd, true);
			leftEnableAtTime = -1;
		}
		if (rightEnableAtTime > 0 && Time.time >= rightEnableAtTime)
		{
			EnableStickEnd(rightStickEnd, true);
			rightEnableAtTime = -1;
		}
		if (leftDisableAtTime > 0 && Time.time >= leftDisableAtTime)
		{
			EnableStickEnd(leftStickEnd, false);
			leftDisableAtTime = -1;
		}
		if (rightDisableAtTime > 0 && Time.time >= rightDisableAtTime)
		{
			EnableStickEnd(rightStickEnd, false);
			rightDisableAtTime = -1;
		}
	}

	void EnableStickEnd(GameObject stickEnd, bool enable)
	{
		Debug.Log("FX: Setting stick end:" + stickEnd.name + " to " + enable);
		stickEnd.SetActiveRecursively(enable);
	}

	void TriggerEffect(GameObject o, bool hitLanded, bool lastHit)
	{
		Debug.Log("Effect: " + o.name);
		Instantiate(o, effectTransform.position, effectTransform.rotation);
		if (!targetAudioSource.isPlaying && hitLanded && !lastHit)
		{
			targetAudioHandler.TriggerSound();
		}
	}

	void OnStickHitLandedLeft(float health)
	{
		TriggerEffect(receivedHitLeft, true, health == 0);
	}

	void OnStickHitLandedRight(float health)
	{
		TriggerEffect(receivedHitRight, true, health == 0);
	}

	void OnStickHitLandedCombination(float health)
	{
		TriggerEffect(receivedHitCombination, true, health == 0);
	}

	void OnStickHealthStarsChanged(int numberOfStars)
	{
		for (int i = 0; i < stars.Length; ++i)
		{
			bool enableStar = (numberOfStars > i);
			if (i == 0 && enableStar)
			{
				foreach (var otherObject in enableObjectsOnFirstStar)
				{
					otherObject.SetActiveRecursively(true);
				}
			}
			stars[i].SetActiveRecursively(enableStar);
		}
	}

	void SetEnableObjects(GameObject[] objects, bool enabled)
	{
		foreach (var o in objects)
		{
			o.SetActiveRecursively(enabled);
		}
	}

	void OnStickPunchLeftStart()
	{
		SetEnableObjects(enableObjectsOnLeftPunch, true);
	}

	void OnStickPunchRightStart()
	{
		SetEnableObjects(enableObjectsOnRightPunch, true);
	}

	void OnStickPunchLeftDone()
	{
		SetEnableObjects(enableObjectsOnLeftPunch, false);
	}

	void OnStickPunchRightDone()
	{
		SetEnableObjects(enableObjectsOnRightPunch, false);
	}

	void OnStickCombinationPunchStart()
	{
		Debug.Log("fx: Combination start");
		var currentTime = Time.time;

		leftEnableAtTime = currentTime + leftEnableStickAtTime;
		leftDisableAtTime = currentTime + leftDisableStickAtTime;

		rightEnableAtTime = currentTime + rightEnableStickAtTime;
		rightDisableAtTime = currentTime + rightDisableStickAtTime;
	}

	void OnStickCombinationPunchDone()
	{
		Debug.Log("fx: Combination done");
	}

	void OnStickBlockLeft()
	{
		Debug.Log("fx: Block left");
		TriggerEffect(blockLeft, false, false);
	}

	void OnStickBlockRight()
	{
		Debug.Log("fx: Block right");
		TriggerEffect(blockRight, false, false);
	}

	void OnStickFightMinigameClose()
	{
		SetEnableObjects(enableObjectsOnFirstStar, false);
	}
}
