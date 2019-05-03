using UnityEngine;
using System.Collections;

public class Boat : Vehicle
{
	public float turnFactor = 1.0f;
	public float paddleForce = 5000.0f;
	public float turnForce = 2000.0f;

	Vector3 forceOrientation;
	bool paddleIsInWater;
	bool isMoving;

	public override void OnAvatarEnter(IAvatar avatar)
	{
		Debug.Log("Boat: Avatar Enter");
		avatar.transform.root.BroadcastMessage("OnEnterBoat", this);
	}

	public override void OnAvatarLeave(IAvatar avatar)
	{
		Debug.Log("Boat: Avatar Leave");
		avatar.transform.root.BroadcastMessage("OnLeaveBoat", this);
	}

	public override void SetAllowedToMove(bool allowed)
	{
	}

	public override bool IsAvatar
	{
		get
		{
			return false;
		}
	}

	public void SetPaddleInWater(bool inWater)
	{
		paddleIsInWater = inWater;
	}

	public Vector3 BoatForceDirection()
	{
		return forceOrientation;
	}

	public Vector3 AvatarInput(Quaternion requestedRotation)
	{
		Quaternion boatRotation = transform.rotation;
		Quaternion deltaRotation = Quaternion.Inverse(boatRotation) * requestedRotation;
		Vector3 deltaDirection = deltaRotation * Vector3.forward;
		Vector3 calculatedForceOrientation = new Vector3(-deltaDirection.x, 0.0f, deltaDirection.z);

		return calculatedForceOrientation;
	}

	public override void Move(Quaternion requestedRotation, float speed)
	{
		forceOrientation = AvatarInput(requestedRotation);
		isMoving = true;
	}
	
	void Update()
	{
		if (!paddleIsInWater)
		{
			return;
		}

		Quaternion boatRotation = transform.rotation;
		Vector3 rootPosition = transform.TransformPoint(new Vector3(0.0f, 0.0f, -1.5f));

		Vector3 forceDirection = boatRotation * forceOrientation;
		Vector3 force = forceDirection * turnForce;

		Debug.DrawRay(rootPosition, forceDirection, new Color(0.1f, 1.0f, 1.0f, 1.0f));

		GetComponent<Rigidbody>().AddForceAtPosition(force * Time.deltaTime, rootPosition);
	}

	public override void StopMoving()
	{
		forceOrientation = Vector3.zero;
		isMoving = false;
	}

	public bool IsMoving()
	{
		return isMoving;
	}
}
