using UnityEngine;
using System.Collections;

public class ActorFade : ActorSceneComponent
{
	public float fadeInTime = 1.0f;
	public float fadeOutTime = 1.0f;

	protected override void Act()
	{
		actingInScene.FadeInOut().OnFadeOut += OnFadeOutDone;
		actingInScene.FadeInOut().FadeOut(fadeOutTime);
	}

	public override void Skip()
	{
	}

	void OnFadeOutDone()
	{
		actingInScene.FadeInOut().OnFadeOut -= OnFadeOutDone;
		actingInScene.FadeInOut().FadeIn(fadeInTime);
		ComponentDone();
	}

}

