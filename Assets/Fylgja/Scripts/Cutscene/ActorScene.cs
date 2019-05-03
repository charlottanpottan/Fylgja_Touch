using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ActorScene : LogicCamera, ActorSceneComponentNotification
{
	public ActorPosition[] actors;
	public ActorSceneComponent[] lines;
	public ActorPosition[] actorsAfterScene;
	public bool keepAvatarLocked = false;
	public bool useFader = true;
	public bool skippable = true;

	public delegate void ActiveLineNotification(ActorScene scene, ActorSceneComponent component);
	public ActiveLineNotification activeLineNotification;

	public delegate void EndOfSceneNotification(ActorScene scene);
	public EndOfSceneNotification endOfSceneNotification;

	public delegate void SceneAbortedNotification(ActorScene scene);
	public SceneAbortedNotification sceneAbortedNotification;

	int lineIndex;
	Dictionary<string, GameObject> actorsInScene = new Dictionary<string, GameObject>();

	ActorSceneComponent activeLine;

	public delegate void SceneEnd();
	public SceneEnd endFunction;

	float deliverNextLineAtTime;
	bool isLineStopped;

	GameObject listener;

	bool isActingScene;
	bool isEndOfScene;
	
	AllowedToMoveModifier dontMoveModifier;
	AllowedToInteractModifier dontInteractModifier;
	AvatarToPlayerNotifications playerNotifications;
	PlayerInteraction.ListenerStackItem listenerHandle;
	PlayerInteraction.CameraItem selectedCameraHandle;

	LogicCameraInfo logicCameraInfo = new LogicCameraInfo();
	bool hasValidLogicCameraInfo;

	FadeInFadeOut fadeInOut;

	float fadeOutDoneAt = -1.0f;
	const float fadeOutTime = 0.4f;
	const float fadeInTime = 0.7f;

	bool gotoNextLine;

	bool isResuming;

	public FadeInFadeOut FadeInOut()
	{
		return fadeInOut;
	}

	void Awake()
	{
		fadeInOut = gameObject.AddComponent<FadeInFadeOut>();
	}

	public bool IsResuming()
	{
		return isResuming;
	}

	bool IsReallyAQuest()
	{
		return (this as Quest) != null;
	}

	void ChildListenerToCamera()
	{
		Debug.Log("Camera is:" + Camera.main.name);
		listenerHandle = playerNotifications.AttachListener(Camera.main.transform);
	}

	void KickoutListenerFromScene()
	{
		Debug.Log("KickoutListenerFromScene");
		playerNotifications.DetachListener(listenerHandle);
		listenerHandle = null;
	}
	
	public override void UpdateCamera(ref LogicCameraInfo cameraInfo)
	{
		var actorLine = activeLine as ActorLine;
		if (actorLine != null)
		{
			actorLine.UpdateCamera(ref logicCameraInfo);
			hasValidLogicCameraInfo = true;
		}
		else
		{
			DebugUtilities.Assert(hasValidLogicCameraInfo, "Asking for camera but we haven't set any yet!");
		}

		cameraInfo = logicCameraInfo;
	}
	
	public override void SetCameraPivot(ref LogicCameraInfo cameraInfo, Vector2 targetPivot)
	{
	}

	public void AddSceneObject(string name, GameObject o)
	{
		Debug.Log("ActorScene: Alias '" + name + "' is set to " + o.name + " (" + o.transform.root.name + ")" );
		DebugUtilities.Assert(!actorsInScene.ContainsKey(name), "We already have an actor named '" + name + "'");
		actorsInScene.Add(name, o);
	}

	public void SkipToComponent(string skipToComponentName)
	{
		if (skipToComponentName == string.Empty)
		{
			Debug.Log("We skip to the first component");
			lineIndex = 0;
			return;
		}

		DebugUtilities.Assert(!isActingScene, "We can not skip while acting a scene!");
		DebugUtilities.Assert(activeLine == null, "We can not skip while we are acting a line in a scene!");

		var currentLine = lines[lineIndex];

		while (skipToComponentName != currentLine.name)
		{
			Debug.Log("*** We skip line:" + currentLine.name + ". Looking for: " + skipToComponentName);
			lineIndex++;
			DebugUtilities.Assert(lineIndex < lines.Length, "Illegal index! Couldn't find component:" + skipToComponentName);
			currentLine = lines[lineIndex];
		}

		// PlayNextLine();
	}

	public GameObject GetGameObject()
	{
		return gameObject;
	}

	public void Resume()
	{
		isResuming = true;
	}

	public void PlayScene(AvatarToPlayerNotifications notifications)
	{
		Debug.Log("PlayScene!" + name + " resuming:" + isResuming);
		DebugUtilities.Assert(notifications != null, "Player notifications can not be null");
		playerNotifications = notifications;
		if (useFader)
		{
			fadeInOut.FadeOut(fadeOutTime);
			fadeOutDoneAt = Time.time + fadeOutTime;
		}
		else
		{
			fadeOutDoneAt = Time.time + fadeOutTime;
		}
	}

	void PreSceneFadeOutDone()
	{
		PrepareScene();
		PlayNextLine();

		if (useFader)
		{
			fadeInOut.FadeIn(fadeInTime);
		}
	}

	void PostSceneFadeOutDone()
	{
		Debug.Log("Scene is completely done:" + name);
		if (endFunction != null)
		{
			endFunction();
		}

		bool isARealScene = endOfSceneNotification != null;
		if (endOfSceneNotification != null)
		{
			Debug.Log("End of scene notification for scene:" + name);
			endOfSceneNotification(this);
		}
		else
		{
			Debug.Log("No end of scene notification for scene:" + name);
		}

		CloseScene();

		if (isARealScene && Global.levelId != null)
		{
			Application.LoadLevel("Loading");
		}
		else
		{
			if (useFader)
			{
				fadeInOut.FadeIn(fadeInTime);
			}
		}
	}

	void AttachCameraAndListener()
	{
		Debug.Log("########## ATTACH CAMERA AND LISTENER!" + name);
		ChildListenerToCamera();
		selectedCameraHandle = playerNotifications.AddCameraToStack(this, "ActorScene");
	}

	void KickoutCameraAndListener()
	{
		Debug.Log("########## KICKOUT CAMERA AND LISTENER!" + name);
		KickoutListenerFromScene();
		playerNotifications.RemoveCameraFromStack(selectedCameraHandle);
		selectedCameraHandle = null;
	}

	void PrepareScene()
	{
		Debug.Log("=== Preparing scene:" + gameObject.name);
		FindActorsInWorld();
		PlaceActorsBeforeScene();

		isActingScene = true;
	}

	public AvatarToPlayerNotifications GetPlayerNotifications()
	{
		DebugUtilities.Assert(playerNotifications != null, "We have no player notifications!!");
		return playerNotifications;
	}

	public IAvatar GetMainAvatar()
	{
		SceneActor mainActor = GetSceneActor("Tyra");

		if (!mainActor)
		{
			return null;
		}
		var avatar = mainActor.GetComponentInChildren<CharacterAvatar>();
		return avatar;
	}

	void SetInteractionForMainCharacter(bool enabled)
	{
		var avatar = GetMainAvatar();

		if (!enabled)
		{
			if (dontInteractModifier == null)
			{
				dontInteractModifier = new AllowedToInteractModifier();
				avatar.AddAllowedToInteractModifier(dontInteractModifier);
			}
		}
		else if (dontInteractModifier != null)
		{
			avatar.RemoveAllowedToInteractModifier(dontInteractModifier);
			dontInteractModifier = null;
		}
	}

	void SetMovementForMainCharacter(bool enabled)
	{
		var avatar = GetMainAvatar();

		if (!enabled)
		{
			if (dontMoveModifier == null)
			{
				dontMoveModifier = new AllowedToMoveModifier("actorscene");
				avatar.AddAllowedToMoveModifier(dontMoveModifier);
			}
		}
		else if (dontMoveModifier != null)
		{
			avatar.RemoveAllowedToMoveModifier(dontMoveModifier);
			dontMoveModifier = null;
		}
	}

	void CompletedScene()
	{
		Debug.Log("Completed Scene!" + name);
		fadeOutDoneAt = Time.time + fadeOutTime;
		if (useFader)
		{
			fadeInOut.FadeOut(fadeOutTime);
		}
		isEndOfScene = true;
	}

	void CloseScene()
	{
		if (activeLine != null)
		{
			activeLine.Dispose();
			activeLine.RemoveFromScene();
			activeLine = null;
		}

		Debug.Log("### CLOSING SCENE:" + name);

		if (selectedCameraHandle != null)
		{
			KickoutCameraAndListener();
		}
		else
		{
			Debug.Log("No camera or listener was attached to:" + name);
		}

		isActingScene = false;
		PlaceActorsAfterScene();
		NotifyActorsThatSceneIsOver();
		SetInteractionForMainCharacter(true);
		SetMovementForMainCharacter(true);
		actorsInScene.Clear();
		Destroy(transform.root.gameObject);
	}

	public bool IsPlaying()
	{
		return isActingScene;
	}

	void NotifyActorsThatSceneIsOver()
	{
		foreach (var actorKeyValue in actorsInScene)
		{
			var actorObject = actorKeyValue.Value;
			if (actorObject == null)
			{
				Debug.Log("Actor: " + actorKeyValue.Key + " disappeared!");
				continue;
			}
			var sceneActor = actorObject.GetComponentInChildren<SceneActor>();
			if (sceneActor != null)
			{
				if (sceneActor.IsInScene())
				{
					sceneActor.ActorSceneExit();
				}
				else
				{
				}
			}
			else
			{
			}
		}
	}

	void Update()
	{
		if (!isActingScene)
		{
			if (fadeOutDoneAt > 0 && Time.time > fadeOutDoneAt)
			{
				fadeOutDoneAt = -1.0f;
				PreSceneFadeOutDone();
			}
			return;
		}
		else if (isEndOfScene)
		{
			if (fadeOutDoneAt > 0 && Time.time > fadeOutDoneAt)
			{
				fadeOutDoneAt = -1.0f;
				PostSceneFadeOutDone();
			}
			return;
		}

		if (gotoNextLine)
		{
			PlayNextLine();
		}
		else if (activeLine.CanBeInterrupted() && skippable && Input.GetButtonDown("interact") && !GetMainAvatar().player.playerInteraction.AllowedToUseUI)
		{
			activeLine.Skip();
		}
	}

	bool SceneIsDone()
	{
		return lineIndex >= lines.Length;
	}

	void PlayNextLine()
	{
		gotoNextLine = false;

		if (SceneIsDone())
		{
			CompletedScene();
			return;
		}

		if (activeLine != null)
		{
			activeLine.Dispose();
			activeLine = null;
		}

		activeLine = lines[lineIndex];

		var actorLine = activeLine as ActorLine;
		if (actorLine != null && actorLine.shotComposition != ShotComposition.UsePrevious && selectedCameraHandle == null)
		{
			AttachCameraAndListener();
		}

		DebugUtilities.Assert(activeLine != null, "Active Line is null");
		lineIndex++;

		if (activeLineNotification != null)
		{
			activeLineNotification(this, activeLine);
		}

		DebugUtilities.Assert(activeLine != null, "Active Line is null before act");
		Debug.Log("ACT: scene: " + activeLine.name + " " + activeLine.GetType());

		activeLine.OnComponentDone += OnComponentDone;
		activeLine.OnComponentFailed += OnComponentFailed;
		activeLine.OnComponentQuit += OnComponentQuit;

		activeLine.ActInScene(this);

		SetupMoveAndInteraction();

		isResuming = false;
	}

	void SetupMoveAndInteraction()
	{
		if (activeLine != null)
		{
			DebugUtilities.Assert(activeLine != null, "Active Line is null after act");
			bool canMove = activeLine.AvatarAllowedToMove();
			bool canInteract = activeLine.AvatarAllowedToInteract();
			SetInteractionForMainCharacter(canInteract);
			SetMovementForMainCharacter(canMove);
		}
	}

	void SkipLine()
	{
		activeLine.Skip();
	}



	public void QuitScene()
	{
		if (sceneAbortedNotification != null)
		{
			sceneAbortedNotification(this);
		}
		CloseScene();
	}


	void OnComponentDone(ActorSceneComponent component)
	{
		Debug.Log("Component reported done:" + component.name);
		DebugUtilities.Assert(component == activeLine, "Got Component Done from a component that is not acting. Got:" + component.name + " expecting:" + activeLine.name);
		gotoNextLine = true;
	}

	void OnComponentFailed(ActorSceneComponent component)
	{
		QuitScene();
	}

	void OnComponentQuit(ActorSceneComponent component)
	{
		QuitScene();
	}

	public GameObject GetActor(string name)
	{
		DebugUtilities.Assert(actorsInScene.ContainsKey(name), "We have no actor named '" + name + "'");
		return actorsInScene[name];
	}

	public ActorSceneComponent ActingComponent()
	{
		return activeLine;
	}

	public SceneActor GetSceneActor(string name)
	{
		return GetActor(name).GetComponentInChildren<SceneActor>();
	}

	void FindActorsInWorld()
	{
		var instantiatedActors = FindObjectsOfType(typeof(SceneActor));

		foreach (var instantiatedActorObject in instantiatedActors)
		{
			var foundActorName = instantiatedActorObject.name;
			if (foundActorName == "Tyra(Clone)")
			{
				foundActorName = "Tyra";
			}
			var sceneActor = instantiatedActorObject as SceneActor;
			DebugUtilities.Assert(sceneActor != null, "Actor " + instantiatedActorObject.name + " has no SceneActor component!");
			DebugUtilities.Assert(!actorsInScene.ContainsKey(foundActorName), "We already have " + foundActorName + " in this scene");
			actorsInScene.Add(foundActorName, sceneActor.gameObject);
		}
	}

	void PlaceActorsBeforeScene()
	{
		foreach (var actor in actors)
		{
			var actorObject = GetActor(actor.name);
			var sceneActor = actor.GetComponentInChildren<SceneActor>();
			if (sceneActor)
			{
				sceneActor.ActorSceneEnter();
			}
			actorObject.transform.position = actor.transform.position;
			actorObject.transform.rotation = actor.transform.rotation;
		}
	}

	void PlaceActorsAfterScene()
	{
		foreach (var actorPosition in actorsAfterScene)
		{
			var instantiatedActor = GetActor(actorPosition.name);
			instantiatedActor.transform.position = actorPosition.transform.position;
			instantiatedActor.transform.rotation = actorPosition.transform.rotation;
		}
	}


	public Transform GetCameraTransform()
	{
		return transform;
	}
}
