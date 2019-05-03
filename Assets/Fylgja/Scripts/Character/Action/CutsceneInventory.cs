using UnityEngine;
using System.Collections;

public class CutsceneInventory : MonoBehaviour
{
	public QuestInventory inventory;
	public QuestItem[] itemsToAdd;
	public QuestItem[] itemsToRemove;

	void Start()
	{
	}

	void Update()
	{
	}

	public void AddQuestItems()
	{
		foreach (var item in itemsToAdd)
		{
			inventory.Add(item.name);
		}
	}

	public void RemoveQuestItems()
	{
		foreach (var item in itemsToRemove)
		{
			inventory.Remove(item.name);
		}
	}
}
