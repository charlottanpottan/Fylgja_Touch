using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
	private List<string> items = new List<string>();

	public void Add(Item item)
	{
		Debug.Log("Adding '" + item.name + "' to inventory");
		items.Add(item.name);

		SendMessage("OnItemAddedToInventory", item);
	}

	public void Remove(Item item)
	{
		Debug.Log("Removing '" + item.name + "' from inventory");
		items.Remove(item.name);
		SendMessage("OnItemRemovedFromInventory", item);
	}

	public bool HasItem(Item item)
	{
		return items.Contains(item.name);
	}
}
