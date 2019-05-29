using UnityEngine;
using System.Collections;

public class DisableOnStart : MonoBehaviour
{
	public MonoBehaviour[] componentsToDisable;
	public GameObject[] objectsToDisable;

	void Start()
	{
		SetEnableComponents(componentsToDisable, false);
		SetEnableObjects(objectsToDisable, false);
	}

	void SetEnableComponents(MonoBehaviour[] objects, bool enabled)
	{
		foreach (var o in objects)
		{
			o.enabled = false;
		}
	}

	void SetEnableObjects(GameObject[] objects, bool enabled)
	{
		foreach (var o in objects)
		{
			o.SetActiveRecursively1(enabled);
		}
	}
}
