using UnityEngine;
using System.Collections;

public class Minimap : MonoBehaviour
{
	public Camera cameraToFollow;
	// Use this for initialization
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		if (!cameraToFollow)
		{
			return;
		}
		Vector3 euler = cameraToFollow.transform.rotation.eulerAngles;
		Vector3 newRotation = new Vector3(0, 0, -euler.y);
		transform.localEulerAngles = newRotation;
	}
}

