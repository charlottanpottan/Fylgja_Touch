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
		if (GetComponent<Animation>().IsPlaying(paddleIdle.name))
		{
			return;
		}
		GetComponent<Animation>().CrossFade(paddleIdle.name, 1.0f);
	}

	void PaddleLeft()
	{
		if (GetComponent<Animation>().IsPlaying(paddleSwitchToLeft.name) || GetComponent<Animation>().IsPlaying(paddleLeft.name))
		{
			return;
		}
		GetComponent<Animation>().CrossFade(paddleSwitchToLeft.name);
		GetComponent<Animation>().PlayQueued(paddleLeft.name);
	}

	void PaddleRight()
	{
		if (GetComponent<Animation>().IsPlaying(paddleSwitchToRight.name) || GetComponent<Animation>().IsPlaying(paddleRight.name))
		{
			return;
		}
		GetComponent<Animation>().CrossFade(paddleSwitchToRight.name);
		GetComponent<Animation>().PlayQueued(paddleRight.name);
	}

	void PaddleForward()
	{
		GetComponent<Animation>().CrossFade(paddleForward.name);
		GetComponent<Animation>()[paddleForward.name].speed = 1.0f;
	}

	void PaddleBackward()
	{
		GetComponent<Animation>().CrossFade(paddleBackward.name);
		GetComponent<Animation>()[paddleBackward.name].speed = -1.0f;
	}

	void OnEnterBoat(Boat boat)
	{
		Debug.Log("CharacterBoat: Entered boat:" + boat.name);
		boatToControl = boat;
		boatToControl.gameObject.layer = 8;
		paddle.gameObject.SetActiveRecursively1(true);
	}

	void OnLeaveBoat()
	{
		GetComponent<Animation>().Stop();
		SendMessage("SetInsideWater", false);
		//boatToControl.gameObject.layer = 0;
		boatToControl = null;
		paddle.gameObject.SetActiveRecursively1(false);
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

