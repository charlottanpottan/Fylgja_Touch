using UnityEngine;
using System.Collections.Generic;

public class ActorCompleteScenes : ActorSceneComponent
{
	public ActorScene[] scenePrefabs;
	public string actorName;

	List<ActorScene> instantiatedScenes = new List<ActorScene>();

	public override bool AvatarAllowedToMove()
	{
		return true;
	}

	public override bool AvatarAllowedToInteract()
	{
		return true;
	}

	protected override void Act()
	{
		Debug.Log("ActorCompleteScenes:Act");
		var avatarObject = actingInScene.GetActor(actorName);
		var avatarQuest = avatarObject.GetComponentInChildren<AvatarQuest>();
		foreach (var scenePrefab in scenePrefabs)
		{
			Debug.Log("ActorCompleteScenes:Instantiating:" + scenePrefab.name);
			ActorScene instantiatedScene;
			if (!actingInScene.IsResuming())
			{
				instantiatedScene = avatarQuest.CreateQuest(scenePrefab);
			}
			else
			{
				instantiatedScene = avatarQuest.FetchQuest(scenePrefab.name);
				if (instantiatedScene == null)
				{
					continue;
				}
			}
			instantiatedScenes.Add(instantiatedScene);
			instantiatedScene.endOfSceneNotification += OnEndOfScene;
		}
	}

	public override void Skip()
	{
	}



	void OnEndOfScene(ActorScene sceneThatEnded)
	{
		sceneThatEnded.endOfSceneNotification -= OnEndOfScene;

		instantiatedScenes.Remove(sceneThatEnded);

		if (instantiatedScenes.Count == 0)
		{
			ComponentDone();
		}
	}
}
