using UnityEngine;
using System.Collections;

public class DebugActivator : MonoBehaviour {

	
	public GameObject[] objectsToActivate;
	
	void Awake()
	{
		SwitchActivationState(false);	
	}
	
	void Update()
	{
		if(Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.Z))
		{
			SwitchActivationState();
		}
	}
	
	void SwitchActivationState()
	{
		foreach(GameObject go in objectsToActivate)
		{
            go.SetActive(!go.activeSelf);
		}
	}
	
	void SwitchActivationState(bool state)
	{
	foreach(GameObject go in objectsToActivate)
		{
            go.SetActive(state);
		}
	}
}
