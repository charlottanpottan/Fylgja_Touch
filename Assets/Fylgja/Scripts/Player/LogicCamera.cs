using UnityEngine;
using System.Collections;

public abstract class LogicCamera : MonoBehaviour
{
	public abstract void UpdateCamera(ref LogicCameraInfo cameraInfo);
	public abstract void SetCameraPivot(ref LogicCameraInfo cameraInfo, Vector2 targetPivot);
}


