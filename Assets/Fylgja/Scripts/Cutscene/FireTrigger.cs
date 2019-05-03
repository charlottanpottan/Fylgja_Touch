using UnityEngine;
using System.Collections;

public class FireTrigger : MonoBehaviour
{
	public GameObject[] objectsToActivate;
	public AnimationClip animationToPlay;

	// Use this for initialization
	void Start()
	{
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("FireTrigger"))
		{
			ActivateObjects();
		}
	}

	public void ActivateObjects()
	{
		foreach (GameObject go in objectsToActivate)
		{
			animation.Play(animationToPlay.name);
			go.active = true;
		}
	}

	public void DeactivateObjects()
	{
		foreach (GameObject go in objectsToActivate)
		{
			go.active = false;
		}
	}
}
