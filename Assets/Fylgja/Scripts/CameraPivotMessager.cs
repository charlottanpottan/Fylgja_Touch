using UnityEngine;
using System.Collections;

public class CameraPivotMessager : MonoBehaviour {
	
	LogicCameraInfoApplicator pivotCamera;
	
	// Use this for initialization
	void Start () 
	{
		Init();
	}
	
	void Init()
	{
		pivotCamera = Camera.main.gameObject.GetComponent<LogicCameraInfoApplicator>();
	}
	
	void SetPivot(Vector2 targetPivot)
	{
		pivotCamera.CameraSetPivot(targetPivot);
	}
}
