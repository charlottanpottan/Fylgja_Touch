using UnityEngine;
using System.Collections;

public class CutsceneQuestParchment : MonoBehaviour
{
	public IAvatar avatar;

	void Start()
	{
	}

	void Update()
	{
	}

	public void ShowParchment()
	{
		avatar.ShowQuestParchment();
	}
}
