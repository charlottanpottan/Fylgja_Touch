using UnityEngine;
using System.Collections;

public class ActivateOnTrigger : MonoBehaviour
{
	public GameObject[] objectsToActivateRecursively;
	private GameObject[] objectsToActivate;
	public string tagToActivate;
	public bool deactivate;

	void Awake()
	{
		if (tagToActivate.Length >= 1)
		{
			Debug.Log("Initializing tagToActivate:" + tagToActivate);
			objectsToActivate = GameObject.FindGameObjectsWithTag(tagToActivate);
		}
	}

	void OnTriggerEnter()
	{
		if (deactivate)
		{
			foreach (var o in objectsToActivateRecursively)
			{
				o.SetActiveRecursively(false);
			}
			if (tagToActivate.Length >= 1)
			{
				foreach (var o in objectsToActivate)
				{
					o.active = false;
				}
			}
		}
		else
		{
			foreach (var o in objectsToActivateRecursively)
			{
				o.SetActiveRecursively(true);
			}
			if (tagToActivate.Length >= 1)
			{
				foreach (var o in objectsToActivate)
				{
					o.active = true;
				}
			}
		}
	}
}
