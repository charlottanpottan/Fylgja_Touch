using UnityEngine;
using System.Collections;

public class PlayerCamera : LogicCamera
{
	public float cameraSnapRotationFactor = 10.0f;
	public float cameraSnapPositionFactor = 10.0f;
	public float holdCameraStillDistance = 0.5f;
	public LogicCamera logicCamera;

	void Start()
	{
		DebugUtilities.Assert(logicCamera != null, "LogicCamera can not be null for player camera");
	}

    public override void OnCameraSwitch(LogicCameraInfo cameraInfo)
    {
        logicCamera.OnCameraSwitch(cameraInfo);
    }

    public override void UpdateCamera(ref LogicCameraInfo cameraInfo)
	{
		DebugUtilities.Assert(logicCamera != null, "LogicCamera can not be null for player camera");
		if (Time.deltaTime < 0.000001)
		{
			return;
		}

		logicCamera.UpdateCamera(ref cameraInfo);
/*
		var headOffset = new Vector3();
		const float headRadius = 2.0f;
		const float headHeight = 0.8f;
		if (cameraInfo.pivotDistance < headRadius)
		{
			headOffset.y = ((headRadius - cameraInfo.pivotDistance) / headRadius) * headHeight;
		}
		
*/		
	}
	
	public override void SetCameraPivot(ref LogicCameraInfo cameraInfo, Vector2 targetPivot)
	{
		logicCamera.SetCameraPivot(ref cameraInfo, targetPivot);
	}

    public override void SetCameraPivotDistance(ref LogicCameraInfo cameraInfo, float distance, bool instant)
    {
        logicCamera.SetCameraPivotDistance(ref cameraInfo, distance, instant);
    }
}
