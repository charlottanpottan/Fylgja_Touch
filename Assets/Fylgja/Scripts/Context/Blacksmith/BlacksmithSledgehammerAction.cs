using UnityEngine;
using System.Collections;

public class BlacksmithSledgehammerAction : ActionArbitration
{
	public override bool IsActionPossible(IAvatar avatar)
	{
		var blacksmith = avatar.transform.root.gameObject.GetComponentInChildren<CharacterBlacksmith>();
		return !blacksmith.IsAimingSledgehammer();
	}

	public override void ExecuteAction(IAvatar avatar)
	{
		Debug.Log("Use: Sledgehammer");
		avatar.StartAimingSledgehammer();
	}
}
