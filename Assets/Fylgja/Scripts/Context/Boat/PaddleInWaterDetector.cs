using UnityEngine;

public class PaddleInWaterDetector : MonoBehaviour
{
	bool isInWater;
	float timer;

	void Start()
	{
	}

	void Update()
	{
		timer -= Time.deltaTime;
	}

	void OnTriggerEnter(Collider collider)
	{
		if (timer <= 0)
		{
			isInWater = true;
			SendMessage("OnPaddleInWater");
			timer = 0.2f;
		}
	}

	void OnTriggerExit(Collider collider)
	{
		if (isInWater)
		{
			SendMessage("OnPaddleOutOfWater");
			isInWater = false;
		}
	}
}

