using UnityEngine;

public class CameraList : LogicCamera
{
	public LogicCamera[] cameras;

    public override void OnCameraSwitch(LogicCameraInfo cameraInfo)
    {
        foreach (var logicCamera in cameras)
        {
            logicCamera.OnCameraSwitch(cameraInfo);
        }
    }

    public override void UpdateCamera(ref LogicCameraInfo cameraInfo)
	{
		foreach (var logicCamera in cameras)
		{
			logicCamera.UpdateCamera(ref cameraInfo);
		}
	}
	
	public override void SetCameraPivot(ref LogicCameraInfo cameraInfo, Vector2 targetPivot)
	{
		foreach(var logicCamera in cameras)
		{
			if(logicCamera is PivotCamera)
			{
				logicCamera.SetCameraPivot(ref cameraInfo, targetPivot);
			}
		}
	}

    public override void SetCameraPivotDistance(ref LogicCameraInfo cameraInfo, float distance)
    {
        foreach (var logicCamera in cameras)
        {
            if (logicCamera)
            {
                logicCamera.SetCameraPivotDistance(ref cameraInfo, distance);
            }
        }
    }
}
