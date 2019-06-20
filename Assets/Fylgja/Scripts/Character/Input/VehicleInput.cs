using UnityEngine;

public class VehicleInput : PlayerInput
{
	Vehicle vehicle;

	private Quaternion lastCameraRotation;
	private Vector3 lastCameraForward;
	bool movedLastFrame;

	public void Start()
	{
		FetchCamera();
	}

	public void SetVehicle(Vehicle vehicleToInputTo)
	{
		vehicle = vehicleToInputTo;
	}

	public void OnInput()
	{
		if (!vehicle)
		{
			return;
		}
		Vector3 requestedDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
		requestedDirection = requestedDirection.normalized;

		FetchCamera();

		requestedDirection = lastCameraRotation * requestedDirection;

		Quaternion camToCharacterSpace = Quaternion.FromToRotation(lastCameraForward * -1, transform.up);
		Vector3 requestedCameraRelativeDirection = (camToCharacterSpace * requestedDirection);

		bool wantsToMove = (requestedCameraRelativeDirection.magnitude > .1);

		if (wantsToMove)
		{
			Quaternion inputRotation = Quaternion.LookRotation(requestedCameraRelativeDirection);
			Quaternion requestedRotation = inputRotation;
			Debug.DrawRay(transform.position, requestedCameraRelativeDirection, new Color(1.0f, 1.0f, 0.3f));
			vehicle.Move(requestedRotation, 1.0f);
			movedLastFrame = true;
			var walkToPoint = vehicle.GetComponentInChildren<VehicleMoveToPoint>();
			walkToPoint.ClearTarget();
		}
		else if (movedLastFrame)
		{
			movedLastFrame = false;
			vehicle.StopMoving();
		}

		vehicle.FeedInput();
	}


	private void FetchCamera()
	{
		if (Camera.main == null)
		{
			return;
		}
		lastCameraRotation = Camera.main.transform.rotation;
		lastCameraForward = Camera.main.transform.forward;
	}
}
