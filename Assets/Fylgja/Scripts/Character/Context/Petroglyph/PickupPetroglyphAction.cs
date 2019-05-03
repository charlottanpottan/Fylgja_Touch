using UnityEngine;
using System.Collections;

public class PickupPetroglyphAction : ActionArbitration
{
	public Petroglyph petroglyph;

	public override bool IsActionPossible(IAvatar avatar)
	{
		var petroglypher = avatar.transform.root.GetComponentInChildren<CharacterPetroglypher>();
		return !petroglypher.IsHoldingPetroglyph();
	}

	public override void ExecuteAction(IAvatar avatar)
	{
		var petroglypher = avatar.transform.root.GetComponentInChildren<CharacterPetroglypher>();
		petroglypher.PickupPetroglyph(petroglyph);
	}
}

