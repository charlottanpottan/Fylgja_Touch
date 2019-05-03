using UnityEngine;
using System.Collections;

public class StickFighter : MonoBehaviour
{
	public int maxHealth = 3 * 3;
	public int takenDamage = 3;
	public AnimationClip hitLeft;
	public AnimationClip hitRight;
	public AnimationClip struckLeft;
	public AnimationClip struckRight;
	public float anticipationDuringPunchTime = 1.0f;
	
	
	public GameObject barObject;
	
	[HideInInspector]
	public Animation barAnim = null;

	protected StickFightMinigame stickFightGame;
	bool punchingLeftIsInProgress;
	bool punchingRightIsInProgress;
	[HideInInspector]
	public int damageStar;
	[HideInInspector]
	public int health;

	public void InitStickFighter()
	{
		Debug.Log("StickFighter start!!");
		health = maxHealth;
		damageStar = 0;
		
		GameObject go = GameObject.Instantiate(barObject) as GameObject;
		barAnim = go.GetComponent<Animation>();
		barAnim[barAnim.clip.name].normalizedSpeed = 0;
		barAnim[barAnim.clip.name].normalizedTime = 0;
	}

	protected void FighterUpdate()
	{
		CheckPunchDone();
	}

	void CheckPunchDone()
	{
		if (punchingLeftIsInProgress && !GetComponent<Animation>().IsPlaying(hitLeft.name))
		{
			PunchLeftIsDone();
		}
		else if (punchingRightIsInProgress && !GetComponent<Animation>().IsPlaying(hitRight.name))
		{
			PunchRightIsDone();
		}
	}

	public void StickFightMinigameStart(StickFightMinigame minigame)
	{
		stickFightGame = minigame;
	}

	public virtual void OnReceivedHitFromLeft()
	{
	}

	public virtual void OnReceivedHitFromRight()
	{
	}

	public virtual StickFighterState GetState()
	{
		if (GetComponent<Animation>().IsPlaying(struckLeft.name) || GetComponent<Animation>().IsPlaying(struckRight.name))
		{
			return StickFighterState.Flinching;
		}
		else if (GetComponent<Animation>().IsPlaying(hitLeft.name))
		{
			if (GetComponent<Animation>()[hitLeft.name].time < anticipationDuringPunchTime)
			{
				return StickFighterState.StartingLeftPunch;
			}
			else
			{
				return StickFighterState.LeftPunch;
			}
		}
		else if (GetComponent<Animation>().IsPlaying(hitRight.name))
		{
			if (GetComponent<Animation>()[hitRight.name].time < anticipationDuringPunchTime)
			{
				return StickFighterState.StartingRightPunch;
			}
			else
			{
				return StickFighterState.RightPunch;
			}
		}
		else
		{
			return StickFighterState.Unknown;
		}
	}

	protected void ReceivedHitLeft()
	{
		GetComponent<Animation>().CrossFade(struckLeft.name);
		ReceivedHit();
		SendMessage("OnStickHitLandedRight", health);
	}

	protected void ReceivedHitRight()
	{
		GetComponent<Animation>().CrossFade(struckRight.name);
		ReceivedHit();
		SendMessage("OnStickHitLandedLeft", health);
	}

	void ReceivedHit()
	{
		health -= takenDamage;
		float h = maxHealth;
		barAnim[barAnim.clip.name].normalizedTime += takenDamage / h;
		if (health == 0)
		{
			OnLostAllHealth();
			return;
		}
		Debug.Log("Fighter received hit. Health:" + health);
		if (health % 3 == 0)
		{
			ReceiveDamageStar();
		}
	}

	protected void ReceiveDamageStar()
	{
		damageStar++;
		Debug.Log("Received new star. Stars:" + damageStar);
		SendMessage("OnStickHealthStarsChanged", damageStar);
	}

	public void PunchLeft()
	{
		Debug.Log("Wants to hit left!");
		GetComponent<Animation>().Play(hitLeft.name);
		punchingLeftIsInProgress = true;
		SendMessage("OnStickPunchLeftStart");
	}

	public void PunchRight()
	{
		Debug.Log("Wants to hit right!");
		GetComponent<Animation>().Play(hitRight.name);
		punchingRightIsInProgress = true;
		SendMessage("OnStickPunchRightStart");
	}

	void PunchLeftIsDone()
	{
		Debug.Log("Punch Left is Done!");
		punchingLeftIsInProgress = false;
		SendMessage("OnStickPunchLeftDone");
	}

	void PunchRightIsDone()
	{
		Debug.Log("Punch Right is Done!");
		punchingRightIsInProgress = false;
		SendMessage("OnStickPunchRightDone");
	}

	public virtual void OnLostAllHealth()
	{
	}
}