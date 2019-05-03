using UnityEngine;
using System.Collections;

public class CharacterFan : MonoBehaviour
{
	public AnimationClip fanUp;
	public AnimationClip fanDown;
	public AnimationClip birchUp;
	public AnimationClip birchDown;
	public Animation birchAnimation;
	public AnimationClip firePitDone;

	public AnimationClip fanStart;
	public AnimationClip fanStop;
	
	public GameObject barObject;

	public AudioHandler firePitDoneDialog;

	public int perfectFanAmount = 10;
	public int mediumFanAmount = 4;

	public CharacterAvatar walking;

	bool isFanning;
	bool isInFanMode = false;
	bool fanDirectionIsUp = false;

	AnimationState fanAnimationState;

	bool waitForFirePitDone;
	public float lessThanFailThreshold = 0.5f;
	public float moreThanFailThreshold = 0.99f;
	public float moreThanPerfectThreshold = 0.88f;
	public float fanAnimationSpeed = 1.0f;
	public float crossfadeFanTime = 0.01f;

	bool waitingForFanReady;
	float fanReadyTime;

	bool waitingForFanReachedEnd;
	
	Animation barAnim;

	float waitingForPitDoneTime;
	bool waitingForPitDone;

	FirePit lastUsedPit;

	AllowedToMoveModifier dontMoveModifier;

	void Start()
	{
	}

	void Update()
	{
		if (waitForFirePitDone)
		{
			Debug.Log("Waiting for animation....");
			if (!animation.IsPlaying(firePitDone.name))
			{
				OnFirePitMinigameDoneAnimationReady();
			}
		}
		else if (waitingForFanReady && Time.time >= fanReadyTime)
		{
			OnFanReady();
		}
		if (isInFanMode && waitingForFanReachedEnd)
		{
			float fanTime = Mathf.Clamp(fanAnimationState.normalizedTime, 0, 1.0f);
			if (fanTime == 1.0f)
			{
				//OnFanReachedEnd();
			}
			if(fanDirectionIsUp)
				barAnim[barAnim.clip.name].normalizedTime = fanTime;
			else
				barAnim[barAnim.clip.name].normalizedTime = 1-fanTime;
		}

		if (waitingForPitDone && Time.time >= waitingForPitDoneTime)
		{
			OnFirePitDoneAnimationReady();
		}
	}

	void OnEnable()
	{
	}

	void OnDisable()
	{
	}

	
	void OnFirePitIgnitionStart()
	{
		if (isInFanMode)
		{
			StopFanningMode();
		}
	}
	
	void OnFirePitIgnitionDone()
	{
		StartFanMode();
	}

	void StartFanMode()
	{
		Debug.Log("CrossFade: StartFanMode!");
		if (dontMoveModifier == null)
		{
			dontMoveModifier = new AllowedToMoveModifier("fanning");
			walking.AddAllowedToMoveModifier(dontMoveModifier);
		}
		GameObject go = GameObject.Instantiate(barObject) as GameObject;
		barAnim = go.animation;
		barAnim[barAnim.clip.name].normalizedSpeed = 0;
		barAnim[barAnim.clip.name].normalizedTime = 1;
		animation.CrossFade(fanStart.name);
		waitingForFanReady = true;
		fanReadyTime = Time.time + fanStart.length;
		isFanning = false;
	}

	void OnFirePitDoneAnimationReady()
	{
		waitingForPitDone = false;
		Reset();
	}

	bool IsWaitingForFan()
	{
		return waitingForFanReady;
	}


	AnimationState StartAnimationAt(Animation anim, AnimationClip clip, float time)
	{
		AnimationState state = anim[clip.name];

		state.normalizedTime = time;
		state.wrapMode = WrapMode.ClampForever;
		state.speed = fanAnimationSpeed;
		anim.CrossFade(clip.name);

		return state;
	}

	void StartAnimationAt(AnimationClip arm, AnimationClip birch, float time)
	{
		fanAnimationState = StartAnimationAt(animation, arm, time);
		StartAnimationAt(birchAnimation, birch, time);
	}

	void OnFanReady()
	{
		fanDirectionIsUp = true;
		StartAnimationAt(fanUp, birchUp, 0.97f);
		waitingForFanReady = false;
		isInFanMode = true;
	}

	void OnFanReachedEnd()
	{
		lastUsedPit.Fan(0);
		waitingForFanReachedEnd = false;
		lastUsedPit = null;
		isFanning = false;
	}


	void OnFanRequested(FirePit pit)
	{
		if (!isInFanMode)
		{
			if (!IsWaitingForFan())
			{
				StartFanMode();
			}
			return;
		}

		float fanTime = Mathf.Clamp(fanAnimationState.normalizedTime, 0, 1.0f);
		int fanPower = 0;
		if (isFanning)
		{
			if (fanTime < lessThanFailThreshold)
			{
				return;
			}
//			else if (fanTime >= moreThanFailThreshold)
//			{
//				isFanning = false;
//			}
			else
			{
				if (fanTime > moreThanPerfectThreshold)
				{
					fanPower = perfectFanAmount;
				}
				else
				{
					fanPower = mediumFanAmount;
				} fanDirectionIsUp = !fanDirectionIsUp;
			}
		}

		if (!isFanning)
		{
			if (fanTime == 1.0f)
			{
				fanDirectionIsUp = !fanDirectionIsUp;
				isFanning = true;
			}
			else
			{
				return;
			}
		}

		if (fanDirectionIsUp)
		{
			StartAnimationAt(fanUp, birchUp, 1.0f - fanTime);
		}
		else
		{
			StartAnimationAt(fanDown, birchDown, 1.0f - fanTime);
		} waitingForFanReachedEnd = true;
		lastUsedPit = pit;
		if (fanPower != 0)
		{
			pit.Fan(fanPower);
		}
	}

	void OnVehicleWantsToMove()
	{
		if (isInFanMode)
		{
			Debug.Log("Fanning aborted. CrossFade: FanStop!");
			animation.CrossFade(fanStop.name, 0.1f);
			Reset();
		}
	}

	void OnFirePitFullFlame()
	{
		Debug.Log("Effect: full flame! YESS!!!");
		firePitDoneDialog.TriggerSound();
		animation.CrossFade(firePitDone.name);
		waitingForFanReachedEnd = false;
		lastUsedPit = null;
 		waitingForPitDone = true;
		waitingForPitDoneTime = Time.time + firePitDone.length;
	}

	void OnFirePitMinigameDone()
	{
		Debug.Log("WAITING FOR END ANIMATION!");
		waitForFirePitDone = true;
	}

	void OnFirePitMinigameDoneAnimationReady()
	{
		Debug.Log("WE SHOULD END MINIGAME NOW!");
		Reset();
		BroadcastMessage("OnFirePitMinigameComplete");
		waitForFirePitDone = false;
	}
	
	
	void StopFanningMode()
	{
		Debug.Log("Fanning: Stop");
		DebugUtilities.Assert(isInFanMode, "You can not reset if you aren't fanning");
		if(barAnim != null)
		{
			Destroy(barAnim.gameObject);
			barAnim = null;
		}

		isInFanMode = false;
	}

	void Reset()
	{
		Debug.Log("Fanning: Reset");
		if (isInFanMode)
		{
			StopFanningMode();
		}
		walking.RemoveAllowedToMoveModifier(dontMoveModifier);
		dontMoveModifier = null;
	}

	void OnMinigameAborted(Minigame game)
	{
		Reset();
	}
}
