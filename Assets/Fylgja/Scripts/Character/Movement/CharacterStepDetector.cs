using UnityEngine;
using System.Collections;

public class CharacterStepDetector : MonoBehaviour
{
	float timer;
	bool isInsideWater;
	
	bool isEnabled = true;

	void Start()
	{
	}

	void Update()
	{
		if(isEnabled)
		{
			timer -= Time.deltaTime;
		}
	}

	void OnInsideWater()
	{
		isInsideWater = true;
	}

	void OnOutsideWater()
	{
		isInsideWater = false;
	}
	
	public void IsEnabled (bool enableState)
	{
		isEnabled = enableState;
	}

	void OnTriggerEnter(Collider collider)
	{
		if (isEnabled && timer <= 0)
		{
			SendMessage("OnFootStrike", isInsideWater ? "water" : "ground");
			timer = 0.2f;
		}
	}
}

