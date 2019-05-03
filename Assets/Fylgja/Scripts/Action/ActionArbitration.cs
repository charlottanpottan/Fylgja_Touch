using UnityEngine;
using System.Collections.Generic;

public class ActionArbitration : MonoBehaviour
{
	public ActorScene actorScenePrefab;

	public virtual bool IsActionPossible(IAvatar avatar)
	{
		return true;
	}
	public virtual void ExecuteAction(IAvatar avatar)
	{
	}
	public virtual Transform AvatarInteractTransform(IAvatar avatar)
	{
		return null;
	}

	protected void PerformAction(IAvatar avatar)
	{
		DebugUtilities.Assert(actorScenePrefab != null, "You must have a valid actorScene for the action");
		var instantiatedScene = ActorSceneUtility.CreateSceneWithAvatarAndInteractable(actorScenePrefab.gameObject, avatar, gameObject);
		instantiatedScene.PlayScene(avatar.playerNotifications);
	}

	protected void DenyAction(IAvatar avatar)
	{
	}

	public void ActionRequest(IAvatar avatar)
	{
		if (!IsActionPossible(avatar))
		{
			DenyAction(avatar);
		}
		else
		{
			ExecuteAction(avatar);
			if (actorScenePrefab != null)
			{
				PerformAction(avatar);
			}
		}
	}
}
