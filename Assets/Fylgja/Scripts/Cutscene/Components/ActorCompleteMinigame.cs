using UnityEngine;
using System.Collections;

public class ActorCompleteMinigame : ActorSceneComponent
{
	public string actorName;
	public string minigameName;
	public GameObject failedMinigameScene;
	public GameObject quitMinigameScene;

	Minigame minigame;

	public override bool AvatarAllowedToMove()
	{
		return minigame.AllowedToMove();
	}

	public override bool AvatarAllowedToInteract()
	{
		return minigame.AllowedToInteract();
	}

	protected override void Act()
	{
		var o = actingInScene.GetActor(actorName);
		var avatar = o.GetComponentInChildren<IAvatar>();

		var minigameObject = GameObject.Find(minigameName);

		DebugUtilities.Assert(minigameObject != null, "Couldn't find minigame: " + minigameName);
		minigame = minigameObject.GetComponentInChildren<Minigame>();
		DebugUtilities.Assert(minigame != null, "Object does not implement Minigame: " + minigameName);

		minigame.StartMinigame(avatar);
		minigame.completed = OnCompleted;
		minigame.failed = OnFailed;
		minigame.quit = OnQuit;
	}

	public override void Skip()
	{
		ComponentDone();
	}

	void OnCompleted()
	{
		ComponentDone();
	}

	void PlayScene(GameObject actorScenePrefab)
	{
		var scene = ActorSceneUtility.CreateSceneWithAvatar(actorScenePrefab, actingInScene.GetMainAvatar());
		scene.PlayScene(actingInScene.GetMainAvatar().playerNotifications);
	}

	void OnFailed()
	{
		Debug.Log("On Failed minigame!");
		if (failedMinigameScene != null)
		{
			PlayScene(failedMinigameScene);
		}
		ComponentFailed();
	}

	void OnQuit()
	{
		if (quitMinigameScene != null)
		{
			PlayScene(quitMinigameScene);
		}
		ComponentQuit();
	}
}
