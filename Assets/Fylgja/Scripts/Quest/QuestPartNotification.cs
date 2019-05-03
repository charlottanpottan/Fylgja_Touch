using UnityEngine;
using System.Collections;

public class QuestPartNotification
{
	public Quest quest;
	public ActorSceneComponent part;

	public QuestPartNotification(Quest _quest, ActorSceneComponent _part)
	{
		quest = _quest;
		part = _part;
	}
}

