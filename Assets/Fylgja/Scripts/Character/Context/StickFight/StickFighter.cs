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
		barAnim = go.animation;
		barAnim[barAnim.clip.name].normalizedSpeed = 0;
		barAnim[barAnim.clip.name].normalizedTime = 0;
	}

	protected void FighterUpdate()
	{
		CheckPunchDone();
	}

	void CheckPunchDone()
	{
		if (punchingLeftIsInProgress && !animation.IsPlaying(hitLeft.name))
		{
			PunchLeftIsDone();
		}
		else if (punchingRightIsInProgress && !animation.IsPlaying(hitRight.name))
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
		if (animation.IsPlaying(struckLeft.name) || animation.IsPlaying(struckRight.name))
		{
			return StickFighterState.Flinching;
		}
		else if (animation.IsPlaying(hitLeft.name))
		{
			if (animation[hitLeft.name].time < anticipationDuringPunchTime)
			{
				return StickFighterState.StartingLeftPunch;
			}
			else
			{
				return StickFighterState.LeftPunch;
			}
		}
		else if (animation.IsPlaying(hitRight.name))
		{
			if (animation[hitRight.name].time < anticipationDuringPunchTime)
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
		animation.CrossFade(struckLeft.name);
		ReceivedHit();
		SendMessage("OnStickHitLandedRight", health);
	}

	protected void ReceivedHitRight()
	{
		animation.CrossFade(struckRight.name);
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
		animation.Play(hitLeft.name);
		punchingLeftIsInProgress = true;
		SendMessage("OnStickPunchLeftStart");
	}

	public void PunchRight()
	{
		Debug.Log("Wants to hit right!");
		animation.Play(hitRight.name);
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