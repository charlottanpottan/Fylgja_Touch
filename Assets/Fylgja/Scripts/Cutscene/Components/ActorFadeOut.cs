using UnityEngine;
using System.Collections;

public class ActorFadeOut : ActorSceneComponent
{
    public float fadeOutTime = 1.0f;

    protected override void Act()
    {
        if (Mathf.Approximately(fadeOutTime, 0))
        {
            actingInScene.FadeInOut().SetToBlack();
            ComponentDone();
        }
        else
        {
            actingInScene.FadeInOut().FadeOut(fadeOutTime);
            actingInScene.FadeInOut().OnFadeOut += OnFadeOutDone;
        }
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

