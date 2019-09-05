using UnityEngine;
using System.Collections;

public enum ActorMood
{
	Neutral,
	Angry,
	Happy,
	Sad,
	Smirky,
	Coy
}

public class ActorLine : ActorSceneComponent
{
	public string actorName;
	public AudioClip actorClip;
	public ActorMood actorMood;
	public string talkingToActorName;

	public AnimationClip customAnimation;
	public bool blendCustomAnimation;
	public float customAnimationCrossfadeTime = 0.1f;
	public float delayAfter;
	
	public string subtitle;

	static string lastConversationWithNpc;
	static string lastSpeakerName;
	static string lastTalkingToActorName;

	SceneActor speakingActor;
	
	void SwitchedActor()
	{
	}

	public override bool CanBeInterrupted()
	{
		return true;
	}

	void SwitchedCamera()
	{
		iTween.Stop(gameObject);
	}

	void Close()
	{
		IAvatar avatar = actingInScene.GetMainAvatar();
		avatar.OnSubtitleStop();
		StopFacialAnimation(speakingActor);
		ComponentDone();
	}

	public override void Skip()
	{
		if (customAnimation != null)
		{
			speakingActor.GetComponent<Animation>()[customAnimation.name].normalizedTime = 1.0f;
		}
		Close();
	}

	public void Update()
	{
		if (!ShouldUpdate)
		{
			return;
		}
		if (actorClip != null && actingInScene.GetGameObject().GetComponent<AudioSource>().isPlaying)
		{
			return;
		}
		
		if (actorClip != null)
		{
			IAvatar avatar = actingInScene.GetMainAvatar();
			avatar.OnSubtitleStop();
		}
		if (customAnimation != null)
		{
			var currentAnimation = speakingActor.GetComponent<Animation>()[customAnimation.name];
			if (currentAnimation.normalizedTime < 1.0 && speakingActor.GetComponent<Animation>().IsPlaying(customAnimation.name))
			{
				return;
			}
		}
		Close();
	}

	public string TalkingToActorName
	{
		get
		{
			var currentTalkingToActorName = talkingToActorName;

			if (currentTalkingToActorName == null || currentTalkingToActorName == "")
			{
				if (actorName == "Tyra")
				{
					currentTalkingToActorName = lastConversationWithNpc;
				}
				else
				{
					currentTalkingToActorName = "Tyra";
				}
			}

			DebugUtilities.Assert(currentTalkingToActorName != null, "You tried to find a null talk to actor in line:" + name);

			return currentTalkingToActorName;
		}
	}


	float FramingAngleFromPositions(Vector3 source, Vector3 target, Color color, float debugRotation)
	{
		const float duration = 2.0f;
		var vectorToTarget = target - source;
		var length = vectorToTarget.magnitude;
		Debug.DrawRay(source, vectorToTarget, color, duration);

		var angleToTarget = Mathf.Atan2(-vectorToTarget.y, length) * Mathf.Rad2Deg;

		Debug.Log("vector:" + vectorToTarget + " angle:" + angleToTarget);

		var calculatedVector = Quaternion.Euler(angleToTarget, debugRotation, 0) * Vector3.forward * length;
		Debug.DrawRay(source, calculatedVector, color, duration);
		return angleToTarget;
	}

	float AngleFromPositions(Vector3 source, Vector3 target, Color color)
	{
		var vectorToTarget = target - source;
		// vectorToTarget.y = 0;
		// vectorToTarget.Normalize();
		var angleToTarget = Mathf.Atan2(vectorToTarget.x, vectorToTarget.z) * Mathf.Rad2Deg;
		//Debug.DrawRay(source, vectorToTarget, color, 10.0f);

		return angleToTarget;
	}

	protected override void Act()
	{
		DebugUtilities.Assert(actorName != null, "You tried to find a null speak actor in line:" + name);
		speakingActor = actingInScene.GetSceneActor(actorName);
		
		if (!speakingActor.IsInScene())
		{
			speakingActor.ActorSceneEnter();
		}
		
		var currentTalkingToActorName = TalkingToActorName;

		if (actorClip != null)
		{
			Debug.Log("Speaking line:" + name + " clip:" + actorClip.name + " speaker:" + actorName + " talkingTo:" + talkingToActorName);
			lastSpeakerName = actorName;
			lastTalkingToActorName = TalkingToActorName;
			actingInScene.GetGameObject().GetComponent<AudioSource>().clip = actorClip;
			actingInScene.GetGameObject().GetComponent<AudioSource>().Play();
			StartFacialAnimation(speakingActor);
			IAvatar avatar = actingInScene.GetMainAvatar();
			avatar.OnSubtitleStart(subtitle);
		}

		if (customAnimation != null)
		{
			PlayCustomAnimation(speakingActor, customAnimation);
		}
		if (actorName != "Tyra")
		{
			lastConversationWithNpc = actorName;
		}
		else if (talkingToActorName != "Tyra")
		{
			lastConversationWithNpc = currentTalkingToActorName;
		}
	}

	void PlayCustomAnimation(SceneActor actor, AnimationClip clip)
	{
		Debug.Log("PlayCustomAnimation:" + actor.name + " clip:" + clip.name);
		actor.PlayCustomAnimation(clip, blendCustomAnimation ? customAnimationCrossfadeTime : 0);
	}

	void LateUpdate()
	{
		if (!IsInScene())
		{
			return;
		}
	}


	void StartFacialAnimation(SceneActor actorToSpeak)
	{
		actorToSpeak.StartFacialAnimation();
	}

	void StopFacialAnimation(SceneActor actorToStopSpeaking)
	{
		actorToStopSpeaking.StopFacialAnimation();
	}


	void PlayRandomGesture(SceneActor actorToGesture)
	{
		actorToGesture.RandomGesture();
	}

	/*
	 * void StartIdleOnAllActors()
	 * {
	 *      foreach (var actorPair in actorsInScene)
	 *      {
	 *              var actor = actorPair.Value;
	 *              var sceneActor = actor.GetComponentInChildren<SceneActor>();
	 *
	 *              if (sceneActor != null) {
	 *                      sceneActor.StartIdleAnimation();
	 *              }
	 *      }
	 * }
	 *
	 * void StopIdleOnAllActors()
	 * {
	 *      foreach (var actorPair in actorsInScene)
	 *      {
	 *              var actor = actorPair.Value;
	 *              var sceneActor = actor.GetComponentInChildren<SceneActor>();
	 *
	 *              if (sceneActor != null) {
	 *                      sceneActor.ActorSceneExit();
	 *              }
	 *      }
	 * }
	 */
}




