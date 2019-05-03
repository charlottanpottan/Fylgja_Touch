using UnityEngine;

public enum StickFighterState
{
	BlockingEverything,
	Blocking,
	StartingLeftPunch,
	LeftPunch,
	StartingRightPunch,
	RightPunch,
	CombinationPunch,
	Flinching,
	Dizzy,
	Unknown
}

public class CharacterOpponentStickFight : StickFighter
{
	public GameObject stickToActivate;
	public AnimationClip blockPose;
	public AnimationClip blockLeft;
	public AnimationClip blockRight;
	public AnimationClip combinationPunch;
	public AnimationClip dizzyAfterCombination;
	public float blockCrossFadeTime = 0.1f;

	bool combinationPunchInProgress;
	int flinchCountInRow;

	void Start()
	{
	}

	void Update()
	{
		FighterUpdate();
		CheckCombinationPunchDone();
	}

	void OnStickFightMinigameStart(StickFightMinigame minigame)
	{
		InitStickFighter();
		
		stickToActivate.active = true;
		
		animation[blockPose.name].layer = 4;
		animation[blockLeft.name].layer = 5;
		animation[blockRight.name].layer = 5;
		animation[struckLeft.name].layer = 5;
		animation[struckRight.name].layer = 5;
		animation[hitLeft.name].layer = 5;
		animation[hitRight.name].layer = 5;
		animation[combinationPunch.name].layer = 5;
		animation[dizzyAfterCombination.name].layer = 5;
		
		animation.Play(blockPose.name);
		
		base.StickFightMinigameStart(minigame);
	}

	void OnStickFightMinigameClose()
	{
		stickToActivate.active = false;
		animation[blockPose.name].layer = 1;
		Destroy(barAnim.gameObject);
		barAnim = null;
	}

	void CheckCombinationPunchDone()
	{
		if (combinationPunchInProgress && !animation.IsPlaying(combinationPunch.name))
		{
			combinationPunchInProgress = false;
			SendMessage("OnStickCombinationPunchDone");
		}
	}

	public override StickFighterState GetState()
	{
		StickFighterState state = base.GetState();

		if (state == StickFighterState.Unknown)
		{
			if (animation.IsPlaying(combinationPunch.name))
			{
				return StickFighterState.CombinationPunch;
			}
			else if (animation.IsPlaying(dizzyAfterCombination.name))
			{
				return StickFighterState.Dizzy;
			}
			else if (animation.IsPlaying(blockLeft.name) || animation.IsPlaying(blockRight.name))
			{
				return StickFighterState.Blocking;
			}
			else
			{
				return StickFighterState.BlockingEverything;
			}
		}

		return state;
	}

	public void CombinationPunch()
	{
		SendMessage("OnStickCombinationPunchStart");
		animation.CrossFade(combinationPunch.name);
		animation.PlayQueued(dizzyAfterCombination.name);
		combinationPunchInProgress = true;
	}

	public void OnReceivedHitFromAnySide()
	{
		StickFighterState state = GetState();

		if (state == StickFighterState.Flinching)
		{
			flinchCountInRow++;
		}
		else
		{
			flinchCountInRow = 0;
		}
	}

	public override void OnReceivedHitFromLeft()
	{
		OnReceivedHitFromAnySide();
		StickFighterState state = GetState();

		Debug.Log("Opponent received potential hit left: " + state);
		if (state == StickFighterState.StartingRightPunch || (state == StickFighterState.Flinching && flinchCountInRow < 3) || state == StickFighterState.Dizzy)
		{
			ReceivedHitLeft();
		}
		else
		{
			if(state != StickFighterState.StartingLeftPunch && state != StickFighterState.CombinationPunch && state != StickFighterState.LeftPunch && state != StickFighterState.RightPunch)
			{
				animation.CrossFade(blockLeft.name, blockCrossFadeTime);
			}
			SendMessage("OnStickBlockLeft");
		}
	}

	public override void OnReceivedHitFromRight()
	{
		OnReceivedHitFromAnySide();

		StickFighterState state = GetState();

		Debug.Log("Opponent received potential hit right: " + state);
		if (state == StickFighterState.StartingLeftPunch || (state == StickFighterState.Flinching && flinchCountInRow < 3) || state == StickFighterState.Dizzy)
		{
			ReceivedHitRight();
		}
		else
		{
			if(state != StickFighterState.StartingRightPunch && state != StickFighterState.CombinationPunch && state != StickFighterState.LeftPunch && state != StickFighterState.RightPunch)
			{
				animation.CrossFade(blockRight.name, blockCrossFadeTime);
			}
			SendMessage("OnStickBlockRight");
		}
	}

	public override void OnLostAllHealth()
	{
		stickFightGame.OnOpponentLost();
	}
}

