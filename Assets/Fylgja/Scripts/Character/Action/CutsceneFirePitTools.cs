using UnityEngine;
using System.Collections;

public class CutsceneFirePitTools : MonoBehaviour
{
	public CharacterFirePitMinigame avatarFirePit;

	void Start()
	{
	}

	void Update()
	{
	}

	public void PickupFirePitTools()
	{
		avatarFirePit.SetEquipmentEnable(true);
	}
}
