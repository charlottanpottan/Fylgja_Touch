using UnityEngine;
using System.Collections;

public class CharacterIgnite : MonoBehaviour
{
	public IAvatar avatar;
	public AnimationClip ignite;
	public AnimationClip poking;
	public AnimationClip pokingToIdle;
	public VehicleMoveToPoint walkToPoint;
	public float ignitionTime = 4.0f;

	FirePit firePit;
	float ignitionEndTime;
	bool isIgniting;

	float goingBackToIdleDoneTime;
	bool goingBackToIdle;
	AllowedToMoveModifier dontMoveModifier;

	void Start()
	{
	}

	void Update()
	{
		if (isIgniting && Time.time >= ignitionEndTime)
		{
			OnIgniteDone();
		}
		if (goingBackToIdle && Time.time >= goingBackToIdleDoneTime)
		{
			OnBackToIdleDone();
		}
	}

	void OnEnable()
	{
	}

	void OnDisable()
	{
	}

	void OnFirePitIgnite(FirePit pit)
	{
		BroadcastMessage("OnFirePitIgnitionStart");
		if (dontMoveModifier == null)
		{
			dontMoveModifier = new AllowedToMoveModifier("igniting");
			avatar.AddAllowedToMoveModifier(dontMoveModifier);
		}
		GetComponent<Animation>().CrossFade(ignite.name);
		GetComponent<Animation>().PlayQueued(poking.name);
		ignitionEndTime = Time.time + ignitionTime;
		isIgniting = true;
		firePit = pit;
		pit.StartIgniting();
	}

	void OnIgniteDone()
	{
		GetComponent<Animation>().CrossFade(pokingToIdle.name);
		firePit.Ignited();
		isIgniting = false;
		firePit = null;
		goingBackToIdle = true;
		goingBackToIdleDoneTime = Time.time + GetComponent<Animation>()[pokingToIdle.name].length;
	}

	void Reset()
	{
		if (isIgniting)
		{
			firePit.AbortIgnite();
		}
		walkToPoint.enabled = true;
		goingBackToIdle = false;
		isIgniting = false;
		avatar.RemoveAllowedToMoveModifier(dontMoveModifier);
		dontMoveModifier = null;
	}

	void OnBackToIdleDone()
	{
		Reset();
		BroadcastMessage("OnFirePitIgnitionDone");
	}

	void OnMinigameAborted(Minigame game)
	{
		Reset();
	}
}
