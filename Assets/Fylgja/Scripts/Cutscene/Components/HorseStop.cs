using UnityEngine;
using System.Collections;

public class HorseStop : ActorSceneComponent
{
    protected override void Act()
    {
        GameObject o = actingInScene.GetActor("Horse");
        Horse horse = o.GetComponent<Horse>();

        horse.StopMovingInstant();

        ComponentDone();
    }

    public override void Skip()
    {
        ComponentDone();
    }
}
