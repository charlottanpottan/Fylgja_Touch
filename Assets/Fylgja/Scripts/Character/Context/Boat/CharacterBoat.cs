using UnityEngine;
using System.Collections;

public class CharacterBoat : MonoBehaviour
{
	public AnimationClip paddleLeft;
	public AnimationClip paddleRight;
	public AnimationClip paddleIdle;
	public AnimationClip paddleSwitchToLeft;
	public AnimationClip paddleSwitchToRight;
	public AnimationClip paddleForward;
	public AnimationClip paddleBackward;

	public Transform paddle;

	Boat boatToControl;
	bool paddleIsInWater;
	float debugPaddleToggleTime;

	void Start()
	{
	}

	void Update()
	{
		if (boatToControl != null)
		{
			UpdatePaddleAnimation();
			DebugUpdate();
		}
	}

	void DebugUpdate()
	{
		if (Time.time > debugPaddleToggleTime)
		{
			if (paddleIsInWater)
			{
				OnPaddleOutsideWater();
			}
			else
			{
				OnPaddleInWater();
			}
			debugPaddleToggleTime = Time.time + 1.0f;
		}
	}

	void UpdatePaddleAnimation()
	{
		if (!boatToControl.IsMoving())
		{
			PaddleIdle();
			return;
		}

		Vector3 forceOrientation = boatToControl.BoatForceDirection();
		if (forceOrientation.z < -0.2f)
		{
			PaddleBackward();
		}
		else if (forceOrientation.x < -0.2f)
		{
			PaddleLeft();
		}
		else if (forceOrientation.x > 0.2f)
		{
			PaddleRight();
		}
		else
		{
			PaddleForward();
		}
	}

	void PaddleIdle()
	{
		if (animation.IsPlaying(paddleIdle.name))
		{
			return;
		}
		animation.CrossFade(paddleIdle.name, 1.0f);
	}

	void PaddleLeft()
	{
		if (animation.IsPlaying(paddleSwitchToLeft.name) || animation.IsPlaying(paddleLeft.name))
		{
			return;
		}
		animation.CrossFade(paddleSwitchToLeft.name);
		animation.PlayQueued(paddleLeft.name);
	}

	void PaddleRight()
	{
		if (animation.IsPlaying(paddleSwitchToRight.name) || animation.IsPlaying(paddleRight.name))
		{
			return;
		}
		animation.CrossFade(paddleSwitchToRight.name);
		animation.PlayQueued(paddleRight.name);
	}

	void PaddleForward()
	{
		animation.CrossFade(paddleForward.name);
		animation[paddleForward.name].speed = 1.0f;
	}

	void PaddleBackward()
	{
		animation.CrossFade(paddleBackward.name);
		animation[paddleBackward.name].speed = -1.0f;
	}

	void OnEnterBoat(Boat boat)
	{
		Debug.Log("CharacterBoat: Entered boat:" + boat.name);
		boatToControl = boat;
		boatToControl.gameObject.layer = 8;
		paddle.gameObject.SetActiveRecursively(true);
	}

	void OnLeaveBoat()
	{
		animation.Stop();
		SendMessage("SetInsideWater", false);
		//boatToControl.gameObject.layer = 0;
		boatToControl = null;
		paddle.gameObject.SetActiveRecursively(false);
	}

	void OnPaddleInWater()
	{
		paddleIsInWater = true;
		boatToControl.SetPaddleInWater(true);
	}

	void OnPaddleOutsideWater()
	{
		paddleIsInWater = false;
		boatToControl.SetPaddleInWater(false);
	}
}

