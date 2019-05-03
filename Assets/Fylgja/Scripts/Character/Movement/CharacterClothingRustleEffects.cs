using UnityEngine;

public class CharacterClothingRustleEffects : MonoBehaviour
{
	public AudioHandler walkRustleSounds;
	public CharacterController characterBody;
	float nextRustleTrigger;

	// Use this for initialization
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		if (Time.time >= nextRustleTrigger)
		{
			if (characterBody.velocity.magnitude > 0.4f)
			{
				walkRustleSounds.TriggerSound();
			}
			float magnitude = characterBody.velocity.magnitude * 0.2f;

			float deltaTime = 0.5f;

			if (magnitude > 0.1f)
			{
				deltaTime = magnitude;
			}
			nextRustleTrigger = Time.time + deltaTime;
		}
	}
}

