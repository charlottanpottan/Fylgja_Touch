using UnityEngine;
using System.Collections;

public class PlayerCameraCollision : LogicCamera
{
	public float sphereSize;
	public LayerMask layerMask;
	public float cameraNearPlane = 0.3f;
	public float moveOutLerpSpeed = 3.0f;
	public float freeDistanceBehindHead = -0.5f;
	float currentPivotDistanceOverride = -1.0f;
	bool overridingPivotDistance = false;
	bool lerpingPivotOut = false;


	void Update()
	{
	}

	public override void UpdateCamera(ref LogicCameraInfo info)
	{
		var freeSpaceBehindHead = new Vector3(0.0f, 0, freeDistanceBehindHead);
		Vector3 spaceBehindHeadPosition = info.CameraPosition() + (info.PivotRotation() * freeSpaceBehindHead);
		Vector3 hitPosition;

		bool collidedBack = CollisionFromHeadToPosition(info.targetPosition, spaceBehindHeadPosition, out hitPosition);

		float targetPivotDistance;

		if (info.cameraSwitched)
		{
			lerpingPivotOut = false;
			overridingPivotDistance = false;
		}


		if (collidedBack)
		{
			targetPivotDistance = (hitPosition - info.targetPosition).magnitude - cameraNearPlane;
			targetPivotDistance = Mathf.Max(targetPivotDistance, 0.0f);
		}
		else
		{
			targetPivotDistance = info.pivotDistance;
		}

		if (overridingPivotDistance)
		{
			if (targetPivotDistance < currentPivotDistanceOverride)
			{
				lerpingPivotOut = false;
				currentPivotDistanceOverride = targetPivotDistance;
				info.pivotDistance = currentPivotDistanceOverride;
			}
			else
			{
				lerpingPivotOut = true;
			}
		}
		else
		{
			if (targetPivotDistance < info.pivotDistance)
			{
				currentPivotDistanceOverride = targetPivotDistance;
				overridingPivotDistance = true;
				info.pivotDistance = currentPivotDistanceOverride;
			}
		}


		if (lerpingPivotOut)
		{
			currentPivotDistanceOverride = Mathf.Lerp(currentPivotDistanceOverride, targetPivotDistance, Time.deltaTime * moveOutLerpSpeed);
			info.pivotDistance = currentPivotDistanceOverride;
			if (Mathf.Abs(currentPivotDistanceOverride - targetPivotDistance) < 0.0005f)
			{
				lerpingPivotOut = false;
				if (!collidedBack)
				{
					overridingPivotDistance = false;
					currentPivotDistanceOverride = -1;
				}
			}
		}
	}
	
	public override void SetCameraPivot(ref LogicCameraInfo cameraInfo, Vector2 targetPivot)
	{
	}

	bool CollisionFromHeadToPosition(Vector3 fromPosition, Vector3 toPosition, out Vector3 hitPosition)
	{
		var hitInfo = new RaycastHit();
		Vector3 direction = (toPosition - fromPosition);
		float checkingDistance = direction.magnitude;
		bool didHitSomething = Physics.SphereCast(fromPosition, sphereSize, direction.normalized, out hitInfo, checkingDistance, layerMask);

		if (didHitSomething)
		{
			Debug.DrawRay(fromPosition, (hitInfo.point - fromPosition), Color.green);
			hitPosition = hitInfo.point;
			return true;
		}
		else
		{
			Debug.DrawRay(fromPosition, direction, Color.white);
		}

		hitPosition = new Vector3();

		return false;
	}
}
