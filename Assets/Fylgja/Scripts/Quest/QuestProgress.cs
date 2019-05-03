using UnityEngine;
using System.Collections;

public class QuestProgress
{
	public Quest quest;
	public ActorSceneComponent questPart;

	public QuestProgress(Quest _quest, ActorSceneComponent component)
	{
		quest = _quest;
		questPart = component;
	}
}

