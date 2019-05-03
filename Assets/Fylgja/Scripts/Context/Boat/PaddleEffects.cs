using UnityEngine;

public class PaddleEffects : MonoBehaviour
{
	public AudioHandler paddleInWater;

	void Start()
	{
	}

	void Update()
	{
	}

	void OnPaddleInWater()
	{
		paddleInWater.TriggerSound();
	}

	void OnPaddleOutOfWater()
	{
	}
}
