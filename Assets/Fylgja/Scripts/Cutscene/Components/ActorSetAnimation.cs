using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorSetAnimation : ActorSceneComponent
{
    [SerializeField] AnimationClip clip;
    [SerializeField] string actorName;
    [SerializeField] float crossFadeDuration = 0.3f;

    protected override void Act()
    {
        SceneActor sceneActor = actingInScene.GetSceneActor(actorName);
        sceneActor.PlayCustomAnimation(clip, crossFadeDuration);
        ComponentDone();
    }

    public override void Skip()
    {
        ComponentDone();
    }
}
