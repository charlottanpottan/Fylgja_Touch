using UnityEngine;
using System.Collections;

public class CharacterVehicle : MonoBehaviour
{
	Vehicle controllingVehicle;
	public CharacterWalking walking;
	public CharacterAvatar avatar;
	
	float originalHeight;
	float originalRadius;
	float originalStepOffset;
	float originalSlopeLimit;
	Vector3 originalTransformOffset = Vector3.zero;

	void Start()
	{
	}

	void OnInput()
	{
		if (controllingVehicle == null)
		{
			return;
		}
		Vector3 requestedDirection = new Vector3(Input.GetAxis("horizontal"), 0, Input.GetAxis("vertical"));
		if (Input.GetButtonDown("interact"))
		{
			OnLeaveVehicle();
			return;
		}
		Quaternion camToVehicleSpace = Camera.main.transform.rotation;
		Vector3 requestedCameraRelativeDirection = (camToVehicleSpace * requestedDirection);
		requestedCameraRelativeDirection.y = 0;
		requestedCameraRelativeDirection.Normalize();
		Debug.DrawRay(transform.position, requestedCameraRelativeDirection, new Color(1.0f, 0.2f, 0.2f, 1.0f));
		if (requestedCameraRelativeDirection.magnitude > 0.1f)
		{
			controllingVehicle.Move(Quaternion.LookRotation(requestedCameraRelativeDirection), 1.0f);
		}
		else
		{
			controllingVehicle.StopMoving();
		}
		

	}
	
	
	void LateUpdate()
	{
		if (controllingVehicle != null && controllingVehicle != walking)
		{
			transform.position = controllingVehicle.seatingTransform.position;
			transform.rotation = controllingVehicle.seatingTransform.rotation;
		}
	}

	void OnEnterVehicle(Vehicle vehicle)
	{
		controllingVehicle = vehicle;

		if (controllingVehicle != walking)
		{
			var controller = GetComponent<CharacterController>();
			originalHeight = controller.height;
			originalRadius = controller.radius;
			originalStepOffset = controller.stepOffset;
			originalSlopeLimit = controller.slopeLimit;
			originalTransformOffset = controller.center;
			Destroy(controller);

			walking.TurnOffLocomotion();
		}

		OnEnterVehicleDone();
	}

	public void SetAllowedToMove(bool allowed)
	{
		controllingVehicle.SetAllowedToMove(allowed);
	}

	void OnEnterVehicleDone()
	{
		controllingVehicle.OnAvatarEnter(avatar);
	}

	void OnLeaveVehicle()
	{
		if (controllingVehicle != walking)
		{
			controllingVehicle.StopMoving();
			DetachCharacterFromVehicle();
		}
		else
		{
			Debug.LogWarning("You can not exit avatar");
		}
	}

	void DetachCharacterFromVehicle()
	{
		transform.parent = null;


		var controller = gameObject.AddComponent<CharacterController>();
		controller.height = originalHeight;
		controller.radius = originalRadius;
		controller.stepOffset = originalStepOffset;
		controller.slopeLimit = originalSlopeLimit;
		controller.center = originalTransformOffset;
		walking.FetchCharacterController();

		if (controllingVehicle != null)
		{
			controllingVehicle.OnAvatarLeave(avatar);
			controllingVehicle = null;
		}
		avatar.BlendToLocomotion();
	}

	public bool IsControllingVehicleOutsideAvatar()
	{
		return controllingVehicle != null && controllingVehicle != walking;
	}
}
