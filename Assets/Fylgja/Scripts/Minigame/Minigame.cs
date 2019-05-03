using UnityEngine;
using System.Collections;

public class Minigame : MonoBehaviour
{
	protected IAvatar avatar;
	public delegate void MinigameCompleted();
	public delegate void MinigameFailed();
	public delegate void MinigameQuit();

	bool minigameStarted = false;

	public MinigameCompleted completed;
	public MinigameFailed failed;
	public MinigameQuit quit;

	public virtual bool AllowedToMove()
	{
		return false;
	}

	public virtual bool AllowedToInteract()
	{
		return true;
	}

	// Use this for initialization
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
	}

	public bool IsMinigameStarted()
	{
		return minigameStarted;
	}

	public virtual void StartMinigame(IAvatar a)
	{
		avatar = a;
		avatar.transform.parent.BroadcastMessage("OnMinigameStart", this);
		minigameStarted = true;
	}

	public virtual void CompletedMinigame()
	{
		avatar.transform.parent.BroadcastMessage("OnMinigameDone", this);
		completed();
		minigameStarted = false;
		avatar = null;
	}

	public virtual void FailedMinigame()
	{
		avatar.transform.parent.BroadcastMessage("OnMinigameFailed", this);
		failed();
		minigameStarted = false;
		avatar = null;
	}

	public virtual void QuitMinigame()
	{
		minigameStarted = false;
		avatar.transform.parent.BroadcastMessage("OnMinigameAborted", this);
		quit();
		avatar = null;
	}
}
