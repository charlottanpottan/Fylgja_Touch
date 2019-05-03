using UnityEngine;
using System.Collections;

public class ActorPosition : ActorSceneComponent
{
	protected override void Act()
	{
		GameObject o = actingInScene.GetActor(name);

		o.transform.position = transform.position;
		o.transform.rotation = transform.rotation;

		ComponentDone();
	}

	public override void Skip()
	{
		ComponentDone();
	}
}
