using UnityEngine;

public class StartActorSceneQuestPart : ActorSceneComponent
{
	public GameObject scene;

	ActorScene instantiatedCutscene;

	protected override void Act()
	{
		var avatar = actingInScene.GetMainAvatar();
		if (avatar)
		{
			avatar.OnCutsceneStart();
		}
		
		DebugUtilities.Assert(scene != null, "Actor scene is null in start actor scene:" + name);
		
		instantiatedCutscene = ActorSceneUtility.CreateSceneWithAvatar(scene, avatar);
		DebugUtilities.Assert(instantiatedCutscene != null, "No valid cutscene specified:" + scene.name);
		instantiatedCutscene.endFunction = OnCutscenePlayed;
		instantiatedCutscene.PlayScene(avatar.playerNotifications);
	}

	public override void Skip()
	{
		DebugUtilities.Assert(instantiatedCutscene != null, "Instantiated Cutscene is null. Can not skip!");
		Debug.Log("STOP ACTING************************************** ######################" + scene.name);
		// var avatar = actingInScene.GetMainAvatar();
		instantiatedCutscene.QuitScene();
		Debug.Log("Destroying object:" + instantiatedCutscene.gameObject.name);
		instantiatedCutscene = null;
	}

	void OnCutscenePlayed()
	{
		var avatar = actingInScene.GetMainAvatar();
		if (avatar)
		{
			avatar.OnCutsceneEnd();
		}
		ComponentDone();
	}
}
