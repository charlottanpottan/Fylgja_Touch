using UnityEngine;

public class StickFightMinigame : Minigame
{
	public Camera stickFightCamera;
	public CharacterOpponentStickFight opponentStickFighter;

	Camera mainCamera;

	void Start()
	{
	}

	void Update()
	{
	}

	public override bool AllowedToMove()
	{
		return false;
	}

	public override void StartMinigame(IAvatar a)
	{
		Debug.Log("StickFightMinigame request start!");
		base.StartMinigame(a);
		opponentStickFighter.gameObject.BroadcastMessage("OnStickFightMinigameStart", this);
		avatar.transform.parent.BroadcastMessage("OnStickFightMinigameStart", this);
		mainCamera = Camera.main;
		stickFightCamera.gameObject.SetActiveRecursively(true);
		mainCamera.gameObject.SetActiveRecursively(false);
	}

	void CloseStickFightMinigame()
	{
		stickFightCamera.gameObject.SetActiveRecursively(false);
		mainCamera.gameObject.SetActiveRecursively(true);
		opponentStickFighter.gameObject.BroadcastMessage("OnStickFightMinigameClose");
		avatar.transform.parent.BroadcastMessage("OnStickFightMinigameClose");
	}

	public override void QuitMinigame()
	{
		Debug.Log("Closed minigame!");
		CloseStickFightMinigame();
		//opponentStickFighter.gameObject.BroadcastMessage("OnStickFightMinigameQuit");
		avatar.transform.parent.BroadcastMessage("OnStickFightMinigameQuit");
		base.QuitMinigame();
	}

	public override void CompletedMinigame()
	{
		CloseStickFightMinigame();
		// avatar.transform.parent.BroadcastMessage("OnStickFightMinigameDone");
		base.CompletedMinigame();
	}

	public override void FailedMinigame()
	{
		// avatar.transform.parent.BroadcastMessage("OnStickFightMinigameFailed");
		CloseStickFightMinigame();
		base.FailedMinigame();
	}

	public void OnOpponentLost()
	{
		CompletedMinigame();
	}

	public void OnCharacterLost()
	{
		FailedMinigame();
	}
}

