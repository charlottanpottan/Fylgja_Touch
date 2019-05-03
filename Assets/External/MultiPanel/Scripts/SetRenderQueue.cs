using UnityEngine;
using System.Collections;

public class SetRenderQueue : MonoBehaviour {

	public int queue = 3000;
	public bool applyToChildren = false;
	
	void Awake () {
		if (!renderer || !renderer.sharedMaterial || applyToChildren){
			if(applyToChildren){
				foreach (Transform child in transform){
					child.renderer.sharedMaterial.renderQueue = queue;
				}
			} else { 
				print("No renderer found on this GameObject. Check the applyToChildren box to apply settings to children " + gameObject.name); 
			} 	 
		} else {
			renderer.sharedMaterial.renderQueue = queue;
		}
	}
}

	