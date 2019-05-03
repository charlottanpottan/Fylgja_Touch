using UnityEngine;

public class PivotCamera : LogicCamera
{
	public float verticalSpeed = 0.01f;
	public float horizontalSpeed = 0.01f;
	public float distanceSpeed = 0.01f;

	public float minimumVerticalAngle = -10.0f;
	public float maximumVerticalAngle = 55.0f;

	public float minimumDistance = 0.2f;
	public float maximumDistance = 4.0f;

	float rotateX;
	float rotateY;
	float cameraDistance = 2.0f;
	float targetCameraDistance = 2.0f;
	
	public override void SetCameraPivot(ref LogicCameraInfo cameraInfo, Vector2 targetPivot)
	{
		cameraInfo.pivotRotationX = targetPivot.x;
		cameraInfo.pivotRotationY = targetPivot.y;
		cameraInfo.pivotRotationX = Mathf.Clamp(cameraInfo.pivotRotationX, minimumVerticalAngle, maximumVerticalAngle);
	}

	public override void UpdateCamera(ref LogicCameraInfo cameraInfo)
	{
		targetCameraDistance += -Input.GetAxis("camera_distance") * Time.deltaTime * distanceSpeed;
		targetCameraDistance = Mathf.Clamp(targetCameraDistance, minimumDistance, maximumDistance);

		cameraDistance = Mathf.Lerp(cameraDistance, targetCameraDistance, 0.2f);
		cameraInfo.pivotDistance += cameraDistance;

		if (Input.GetButton("wants_to_rotate_camera"))
		{
			float vertical = -Input.GetAxis("camera_vertical");
			float horizontal = Input.GetAxis("camera_horizontal");

			cameraInfo.pivotRotationX += vertical * Time.deltaTime * verticalSpeed;
			cameraInfo.pivotRotationY += horizontal * Time.deltaTime * horizontalSpeed;
			cameraInfo.pivotRotationX = Mathf.Clamp(cameraInfo.pivotRotationX, minimumVerticalAngle, maximumVerticalAngle);
			cameraInfo.pivotRotationIsDefined = true;
		}
	}
}
