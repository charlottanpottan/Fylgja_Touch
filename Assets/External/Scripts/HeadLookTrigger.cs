using UnityEngine;
using System.Collections;

public class HeadLookTrigger : MonoBehaviour {
	
	public Transform targetTransform;
	public HeadLookControllerImproved targetController;
	[HideInInspector]
	public Transform controllerTransform;
	
	void Start() {
		
		controllerTransform = targetController.transform;
		
		targetController.target = controllerTransform.position + controllerTransform.forward + controllerTransform.up;
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		
		if(targetTransform != null)
			targetController.target = targetTransform.position;
		else
			targetController.target = controllerTransform.position + controllerTransform.forward + controllerTransform.up;
	
	}
	
	void OnTriggerEnter(Collider other) {
		
		
		targetTransform = GameObject.FindWithTag("HeadLookFocus").transform;
		
	}
	
	void OnTriggerExit() {
		
		
		targetTransform = null;
		
		targetController.target = controllerTransform.position + controllerTransform.forward + controllerTransform.up;
		
		
		
	}
}
