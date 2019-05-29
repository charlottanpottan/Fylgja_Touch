using UnityEngine;
using System.Collections;

public enum ShotComposition
{
	CloseUp,
	MediumShot,
	LongShot,
	UsePrevious,
}

public enum ShotMovement
{
	None,
	DollyIn,
	DollyOut,
	DollyUp,
	Stop,
}

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

	public ShotComposition shotComposition;
	public ShotMovement shotMovement;
	public AnimationClip customAnimation;
	public bool blendCustomAnimation;
	public float customAnimationCrossfadeTime = 0.1f;
	public float delayAfter;
	
	public string subtitle;

	static string lastConversationWithNpc;
	static string lastSpeakerName;
	static string lastTalkingToActorName;
	
	bool setupCameraAngle;
	
	LogicCameraInfo preparedCameraInfo = new LogicCameraInfo();

	SceneActor speakingActor;
	bool switchedActorAndCamera;
	
	void SetCameraMovement()
	{
		switch (shotMovement)
		{
		case ShotMovement.DollyIn:
			iTween.MoveTo(gameObject, actingInScene.GetGameObject().transform.position + actingInScene.GetGameObject().transform.TransformDirection(new Vector3(0, 0, 0.7f)), 20.0f);
			break;

		case ShotMovement.DollyUp:
			iTween.MoveFrom(gameObject, actingInScene.GetGameObject().transform.position - actingInScene.GetGameObject().transform.TransformDirection(new Vector3(0, 0.5f, 0)), 10.0f);
			break;

		case ShotMovement.Stop:
			iTween.Stop(gameObject);
			break;
		}
	}

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
		SetupCameraSwitch();
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
	
	void CalculateFramingCamera(Vector3 cameraPosition, Vector3 upper, Vector3 lower, out float angle, float rotationAroundY)
	{
		const float duration = 2.0f;
		var upperAngle = FramingAngleFromPositions(cameraPosition, upper, new Color(1.0f, 0, 0.0f, 1.0f), rotationAroundY);
		var lowerAngle = FramingAngleFromPositions(cameraPosition, lower, new Color(1.0f, 1.0f, 0.0f, 1.0f), rotationAroundY);

		angle = Angle.AngleMiddle(upperAngle, lowerAngle);
		Debug.Log("lowerAngle:" + lowerAngle + " upperAngle:" + upperAngle + " middle:" + angle);
		var middleRay = Quaternion.Euler(angle, rotationAroundY, 0) * Vector3.forward * (upper - cameraPosition).magnitude;
		Debug.DrawRay(cameraPosition, middleRay, new Color(0.0f, 0.0f, 1.0f, 1.0f), duration);
	}

	void CalculateCinematicCamera(Vector3 cameraPosition, Vector3 target, Vector3 source, out float angle, out float fov)
	{
		var angleToSource = AngleFromPositions(cameraPosition, source, new Color(0.0f, 1.0f, 0, 1.0f));
		var angleToTarget = AngleFromPositions(cameraPosition, target, new Color(0.0f, 1.0f, 0.5f, 1.0f));
		fov = Angle.AngleDiff(angleToSource, angleToTarget) + 10.0f;
		angle = Angle.AngleMiddle(angleToSource, angleToTarget);

		var middleRay = Quaternion.Euler(0, angle, 0) * Vector3.forward * (source - cameraPosition).magnitude;
		// Debug.DrawRay(cameraPosition, middleRay, new Color(0.0f, 1.0f, 1.0f, 1.0f), 10.0f);
	}

	float CalculateDistanceAdjustmentFromFovDifference(float calculatedFov, float currentFov)
	{
		float calculatedFovRadians = calculatedFov * Mathf.Deg2Rad;
		float currentFovRadians = currentFov * Mathf.Deg2Rad;

		float diff = currentFovRadians - calculatedFovRadians;

		var magicFactor = 3.0f;
		var optimalDistance = (2.0f * Mathf.Tan(diff / 2.0f)) * magicFactor;

		return optimalDistance;
	}
	
	public void UpdateCamera(ref LogicCameraInfo cameraInfo)
	{
		if (switchedActorAndCamera)
		{
			SwitchedActor();
			SwitchedCamera();
			switchedActorAndCamera = false;
		}
		preparedCameraInfo.useSourcePosition = true;
		cameraInfo = preparedCameraInfo;
	}
	
	void SetupCameraSwitch()
	{
		var currentTalkingToActorName = TalkingToActorName;
		var talkingTo = actingInScene.GetSceneActor(currentTalkingToActorName);

		preparedCameraInfo.fov = 30;

		if (currentTalkingToActorName == actorName)
		{
			Debug.Log("Talking to self:" + talkingTo.name);
			LookAtActor(speakingActor, out preparedCameraInfo.sourcePosition, out preparedCameraInfo.targetPosition);
		}
		else
		{
			Debug.Log("Talking actor to actor:" + speakingActor.name);
			LookAtActorFromActor(speakingActor, talkingTo, currentTalkingToActorName == "Tyra" ? 1 : -1, out preparedCameraInfo.sourcePosition, out preparedCameraInfo.targetPosition);
		}
	} 

	protected override void Act()
	{
		switchedActorAndCamera = true; //(lastSpeakerName != null && (actorName != lastSpeakerName || TalkingToActorName != lastTalkingToActorName));
		if (switchedActorAndCamera)
		{
			Debug.Log("SWITCHED CAMERA!");
		}
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
		
		setupCameraAngle = true;
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

		if (setupCameraAngle)
		{
			SetupNewCameraAngle();
			setupCameraAngle = false;
		}
	}

	void SetupNewCameraAngle()
	{
		if (shotComposition != ShotComposition.UsePrevious)
		{
			const float faceRadius = 0.25f;
			SetShotComposition(speakingActor, faceRadius /*speakingActor.faceRadius*/, shotComposition);
		}

		if (shotMovement != ShotMovement.None)
		{
			SetCameraMovement();
		}
	}

	void LookAtActor(SceneActor talkingTo, out Vector3 sourcePosition, out Vector3 targetPosition)
	{
		var cameraPosition = talkingTo.FacePosition() + (talkingTo.transform.rotation * new Vector3(0.0f, 0.0f, 1.5f));
		var upper = talkingTo.FacePosition() + new Vector3(0.0f, 0.5f, 0.0f);
		var lower = talkingTo.FacePosition() + new Vector3(0.0f, -1.1f, 0.0f);
		var framingAngle = 0.0f;

		var angle = talkingTo.transform.eulerAngles.y + 180.0f;
		CalculateFramingCamera(cameraPosition, upper, lower, out framingAngle, angle);
		sourcePosition = cameraPosition;
		targetPosition = cameraPosition + Quaternion.Euler(framingAngle, angle, 0) * Vector3.forward;
	}

	void LookAtActorFromActor(SceneActor toActor, SceneActor shoulderActor, float factor, out Vector3 sourcePosition, out Vector3 targetPosition)
	{
		Vector3 shoulderPosition;
		float shoulderFactor;

		if (TalkingToActorName == "Tyra")
		{
			shoulderPosition = shoulderActor.leftShoulderTransform.position;
			shoulderFactor = -1.0f;
		}
		else
		{
			shoulderPosition = shoulderActor.rightShoulderTransform.position;
			shoulderFactor = 1.0f;
		}
		
		var deltaPositionFromShouldToFace = toActor.FacePosition() - shoulderPosition;
		var actorToActorRotation = Quaternion.LookRotation(deltaPositionFromShouldToFace);
		var cameraPosition = shoulderPosition + actorToActorRotation * new Vector3(shoulderFactor * 0.8f, 0.2f, -1.5f);
		
		var angle = 0.0f;
		var calculatedFov = 0.0f;
		CalculateCinematicCamera(cameraPosition, shoulderActor.FacePosition(), toActor.FacePosition(), out angle, out calculatedFov);

		var distanceAdjustment = -CalculateDistanceAdjustmentFromFovDifference(calculatedFov, preparedCameraInfo.fov);
		Debug.Log("DistanceAdjustment:" + distanceAdjustment);

		var framingAngle = 0.0f;
		CalculateFramingCamera(cameraPosition, toActor.FacePosition() + new Vector3(0, 0.3f, 0), toActor.FacePosition() + new Vector3(0, -1.3f, 0), out framingAngle, angle);

		var adjustmentVector = Quaternion.Euler(framingAngle, angle, 0) * Vector3.forward * distanceAdjustment;
		Debug.DrawRay(cameraPosition, adjustmentVector, new Color(1.0f, 1.0f, 0.5f), 3.0f);

		sourcePosition = cameraPosition;
		targetPosition = cameraPosition + Quaternion.Euler(framingAngle, angle, 0) * Vector3.forward;
	}

	void SetShotComposition(SceneActor actorToLookAt, float faceRadius, ShotComposition composition)
	{
/*
		float radius;
		float yOffset = 0;

		switch (composition)
		{
		case ShotComposition.LongShot:
			radius = faceRadius * 5.0f;
			yOffset = 0;
			break;

		case ShotComposition.MediumShot:
			radius = faceRadius * 2.5f;
			yOffset = -0.15f;
			break;

		case ShotComposition.CloseUp:
			radius = faceRadius * 2.0f;
			yOffset = -0.08f;
			break;

		default:
			radius = 0;
			break;
		}
*/
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




