using UnityEngine;
using System.Collections;

public class CharacterWaterDetector : MonoBehaviour
{
	bool isInsideWater;

	void Start ()
	{
		
	}

	void Update ()
	{
		
	}

	void SetInsideWater(bool insideWater)
	{
		isInsideWater = insideWater;
		if (isInsideWater)
		{
			BroadcastMessage("OnInsideWater");
		}
		else
		{
			BroadcastMessage("OnOutsideWater");
		}
		Debug.Log(isInsideWater ? " We are inside water!" : " We are outside water!");
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.tag == "Water")
		{
			SetInsideWater(true);
		}
	}

	void OnTriggerExit(Collider collider)
	{
		if (collider.tag == "Water")
		{
			SetInsideWater(false);
		}
	}
}

