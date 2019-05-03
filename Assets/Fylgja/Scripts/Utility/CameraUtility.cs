using UnityEngine;
using System.Collections;

public class CameraUtility
{
	public static float CalculateOptimalDistance(Camera cameraToUse, float radiusOfObject)
	{
		var fov = cameraToUse.fieldOfView * Mathf.PI / 180.0f;
		var optimalDistance = radiusOfObject * (1.0f / (2.0f * Mathf.Tan(fov / 2.0f))) + cameraToUse.nearClipPlane;

		return optimalDistance;
	}


	public static float CalculateFactor(Camera cameraToUse)
	{
		var fov = cameraToUse.fieldOfView * Mathf.PI / 180.0f;

		return (1.0f / (cameraToUse.transform.position.y - cameraToUse.nearClipPlane)) * (1.0f / (2.0f * Mathf.Tan(fov / 2.0f)));
	}
}
