using UnityEngine;
using System.Collections;

public class PetroglyphMinigame : Minigame
{
	public Camera petroglyphCamera;

	Camera mainCamera;

	void Start()
	{
	}

	void Update()
	{
	}

	public override void StartMinigame(IAvatar a)
	{
		Debug.Log("Started minigame!");
		mainCamera = Camera.main;
		mainCamera.gameObject.SetActiveRecursively(false);
		mainCamera.tag = "";
		petroglyphCamera.gameObject.SetActiveRecursively(true);

		base.StartMinigame(a);
		avatar.transform.parent.BroadcastMessage("OnPetroglyphMinigameStart");
	}

	public override void QuitMinigame()
	{
		Debug.Log("Closed minigame!");
		CloseMinigame();
		
		avatar.transform.parent.BroadcastMessage("OnPetroglyphMinigameDone");
		base.CompletedMinigame();

		//avatar.transform.parent.BroadcastMessage("OnPetroglyphMinigameAborted");
		//base.QuitMinigame();
	}

	void CloseMinigame()
	{
		mainCamera.gameObject.SetActiveRecursively(true);
		petroglyphCamera.gameObject.SetActiveRecursively(false);
		mainCamera.tag = "MainCamera";
	}


}
