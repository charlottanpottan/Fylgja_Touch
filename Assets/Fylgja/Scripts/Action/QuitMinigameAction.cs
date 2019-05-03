using UnityEngine;
using System.Collections;

public class QuitMinigameAction : ActionArbitration
{
	public override bool IsActionPossible(IAvatar avatar)
	{
		return avatar.IsPlayingMinigame();
	}

	public override void ExecuteAction(IAvatar avatar)
	{
		avatar.QuitMinigame();
	}
}
