using UnityEngine;

public class StickFightTools : ActionArbitration
{
	public override bool IsActionPossible(IAvatar avatar)
	{
		return !avatar.HasFightStick();
	}
}
