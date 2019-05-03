using UnityEngine;

public struct LogicCameraInfo
{
	public Quaternion targetRotation;
	public bool useTargetPosition;
	public Vector3 targetPosition;
	public Vector3 targetVelocity;
	public Vector3 sourcePosition;
	public float targetMovementFactor;
	public bool useSourcePosition;
	public Vector3 cameraPosition;

	public float pivotOffset;
	public float pivotRotationX;
	public float pivotRotationY;
	public bool pivotRotationIsDefined;
	public float pivotDistance;
	
	public float fov;

	public bool cameraSwitched;
	
	public Quaternion PivotRotation()
	{
		return Quaternion.Euler(new Vector3(pivotRotationX, pivotRotationY, 0.0f));
	}

	public Vector3 CameraPosition()
	{
		const float closeToHeadDistance = 2.0f;
		var closeToHeadFactor = 1.0f - (Mathf.Clamp(pivotDistance, 0, closeToHeadDistance) / closeToHeadDistance);
		closeToHeadFactor *= closeToHeadFactor;
		return targetPosition + (PivotRotation() * new Vector3(0, closeToHeadFactor * 0.9f, -pivotDistance));
	}
}
