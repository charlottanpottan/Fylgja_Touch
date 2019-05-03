using UnityEngine;
using System.Collections;

public class SceneActor : MonoBehaviour
{
	public Transform faceTransform;
	public Transform leftShoulderTransform;
	public Transform rightShoulderTransform;
	public float faceRadius;

	public AnimationClip mouthMovement;
	public AnimationClip randomGesture;
	public AnimationClip idleAnimation;

	string customAnimationName;

	bool isInScene;

	void Awake()
	{
		if (mouthMovement)
		{
			animation[mouthMovement.name].layer = 3;
		}
		if (randomGesture)
		{
			animation[randomGesture.name].layer = 2;
		}
		if (idleAnimation)
		{
			animation[idleAnimation.name].layer = 1;
		}
	}

	public Transform ShoulderTransform()
	{
		return leftShoulderTransform;
	}

	public Vector3 FacePosition()
	{
		return faceTransform.position;
	}

	public void PlayCustomAnimation(AnimationClip clip, float customAnimationCrossfadeTime)
	{
		StopIdleAnimation();
		animation[clip.name].layer = 2;
		customAnimationName = clip.name;
		if (customAnimationCrossfadeTime > 0)
		{
			animation.CrossFade(clip.name, customAnimationCrossfadeTime);
		}
		else
		{
			animation.Play(clip.name);
		}
	}

	public void ActorSceneEnter()
	{
		isInScene = true;
		var avatar = GetComponentInChildren<CharacterAvatar>();
		if (avatar != null)
		{
			avatar.TurnOffLocomotion();
		}
		StartIdleAnimation();

	}

	public void ActorSceneExit()
	{
		isInScene = false;
		if (customAnimationName != null)
		{
			animation.Stop(customAnimationName);
			customAnimationName = null;
		}
		StopIdleAnimation();
		var avatar = GetComponentInChildren<CharacterAvatar>();
		if (avatar != null)
		{
			avatar.BlendToLocomotion();
		}
	}

	public bool IsInScene()
	{
		return isInScene;
	}

	public void StartIdleAnimation()
	{
		if (customAnimationName != null)
		{
			Debug.Log("Ignoring request for idle animation since we are playing a custom animation");
			return;
		}
		Debug.Log("IDLE ANIMATION:" + idleAnimation.name + " for:" + name);
		animation.CrossFade(idleAnimation.name);
	}

	public void StopIdleAnimation()
	{
		Debug.Log("STOP IDLE ANIMATION:" + name);
		animation.Stop(idleAnimation.name);
	}

	public void StartFacialAnimation()
	{
		if(mouthMovement != null)
			animation.CrossFade(mouthMovement.name);
	}

	public void StopFacialAnimation()
	{
		if(mouthMovement != null)
			animation.Stop(mouthMovement.name);
	}

	public void RandomGesture()
	{
		if (randomGesture == null)
		{
			return;
		}
		animation.CrossFade(randomGesture.name);
	}
}
