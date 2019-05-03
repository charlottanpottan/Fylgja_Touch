using UnityEngine;

public class VehicleMoveToPoint : MonoBehaviour
{
	public Vehicle vehicle;

	Vector3 targetPosition;
	Quaternion targetRotation;
	bool isMovingToTarget = false;
	bool hasReachedPosition = false;
	Quaternion desiredRotation;

	public void Start()
	{
	}

	public float DistanceDisregardingHeight()
	{
		Vector3 currentPosition = new Vector3(vehicle.transform.position.x, 0, vehicle.transform.position.z);
		Vector3 targetPositonWithoutY = new Vector3(targetPosition.x, 0, targetPosition.z);

		return (targetPositonWithoutY - currentPosition).magnitude;
	}

	public void Update()
	{
		if (!isMovingToTarget)
		{
			return;
		}
		if (!hasReachedPosition && HasReachedPosition())
		{
			hasReachedPosition = true;
		}
		float desiredSpeed;

		if (hasReachedPosition)
		{
			desiredSpeed = 0;
			desiredRotation = targetRotation;
		}
		else
		{
			float distance = DistanceDisregardingHeight();

			if (distance > 0.2f)
			{
				var delta = targetPosition - vehicle.transform.position;
				desiredRotation = Quaternion.LookRotation(delta);
			}
			desiredSpeed = Mathf.Min(1.0f, distance * distance * 1.0f + 0.01f);
		}

		vehicle.Move(desiredRotation, desiredSpeed);

		if (!IsRotatedToTarget(targetRotation))
		{
//			Debug.Log("Not rotated well enough :(");
		}

		if (!HasReachedPosition())
		{
//			Debug.Log("Not close enough");
		}

		if (isMovingToTarget && hasReachedPosition && IsRotatedToTarget(desiredRotation))
		{
			Debug.Log("We are there!");
			isMovingToTarget = false;
		}
	}


	bool HasReachedPosition()
	{
		float distance = DistanceDisregardingHeight();

		return distance < 0.25f;
	}

	bool IsRotatedToTarget(Quaternion desiredRotation)
	{
		var rotationDiff = Mathf.Abs(Angle.AngleDiff(desiredRotation.eulerAngles.y, vehicle.transform.rotation.eulerAngles.y));
		return rotationDiff < 15f;
	}

	/*
			Debug.Log("VehicleMoveToPoint: New Target position:" + target + " for vehicle:" + vehicle.name);
		targetPosition = target;
		var delta = targetPosition - vehicle.transform.position;
		if (delta.magnitude > 0.001f)
		{
			delta.y = 0;
			delta.Normalize();
			targetRotation = Quaternion.LookRotation(delta);
		}
		if (IsRotatedToTarget(targetRotation) && HasReachedPosition())
		{
			Debug.Log("We are already in position and rotation, so we stay here");
			return;
		}
		isMovingToTarget = true;
		hasReachedPosition = false;
*/
	public void MoveToTarget(Vector3 targetFirstPosition, Quaternion targetFirstRotation)
	{
		SendMessage("OnVehicleWantsToMove", SendMessageOptions.DontRequireReceiver);
		targetRotation = Quaternion.Euler(0, targetFirstRotation.eulerAngles.y, 0);
		targetPosition = targetFirstPosition;

		if (!IsRotatedToTarget(targetRotation))
		{
		}
		if (!HasReachedPosition())
		{
		}
		if (IsRotatedToTarget(targetRotation) && HasReachedPosition())
		{
			return;
		}
		isMovingToTarget = true;
		hasReachedPosition = false;
	}

	public Vector3 TargetPosition()
	{
		return targetPosition;
	}

	public Quaternion TargetRotation()
	{
		return targetRotation;
	}

	public void MoveToTransform(Transform transform)
	{
		Debug.Log("VehicleMoveToPoint: New Target transform:" + transform.position + " for vehicle:" + vehicle.name);
		var targetPosition = transform.position;

		MoveToTarget(targetPosition, targetRotation);
	}

/*
 *      public void OnControllerColliderHit(ControllerColliderHit hit)
 *      {
 *              if (hit.collider.gameObject.layer == Layers.Ground) {
 *                      return;
 *              }
 *              if (isMovingToTarget) {
 *                      Debug.Log("I stop walking because I hit something!");
 *                      ClearTarget();
 *              }
 *      }
 */
	public void ClearTarget()
	{
		isMovingToTarget = false;
	}

	public bool IsWalkingToTarget()
	{
		return isMovingToTarget;
	}
}
