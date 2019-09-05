using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorSetAnimation : ActorSceneComponent
{
    [SerializeField] AnimationClip clip;
    [SerializeField] string actorName;

    protected override void Act()
    {
        SceneActor sceneActor = actingInScene.GetSceneActor(actorName);
        sceneActor.PlayCustomAnimation(clip, 0.3f);
        ComponentDone();
    }

    public override void Skip()
    {
        ComponentDone();
    }
}
