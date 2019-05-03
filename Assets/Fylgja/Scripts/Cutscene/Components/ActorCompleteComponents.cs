using UnityEngine;
using System.Collections;

public class ActorCompleteComponents : ActorSceneComponent, ActorSceneComponentNotification
{
	public ActorSceneComponent[] components;

	protected override void Act()
	{
		foreach (var component in components)
		{
			component.OnComponentDone += OnSubComponentDone;
			component.OnComponentFailed += OnSubComponentFailed;
			component.OnComponentQuit += OnSubComponentQuit;
			component.ActInScene(this);
		}
	}

	public override bool CanBeInterrupted()
	{
		foreach (var component in components)
		{
			if (component.CanBeInterrupted())
			{
				return true;
			}
		}

		return false;
	}

	public override void Skip()
	{
		foreach (var component in components)
		{
			component.Skip();
		}
	}

	void OnSubComponentDone(ActorSceneComponent component)
	{
		Debug.Log("ActorCompleteComponents: " + component.name + " is done.");
		ComponentEnded(component);
	}

	void OnSubComponentFailed(ActorSceneComponent component)
	{
		Debug.Log("ActorCompleteComponents: " + component.name + " Failed!");
		ComponentEnded(component);
	}

	void OnSubComponentQuit(ActorSceneComponent component)
	{
		Debug.Log("ActorCompleteComponents: " + component.name + " QUIT!");
		ComponentEnded(component);
	}

	void ComponentEnded(ActorSceneComponent componentThatIsDone)
	{
		Debug.Log("STOP ACTING (splitter) " + componentThatIsDone.name);
		componentThatIsDone.Dispose();
		foreach (var component in components)
		{
			if (component.ShouldUpdate)
			{
				return;
			}
		}

		ComponentDone();
	}


	public bool IsResuming()
	{
		return actingInScene.IsResuming();
	}


	public GameObject GetActor(string name)
	{
		return actingInScene.GetActor(name);
	}

	public SceneActor GetSceneActor(string name)
	{
		return actingInScene.GetSceneActor(name);
	}

	public IAvatar GetMainAvatar()
	{
		return actingInScene.GetMainAvatar();
	}

	public GameObject GetGameObject()
	{
		return actingInScene.GetGameObject();
	}

	public AvatarToPlayerNotifications GetPlayerNotifications()
	{
		return actingInScene.GetPlayerNotifications();
	}

	public FadeInFadeOut FadeInOut()
	{
		return actingInScene.FadeInOut();
	}
}
