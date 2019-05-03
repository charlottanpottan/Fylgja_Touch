using UnityEngine;
using System.Collections;

public class ActorFadeIn : ActorSceneComponent
{
	public float fadeInTime = 1.0f;

	protected override void Act()
	{
		actingInScene.FadeInOut().OnFadeIn += OnFadeInDone;
		actingInScene.FadeInOut().FadeIn(fadeInTime);
	}

	public override void Skip()
	{

	}

	void OnFadeInDone()
	{
		actingInScene.FadeInOut().OnFadeIn -= OnFadeInDone;
		ComponentDone();
	}
}

