using UnityEngine;
using System.Collections;

public class BlacksmithTools : ActionArbitration
{
	public override bool IsActionPossible(IAvatar avatar)
	{
		return !avatar.IsPlayingMinigame();
	}
}
