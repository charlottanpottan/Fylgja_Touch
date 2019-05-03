using UnityEngine;
using System.Collections.Generic;

public class QuestInventory : MonoBehaviour
{
	List<string> items = new List<string> ();

	public void SetItems(ICollection<string> itemIds)
	{
		items = new List<string>(itemIds);
		Debug.Log("Received inventory items: ");
		foreach (var item in items)
		{
			Debug.Log("# " + item);
		}
	}

	public List<string> ItemNames()
	{
		return items;
	}

	public void Add(string questItemName)
	{
		Debug.Log("Adding " + questItemName + " to the inventory");
		items.Add(questItemName);
		BroadcastMessage("OnInventoryAdd", questItemName);
	}

	public void Remove(string questItemName)
	{
		items.Remove(questItemName);
		BroadcastMessage("OnInventoryRemove", questItemName);
	}

	public bool HasItem(QuestItem item)
	{
		return items.Contains(item.name);
	}

	public bool HasAllItems(QuestItem[] items)
	{
		var itemsMissing = MissingItems(items);

		return(itemsMissing.Count == 0);
	}

	List<QuestItem> MissingItems(QuestItem[] questItems)
	{
		List<QuestItem> missingItems = new List<QuestItem>();

		List<string> inventoryItems = new List<string>(items);
		foreach (var itemToPickup in questItems)
		{
			if (!inventoryItems.Contains(itemToPickup.name))
			{
				missingItems.Add(itemToPickup);
			}
			inventoryItems.Remove(itemToPickup.name);
		}

		return missingItems;
	}

	public bool NeedMoreOf(QuestItem[] questItems, string itemNameToLookFor)
	{
		DebugUtilities.Assert(questItems != null, "Items can not be null!");
		var missingItems = MissingItems(questItems);

		foreach (var missingItem in missingItems)
		{
			if (missingItem.name == itemNameToLookFor)
			{
				// Debug.Log("Yes, we are missing a: " + itemNameToLookFor);
				return true;
			}
		}
		return false;
	}

	public bool NeedMoreOf(QuestItem[] items, QuestItem itemToLook)
	{
		return NeedMoreOf(items, itemToLook.name);
	}

	public void ClearAllItems()
	{
		var allItems = new List<string>(items);

		foreach (var itemName in allItems)
		{
			Remove(itemName);
		}
		BroadcastMessage("OnInventoryCleared", SendMessageOptions.DontRequireReceiver);
	}
}
