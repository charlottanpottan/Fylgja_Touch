using UnityEngine;
using System.Collections;

public class ActorFadeOut : ActorSceneComponent
{
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
		ComponentDone();
	}
}

