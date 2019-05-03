using UnityEngine;
using System.Collections;

public class PlayerWalkSpeedTrigger : MonoBehaviour {

	private CharacterWalking characterWalking;
	public float targetWalkSpeed = 1;
	private float originalWalkSpeed;
	
	void OnTriggerEnter(Collider collider)
	{	
		if(characterWalking == null)
		{
			AssignCharacterWalking(collider.gameObject);
		}
		originalWalkSpeed = characterWalking.maxWalkSpeed;
		characterWalking.maxWalkSpeed = targetWalkSpeed;
	}
	
	void OnTriggerExit()
	{
		characterWalking.maxWalkSpeed = originalWalkSpeed;
	}
	
	void AssignCharacterWalking(GameObject go)
	{
		characterWalking = go.GetComponent<CharacterWalking>();
	}
}