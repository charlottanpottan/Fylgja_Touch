using UnityEngine;
using System.Collections;

public class RandomPositionAudioTrigger : MonoBehaviour
{
	public AudioHandler audioHandler;
	public float minimumTimeBetweenSounds;
	public float maximumTimeBetweenSounds;
	public float distanceHeightMin;
	public float distanceHeightMax;
	public float distanceWidthMin;
	public float distanceWidthMax;

	float triggerNextSoundAtTime;

	void Start()
	{
		triggerNextSoundAtTime = Time.time + Random.Range(1.0f, 3.0f);
	}

	void Update()
	{
		if (Time.time > triggerNextSoundAtTime)
		{
			var relativeSoundPosition = new Vector3(Random.Range(distanceWidthMin, distanceWidthMax), Random.Range(distanceHeightMin, distanceHeightMax), Random.Range(distanceWidthMin, distanceWidthMax));
			var soundPosition = transform.TransformPoint(relativeSoundPosition);
			audioHandler.TriggerSoundFromPosition(soundPosition);
		}
	}
}

