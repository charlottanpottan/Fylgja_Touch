using UnityEngine;
using System.Collections;

public class InventoryBag : ActionArbitration
{
	public AudioHandler inventoryBagCloseHandler;

	IAvatar avatar;

	public override void ExecuteAction(IAvatar _avatar)
	{
		avatar = _avatar;
		inventoryBagCloseHandler.CreateAndPlay(audio.volume);
		Debug.Log("Closing inventory bag...");
		avatar.CloseInventoryBag();
	}

	public void Close(IAvatar avatar)
	{
		avatar.CloseInventoryBag();
	}

	public void Dispose()
	{
		Destroy(gameObject);
	}

}

