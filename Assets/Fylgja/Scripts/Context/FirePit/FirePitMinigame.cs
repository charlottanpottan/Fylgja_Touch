using UnityEngine;
using System.Collections;

public class FirePitMinigame : Minigame, IFirePitNotifications
{
	public FirePit[] firePits;
	public Camera firePitCamera;
	Camera mainCamera;
	public GameObject[] gameAreaCollision;
	public AudioClip music;
	AudioListener playerListener;

	void Start()
	{
		foreach (FirePit firePit in firePits)
		{
			firePit.firePitNotification = this;
		}
	}

	void Update()
	{
	}
	
	public override bool AllowedToMove()
	{
		return true;
	}

	public override bool AllowedToInteract()
	{
		return true;
	}

	public void OnFirePitFullFlame()
	{
		foreach (FirePit firePit in firePits)
		{
			if (!firePit.IsFullFlame())
			{
				Debug.Log("One fire pit didn't have full flame!");
				avatar.transform.parent.BroadcastMessage("OnFirePitFullFlame");
				return;
			}
		}
		Debug.Log("FireMiniGame done!!!");
		CompletedMinigame();
	}

	public override void StartMinigame(IAvatar a)
	{
		GetComponent<AudioSource>().clip = music;
		GetComponent<AudioSource>().Play();
		Debug.Log("FirePitMinigame request start!");
		if (IsMinigameStarted())
		{
			return;
		}
		playerListener = GameObject.FindGameObjectWithTag("Listener").GetComponent<AudioListener>();
		playerListener.enabled = false;
		mainCamera = Camera.main;
		mainCamera.gameObject.SetActiveRecursively(false);
		firePitCamera.gameObject.SetActiveRecursively(true);

		foreach (var obj in gameAreaCollision)
		{
			obj.SetActiveRecursively(true);
		}
		base.StartMinigame(a);
		avatar.transform.parent.BroadcastMessage("OnFirePitMinigameStart");
	}

	public override void QuitMinigame()
	{
		Debug.Log("Closed minigame!");
		
		foreach (FirePit firePit in firePits)
		{
			firePit.ResetFire();
		}
		CloseFirePitMinigame();
		base.QuitMinigame();
	}

	void OnFirePitMinigameComplete()
	{
		CloseFirePitMinigame();
	}

	void CloseFirePitMinigame()
	{
		GetComponent<AudioSource>().Stop();
		playerListener.enabled = true;
		firePitCamera.gameObject.SetActiveRecursively(false);
		mainCamera.gameObject.SetActiveRecursively(true);
		foreach (var obj in gameAreaCollision)
		{
			obj.SetActiveRecursively(false);
		}
	}

	public override void CompletedMinigame()
	{
		foreach (FirePit firePit in firePits)
		{
			firePit.BurnForever();
		}
		CloseFirePitMinigame();
		avatar.transform.parent.BroadcastMessage("OnFirePitMinigameDone");
		base.CompletedMinigame();
	}
}

