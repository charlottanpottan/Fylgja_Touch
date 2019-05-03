using UnityEngine;

public class CameraList : LogicCamera
{
	public LogicCamera[] cameras;

	public override void UpdateCamera(ref LogicCameraInfo info)
	{
		foreach (var logicCamera in cameras)
		{
			logicCamera.UpdateCamera(ref info);
		}
	}
	
	public override void SetCameraPivot(ref LogicCameraInfo info, Vector2 targetPivot)
	{
		foreach(var logicCamera in cameras)
		{
			if(logicCamera is PivotCamera)
			{
				logicCamera.SetCameraPivot(ref info, targetPivot);
			}
		}
	}
}
