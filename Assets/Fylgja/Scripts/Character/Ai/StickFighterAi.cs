using UnityEngine;



public class StickFighterAi : MonoBehaviour
{
	public CharacterOpponentStickFight stickFightCharacter;
	float nextPunchAtTime;
	int numberOfNormalPunchesInARow;
	bool isFighting;

	void Start()
	{
	}

	void Update()
	{
		if (!isFighting)
		{
			return;
		}
		if (Time.time >= nextPunchAtTime && ( stickFightCharacter.GetState() == StickFighterState.BlockingEverything || stickFightCharacter.GetState() == StickFighterState.Blocking) )
		{
			Punch();
		}
	}

	void OnStickFightMinigameStart(StickFightMinigame minigame)
	{
		StartFight();
	}

	void OnStickFightMinigameClose()
	{
		isFighting = false;
	}

	void StartFight()
	{
		isFighting = true;
		nextPunchAtTime = Time.time + 5.0f;
	}

	void Punch()
	{
		int r = Random.Range(0, 100);

		if (numberOfNormalPunchesInARow >= 4 && r <= 30)
		{
			CombinationPunch();
		}
		else
		{
			NormalPunch();
		} nextPunchAtTime = Time.time + 3.0f + Random.Range(0.0f, 2.0f);
	}

	void NormalPunch()
	{
		numberOfNormalPunchesInARow++;
		int side = Random.Range(0, 100);

		if (side >= 50)
		{
			PunchLeft();
		}
		else
		{
			PunchRight();
		}
	}

	void PunchLeft()
	{
		stickFightCharacter.PunchLeft();
	}

	void PunchRight()
	{
		stickFightCharacter.PunchRight();
	}

	void CombinationPunch()
	{
		numberOfNormalPunchesInARow = 0;
		stickFightCharacter.CombinationPunch();
	}
}
