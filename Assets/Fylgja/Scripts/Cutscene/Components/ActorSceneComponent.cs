using UnityEngine;
using System.Collections;

public interface ActorSceneComponentNotification
{
	GameObject GetActor(string name);
	SceneActor GetSceneActor(string name);
	IAvatar GetMainAvatar();
	AvatarToPlayerNotifications GetPlayerNotifications();
	FadeInFadeOut FadeInOut();
	GameObject GetGameObject();
	bool IsResuming();
}

public abstract class ActorSceneComponent : MonoBehaviour
{
	protected ActorSceneComponentNotification actingInScene;

	public delegate void ComponentDoneDelegate(ActorSceneComponent component);
	public ComponentDoneDelegate OnComponentDone;

	public delegate void ComponentFailedDelegate(ActorSceneComponent component);
	public ComponentFailedDelegate OnComponentFailed;

	public delegate void ComponentQuitDelegate(ActorSceneComponent component);
	public ComponentQuitDelegate OnComponentQuit;

	bool isDone;

	public virtual bool AvatarAllowedToMove()
	{
		return false;
	}

	public virtual bool AvatarAllowedToInteract()
	{
		return false;
	}

	public virtual bool CanBeInterrupted()
	{
		return false;
	}

	public virtual void Dispose()
	{
	}

	public virtual void Resume()
	{
	}

	public bool IsInScene()
	{
		return actingInScene != null;
	}

	public void ActInScene(ActorSceneComponentNotification scene)
	{
		actingInScene = scene;
		Act();
	}

	public void RemoveFromScene()
	{
		actingInScene = null;
	}

	protected abstract void Act();
	public abstract void Skip();

	public bool ShouldUpdate
	{
		get
		{
			return IsInScene() && !isDone;
		}
	}

	protected void ComponentDone()
	{
		isDone = true;
		if (OnComponentDone != null)
		{
			OnComponentDone(this);
		}
	}

	protected void ComponentFailed()
	{
		if (OnComponentFailed != null)
		{
			OnComponentFailed(this);
		}
	}

	protected void ComponentQuit()
	{
		if (OnComponentFailed != null)
		{
			OnComponentQuit(this);
		}
	}
}
