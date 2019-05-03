using UnityEngine;

public class BlacksmithAnnealAction : ActionArbitration
{
	public override bool IsActionPossible(IAvatar avatar)
	{
//		var blacksmith = avatar.transform.parent.GetComponentInChildren<CharacterBlacksmith>();
//		return !blacksmith.IsAimingSledgehammer();
		return true;
	}
}
