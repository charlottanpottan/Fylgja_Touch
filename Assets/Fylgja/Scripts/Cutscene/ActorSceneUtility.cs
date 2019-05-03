using System;
using UnityEngine;

public class ActorSceneUtility
{
	public static ActorScene CreateSceneWithAvatar(GameObject actorSceneObject, IAvatar avatar)
	{
		Debug.Log("Creating scene:" + actorSceneObject.name);
		var cutsceneObject = GameObject.Instantiate(actorSceneObject) as GameObject;
		
		var instantiatedCutscene = cutsceneObject.GetComponentInChildren<ActorScene>();
		
		instantiatedCutscene.AddSceneObject("avatar", avatar.gameObject);

		return instantiatedCutscene;
	}
	
	public static ActorScene CreateSceneWithAvatarAndInteractable(GameObject actorSceneObject, IAvatar avatar, GameObject interactable)
	{
		Debug.Log("Creating scene:" + actorSceneObject.name);
		var instantiatedCutscene = CreateSceneWithAvatar(actorSceneObject, avatar);
		instantiatedCutscene.AddSceneObject("interactable", interactable);
		return instantiatedCutscene;
	}
}
