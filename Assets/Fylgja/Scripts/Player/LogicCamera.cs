using UnityEngine;
using System.Collections;

public abstract class LogicCamera : MonoBehaviour
{
    public abstract void OnCameraSwitch(LogicCameraInfo cameraInfo);
    public abstract void UpdateCamera(ref LogicCameraInfo cameraInfo);
	public abstract void SetCameraPivot(ref LogicCameraInfo cameraInfo, Vector2 targetPivot);
    public abstract void SetCameraPivotDistance(ref LogicCameraInfo cameraInfo, float distance);
}


