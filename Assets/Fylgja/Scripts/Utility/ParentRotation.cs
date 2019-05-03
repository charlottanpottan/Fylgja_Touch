using UnityEngine;
using System.Collections;

public class ParentRotation : MonoBehaviour
{
	// Update is called once per frame
	void LateUpdate()
	{
		if (Camera.main)
		{
			transform.rotation = Camera.main.transform.rotation;
		}
	}
}
