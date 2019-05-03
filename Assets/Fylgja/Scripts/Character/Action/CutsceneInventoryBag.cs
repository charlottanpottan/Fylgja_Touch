using UnityEngine;
using System.Collections;

public class CutsceneInventoryBag : MonoBehaviour
{
	public CharacterAvatar avatar;

	void Start()
	{
	}

	void Update()
	{
	}

	public void ShowBag()
	{
		avatar.OpenInventoryBag();
	}
}

