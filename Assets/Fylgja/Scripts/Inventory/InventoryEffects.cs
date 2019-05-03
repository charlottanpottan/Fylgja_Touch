using UnityEngine;

public class InventoryEffects : MonoBehaviour
{
	public GameObject inventoryItemsRoot;

	public Camera bagCamera;
	public Animation bagAnimation;
	public AnimationClip bagOpen;
	public AnimationClip bagClose;

	void Start()
	{
	}

	public void SetQuestInventory(QuestInventory inventory)
	{
		HideAllItems();

		var itemNames = inventory.ItemNames();
		foreach (var itemName in itemNames)
		{
			Debug.Log("You are carrying:" + itemName);
			SetQuestItemEnabled(itemName, true);
		}
	}

	void HideAllItems()
	{
		for (var i = 0; i < inventoryItemsRoot.transform.GetChildCount(); ++i)
		{
			var questItemObject = inventoryItemsRoot.transform.GetChild(i).gameObject;
			questItemObject.SetActiveRecursively(false);
		}
	}


	public void OnInventoryAdd(string itemName)
	{
		SetQuestItemEnabled(itemName, true);
	}

	public void OnInventoryRemove(string itemName)
	{
		SetQuestItemEnabled(itemName, false);
	}

	void SetQuestItemEnabled(string itemName, bool enabled)
	{
		Debug.Log("Setting quest item: " + itemName + " to:" + enabled);
		if (enabled)
		{
			for (var i = 0; i < inventoryItemsRoot.transform.GetChildCount(); ++i)
			{
				if (SetQuestItemEnabled(i, itemName, true))
				{
					return;
				}
			}
		}
		else
		{
			for (var i = inventoryItemsRoot.transform.GetChildCount() - 1; i >= 0; --i)
			{
				if (SetQuestItemEnabled(i, itemName, false))
				{
					return;
				}
			}
		}
		DebugUtilities.Assert(false, "Couldn't find inventory item to toggle:" + itemName);
	}

	bool SetQuestItemEnabled(int i, string itemName, bool enabled)
	{
		var questItemObject = inventoryItemsRoot.transform.GetChild(i).gameObject;

		if (questItemObject.active != enabled && itemName == questItemObject.name)
		{
			questItemObject.SetActiveRecursively(enabled);
			return true;
		}

		return false;
	}

	public void OnInventoryBagOpen()
	{
		Debug.Log("Showing bag!");
		// bagCamera.enabled = true;
		// bagAnimation[bagOpen.name].speed = 0.1f;
		// bagAnimation.Play(bagOpen.name);
	}

	public void OnInventoryBagClose()
	{
		Debug.Log("Close bag!");
		bagAnimation[bagClose.name].speed = -1.0f;
		bagAnimation[bagClose.name].normalizedTime = 1.0f;
		bagAnimation.Play(bagClose.name);
	}

}
