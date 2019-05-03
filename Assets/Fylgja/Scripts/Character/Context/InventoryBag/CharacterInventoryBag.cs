using UnityEngine;
using System.Collections;

public class CharacterInventoryBag : MonoBehaviour
{
	public GameObject inventoryBag;
	public PlayerInventoryBag playerInventoryBag;
	private Transform transformToSpawnBag;
	public QuestInventory questInventory;
	private Camera inventoryCamera;

	public delegate void NotifyOnClose(CharacterInventoryBag parchment);
	public NotifyOnClose notifyOnClose;

	InventoryBag instantiatedBag;
	
	void Start()
	{
		inventoryCamera = GameObject.FindGameObjectWithTag("InventoryCamera").GetComponent<Camera>();
		transformToSpawnBag = GameObject.FindGameObjectWithTag("InventoryBagLocator").transform;
	}

	void OnInventoryBagOpen()
	{
		instantiatedBag = (Instantiate(inventoryBag) as GameObject).GetComponent<InventoryBag>();
		var inventoryEffects = instantiatedBag.GetComponent<InventoryEffects>();

		inventoryEffects.SetQuestInventory(questInventory);

		instantiatedBag.transform.parent = transformToSpawnBag;
		instantiatedBag.transform.localPosition = new Vector3(0, 0, 0);
		instantiatedBag.transform.localRotation = new Quaternion();

		inventoryCamera.enabled = true;
	}

	public void Close(IAvatar avatar)
	{
		instantiatedBag.Close(avatar);
	}

	void OnInventoryBagClose()
	{
		Debug.Log("CharacterInventoryBag: OnInventoryBagClose");
		playerInventoryBag.SetCanOpenBagState(true);
		if (notifyOnClose != null)
		{
			notifyOnClose(this);
			notifyOnClose = null;
		}
	}

	public void Dispose()
	{
		inventoryCamera.enabled = false;
		instantiatedBag.Dispose();
	}
}
