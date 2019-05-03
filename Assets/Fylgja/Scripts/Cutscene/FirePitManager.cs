using UnityEngine;
using System.Collections;

public class FirePitManager : MonoBehaviour {

	private FireTrigger[] firePitTriggers;
	public FirePit[] firePits;
	
	void Start() 
	{	
		firePitTriggers = FindObjectsOfType(typeof(FireTrigger)) as FireTrigger[];
		ActivatePits();
	}
	
	void ActivatePits() 
	{
		foreach(FireTrigger firePitTrigger in firePitTriggers)
		{
			firePitTrigger.ActivateObjects();	
		}
	}
	
	void DeactivatePits() 
	{
		foreach(FireTrigger firePitTrigger in firePitTriggers)
		{
			firePitTrigger.DeactivateObjects();	
		}
		foreach(FirePit firePit in firePits)
		{
			firePit.ResetFire();	
		}
	}
}
