using UnityEngine;
using System.Collections;

public class InteractOnTrigger : MonoBehaviour
{
	public Interactable interactable;
	private bool canInteract = true;
	
	public IEnumerator Pause(float pauseTime)
	{
		canInteract = false;
		yield return new WaitForSeconds(pauseTime);
		canInteract = true;
	}

	void OnTriggerEnter(Collider collider)
	{
		if(canInteract)
		{
			Debug.Log("Collision:" + name);
			var avatar = collider.GetComponentInChildren<CharacterAvatar>();
			DebugUtilities.Assert(avatar != null, "Please set collision masks. Nothing other than CharacterAvatars should trigger this");
			DebugUtilities.Assert(interactable != null, "You must set up interactable for InteractOnTrigger:" + name);
			avatar.PerformPrimaryAction(interactable.gameObject);
		}
	}
}
