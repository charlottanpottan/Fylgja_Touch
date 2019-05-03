using UnityEngine;
using System.Collections;

public class FirePitTools : ActionArbitration
{
	public override bool IsActionPossible(IAvatar avatar)
	{
		var igniter = avatar.GetComponentInChildren<CharacterFirePitMinigame>();
		return !igniter.HasFirePitTools();
	}
}
