using UnityEngine;
using System.Collections;

public class BlacksmithMinigame : Minigame, IBlacksmithSwordNotifications
{
	public Camera blacksmithCamera;
	public Camera mainCamera;
	public GameObject charcoalBurner;
	public BlacksmithSledgehammerAction hammerAction;
//	public GameObject timerObject;
//	private BlacksmithTimer timer;
	public int numberOfCompletedSwords;
	public float timeToCompleteSmithing = 3.0f;

//	float smithingMustBeDoneAtTime;
//
//	float savedRemainingTime;
//	bool timerIsPaused;

	void Start()
	{
	}

//	public void PauseTimer()
//	{
//		savedRemainingTime = smithingMustBeDoneAtTime - Time.time;
//		timerIsPaused = true;
//	}
//
//	public void ResumeTimer()
//	{
//		smithingMustBeDoneAtTime = Time.time + savedRemainingTime;
//		savedRemainingTime = 0;
//		timerIsPaused = false;
//	}

//	void Update()
//	{
//		if (!IsMinigameStarted())
//		{
//			return;
//		}
//
//		if (!timerIsPaused)
//		{
//			UpdateTimeLeft();
//			if (Time.time >= smithingMustBeDoneAtTime)
//			{
//				FailedMinigame();
//			}
//		}
//	}

//	void UpdateTimeLeft()
//	{
//		float timeLeftFactor = 1.0f - ((smithingMustBeDoneAtTime - Time.time) / timeToCompleteSmithing);
//
//		timer.OnSmithingTimeLeftFactorChanged(timeLeftFactor);
//	}

	void SetMinigameCameras(bool enable)
	{
		mainCamera.gameObject.SetActiveRecursively1(!enable);
		blacksmithCamera.gameObject.SetActiveRecursively1(enable);
		charcoalBurner.SetActiveRecursively1(enable);
		hammerAction.gameObject.SetActiveRecursively1(enable);
	}

	void EnableMinigameCameras()
	{
		mainCamera = Camera.main;
		SetMinigameCameras(true);
	}

	void DisableMinigameCameras()
	{
		SetMinigameCameras(false);
	}

	public override void StartMinigame(IAvatar a)
	{
		
		//GameObject go = GameObject.Instantiate(timerObject) as GameObject;
		//timer = go.GetComponent<BlacksmithTimer>();
		numberOfCompletedSwords = 0;
		//smithingMustBeDoneAtTime = Time.time + timeToCompleteSmithing;
		EnableMinigameCameras();
		var smithy = a.transform.root.GetComponentInChildren<CharacterBlacksmith>();
		smithy.SetSwordNotifications(this);
		a.transform.parent.BroadcastMessage("OnBlacksmithMinigameStart");
		base.StartMinigame(a);
	}

	void CloseMinigame()
	{
		StartCoroutine(GameObject.Find("Blacksmith").GetComponentInChildren<InteractOnTrigger>().Pause(0.5f));
		//Destroy(timer.gameObject);
		//timer = null;
		DisableMinigameCameras();
	}

	public override void CompletedMinigame()
	{
		avatar.transform.parent.BroadcastMessage("OnBlacksmithMinigameDone");
		CloseMinigame();
		base.CompletedMinigame();
	}

	public override void FailedMinigame()
	{
		avatar.transform.parent.BroadcastMessage("OnBlacksmithMinigameFailed");
		CloseMinigame();
		base.FailedMinigame();
	}

	public override void QuitMinigame()
	{
		avatar.transform.parent.BroadcastMessage("OnBlacksmithMinigameAborted");
		CloseMinigame();
		base.QuitMinigame();
	}

	public bool OnSwordDone(BlacksmithSword sword)
	{
		numberOfCompletedSwords++;
		if (numberOfCompletedSwords == 3)
		{
			CompletedMinigame();
			return true;
		}
		else
		{
			return false;
		}
	}

}
