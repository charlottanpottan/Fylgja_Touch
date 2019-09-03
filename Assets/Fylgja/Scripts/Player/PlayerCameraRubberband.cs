using UnityEngine;

public class PlayerCameraRubberband : LogicCamera
{
	float pivotDistance;
	float pivotOffset;

	public float offsetFactor = 0.3f;
	public float distanceFactor = 0.5f;

	float offsetRubberBandStrength = 0.0f;
	float distanceRubberBandStrength = 0.0f;

	void Start()
	{
	}

	void Update()
	{
	}

	public override void UpdateCamera(ref LogicCameraInfo info)
	{
//		info.targetVelocity.y = 0;
//		var cameraRelativeVelocity = Quaternion.Inverse(info.PivotRotation()) * info.targetVelocity;

//		if (info.cameraSwitched)
//		{
//			Debug.Log("Camera SWITCHED!");
//			distanceRubberBandStrength = 0;
//			pivotDistance = 0;
//		}

//		distanceRubberBandStrength += Mathf.Abs(cameraRelativeVelocity.z * Time.deltaTime * 0.05f);
//		distanceRubberBandStrength *= 0.98f;
//		distanceRubberBandStrength = Mathf.Clamp(distanceRubberBandStrength, 0.0f, 1.0f);
////		Debug.Log("RubberBand:" + distanceRubberBandStrength);
//		pivotDistance += (1.0f - distanceRubberBandStrength) * cameraRelativeVelocity.z * Time.deltaTime;
//		pivotDistance *= 0.98f;
//		info.pivotDistance += pivotDistance;


//		float timedDistance = info.pivotDistance * Time.deltaTime;
//		if (timedDistance < 0.0001f)
//		{
//			return;
//		}
//		offsetRubberBandStrength += Mathf.Abs(cameraRelativeVelocity.x / timedDistance);
//		offsetRubberBandStrength *= 0.98f;
//		offsetRubberBandStrength = Mathf.Clamp(offsetRubberBandStrength, 0.0f, 1.0f);
////		Debug.Log("RubberBandOffset:" + offsetRubberBandStrength);

//		pivotOffset -= (1.0f - offsetRubberBandStrength) * cameraRelativeVelocity.x * Time.deltaTime;
//		pivotOffset *= 0.98f;

//		info.pivotOffset += pivotOffset;

//		info.targetMovementFactor = Mathf.Min(Mathf.Min(offsetRubberBandStrength + distanceRubberBandStrength, 1.0f) * info.targetVelocity.magnitude * 0.6f, 1.0f);
////		Debug.Log("Movement:" + info.targetMovementFactor + " offset:" + offsetRubberBandStrength + " distance:" + distanceRubberBandStrength + " velocity:" + info.targetVelocity.magnitude);
	}
	
	public override void SetCameraPivot(ref LogicCameraInfo cameraInfo, Vector2 targetPivot)
	{
	}
}
