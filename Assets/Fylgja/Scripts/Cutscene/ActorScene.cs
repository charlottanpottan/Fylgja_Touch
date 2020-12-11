using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class ActorScene : MonoBehaviour, ActorSceneComponentNotification
{
    public ActorPosition[] actors;
    public ActorSceneComponent[] lines;
    public ActorPosition[] actorsAfterScene;
    public bool keepAvatarLocked = false;
    public bool fadeAtStart = false;
    public bool fadeAtEnd = false;
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

    LogicCameraInfo logicCameraInfo = new LogicCameraInfo();
    bool hasValidLogicCameraInfo;

    FadeInFadeOut fadeInOut;

    float fadeOutDoneAt = -1.0f;
    const float fadeOutTime = 0.4f;
    const float fadeInTime = 0.7f;

    bool gotoNextLine;

    bool isResuming;

#if UNITY_EDITOR
    static int numActorScenes;
    int thisNum;
    string debugString;
#endif

    public FadeInFadeOut FadeInOut()
    {
        return fadeInOut;
    }

    void Awake()
    {
        fadeInOut = gameObject.AddComponent<FadeInFadeOut>();
#if UNITY_EDITOR
        numActorScenes++;
        thisNum = numActorScenes;
#endif
    }

    void OnDestroy()
    {
#if UNITY_EDITOR
        numActorScenes--;
#endif
    }

    public bool IsResuming()
    {
        return isResuming;
    }

    bool IsReallyAQuest()
    {
        return (this as Quest) != null;
    }

    public void AddSceneObject(string name, GameObject o)
    {
        Debug.Log("ActorScene: Alias '" + name + "' is set to " + o.name + " (" + o.transform.root.name + ")");
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

        Debug.Log("Line: " + currentLine.name + " _________________________________________");

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
        Debug.Log($"PlayScene! {name} resuming: {isResuming} {Time.time}");
        DebugUtilities.Assert(notifications != null, "Player notifications can not be null");
        playerNotifications = notifications;
        if (fadeAtStart)
        {
            fadeInOut.FadeOut(fadeOutTime);
            fadeOutDoneAt = Time.time + fadeOutTime;
        }
        else
        {
            fadeOutDoneAt = Time.time;
        }
    }

    void PreSceneFadeOutDone()
    {
        Debug.Log("pre scene fade is done");
        PrepareScene();
        PlayNextLine();

        if (fadeAtStart)
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
            SceneManager.LoadScene("Loading");
        }
        else
        {
            if (fadeAtEnd)
            {
                fadeInOut.FadeIn(fadeInTime);
            }
        }
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
        SceneActor mainActor = FindSceneActor("Tyra");
        if (mainActor == null)
        {
            return null;
        }
        var avatar = mainActor.GetComponentInChildren<CharacterAvatar>();
        return avatar;
    }

    void SetInteractionForMainCharacter(bool enabled)
    {
        var avatar = GetMainAvatar();
        if (avatar == null)
        {
            return;
        }

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
        if (avatar == null)
        {
            return;
        }
        
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
        if (fadeAtEnd)
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
            Debug.Log($"is not acting {Time.time} > {fadeOutDoneAt}");
            if (fadeOutDoneAt >= 0 && Time.time > fadeOutDoneAt)
            {
                fadeOutDoneAt = -1.0f;
                PreSceneFadeOutDone();
            }
            return;
        }
        else if (isEndOfScene)
        {
            Debug.Log("is isEndOfScene");
            if (fadeOutDoneAt >= 0 && Time.time > fadeOutDoneAt)
            {
                fadeOutDoneAt = -1.0f;
                PostSceneFadeOutDone();
            }
            return;
        }

        if (gotoNextLine)
        {
            Debug.Log("go to next line");
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

        Debug.Log($"play next line {lineIndex} ");
        activeLine = lines[lineIndex];
        if (activeLine)
            Debug.Log("Actorscene " + name + " sets line: " + activeLine.name + " _________________________________________");
        else
            Debug.Log("Actorscene " + name + " has a missing line with index: " + lineIndex + " _________________________________________!!");


#if UNITY_EDITOR
        debugString = name + ": " + activeLine.name;
#endif

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
        Debug.Log($"fetching actor {name}");
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

    public SceneActor FindSceneActor(string name)
    {
        var worked = actorsInScene.TryGetValue(name, out var gameObject);
        if (!worked)
        {
            return null;
        }
        
        return gameObject.GetComponentInChildren<SceneActor>();
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
            DebugUtilities.Assert(!actorsInScene.ContainsKey(foundActorName), "We already have " + foundActorName + " in scene " + name);
            actorsInScene.Add(foundActorName, sceneActor.gameObject);
        }
    }

    void PlaceActorsBeforeScene()
    {
        Debug.Log("place actors");
        foreach (var actor in actors)
        {
            Debug.Log("place actor '{actor}'");
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

#if UNITY_EDITOR
    void OnGUI()
    {
        GUIStyle guiStyle = new GUIStyle();
        guiStyle.fontSize = 40;
        GUI.color = Color.white;
        GUILayout.Space(40 * thisNum);
        GUILayout.Label(debugString, guiStyle);
    }
#endif
}
