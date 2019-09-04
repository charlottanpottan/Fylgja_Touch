using UnityEngine;

public class PlayerCameraAutoRotate : LogicCamera
{
	public float rotateSpeedFactor = 0.25f;
	public float rotateResetTime = 0.5f;
	private float currentRotationTimer = 1f;

	float rotationX = 0;

    //	public override void UpdateCamera(ref LogicCameraInfo info)
    //	{
    //		if (info.pivotRotationIsDefined) {
    //			return;
    //		}
    //		float factor = Mathf.Max(info.targetMovementFactor - 0.5f, 0) * 2.0f;
    //		info.pivotRotationY = Mathf.LerpAngle(info.pivotRotationY, info.targetRotation.eulerAngles.y, Time.deltaTime * rotateSpeedFactor * factor);
    //	}


    public override void OnCameraSwitch(LogicCameraInfo cameraInfo) { }

    //Linus variant för att ge en smidigare övergång från muskontroll till autorotation
    public override void UpdateCamera(ref LogicCameraInfo info)
	{
		//if (info.cameraSwitched)
		//{
		//	info.pivotRotationX = rotationX;
		//	info.pivotRotationY = info.targetRotation.eulerAngles.y;
		//	info.pivotRotationIsDefined = true;
		//}
		//rotationX = info.pivotRotationX;


		//if (info.pivotRotationIsDefined)
		//{
		//	currentRotationTimer = Time.time + rotateResetTime;
		//	return;
		//}
		//if (Time.time < currentRotationTimer)
		//{
		//	return;
		//}
		//float factor = Mathf.Max(info.targetMovementFactor - 0.5f, 0) * 2.0f;
		//info.pivotRotationY = Mathf.LerpAngle(info.pivotRotationY, info.targetRotation.eulerAngles.y, Time.deltaTime * rotateSpeedFactor * factor);
	}
	public override void SetCameraPivot(ref LogicCameraInfo cameraInfo, Vector2 targetPivot)
	{
	}
}
