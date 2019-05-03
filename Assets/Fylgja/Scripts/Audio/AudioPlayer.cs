using UnityEngine;
using System.Collections;

public class AudioPlayer : AudioHandler
{
	public bool destroyAfterPlaying = false;
	public float minTimeInterval = 10;
	public float maxTimeInterval = 30;
	private float playTimer;
	private float intervalTimer;
	private AudioClip intervalClip;
	private bool hasInterval = true;

	public override void Awake()
	{
		if (minTimeInterval == 0 && maxTimeInterval == 0)
		{
			hasInterval = false;
			if(playAutomatically)
			{
				TriggerSound();
				if (destroyAfterPlaying)
				{
					StartCoroutine("PlayThenDestroy");
				}
			}
		}
		else{
			if(playAutomatically)
			{
				StartCoroutine("PlayInterval");	
			}
		}
	}

	void OnTriggerEnter(Collider c)
	{
		// Debug.Log(gameObject.name + " triggered by " + c.name);
		if (!audio.isPlaying && !hasInterval)
		{
			TriggerSound();
			if (destroyAfterPlaying)
			{
				StartCoroutine("PlayThenDestroy");
			}
		}
		if (hasInterval)
		{
			StartCoroutine("PlayInterval");
		}
	}

	IEnumerator PlayInterval()
	{
		playTimer = Random.Range(minTimeInterval, maxTimeInterval);
		intervalClip = PickSound();
		intervalTimer = intervalClip.length + playTimer;
		audio.clip = intervalClip;
		yield return new WaitForSeconds(playTimer);
		audio.Play();
		yield return new WaitForSeconds(intervalTimer);

		StartCoroutine("PlayInterval");
	}

	IEnumerator PlayThenDestroy()
	{
		yield return new WaitForSeconds(audio.clip.length);

		Destroy(gameObject);
	}

	void OnTriggerExit(Collider c)
	{
		// 	Debug.Log(gameObject.name + " stopped playing by " + c.name);
		if (!destroyAfterPlaying && audio.isPlaying)
		{
			audio.Stop();
		}
		StopCoroutine("PlayInterval");
	}
}
