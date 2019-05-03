using UnityEngine;

public class Item : ActionArbitration
{
	public override void ExecuteAction(IAvatar avatar)
	{
		Destroy(gameObject);
	}
}
