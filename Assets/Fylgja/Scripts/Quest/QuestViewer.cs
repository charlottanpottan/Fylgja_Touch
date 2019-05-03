using UnityEngine;

public class QuestViewer : MonoBehaviour
{
	private Item activeQuest;

	void Start()
	{
	}

	void Update()
	{
	}

	public void ShowQuest(Item quest)
	{
		activeQuest = quest;
	}

	public Item ActiveQuest()
	{
		return activeQuest;
	}
}
