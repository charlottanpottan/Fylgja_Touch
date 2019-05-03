using UnityEngine;
using System.Collections;

public class AudioHandler : MonoBehaviour
{
	public AudioClip[] clips;
	public bool playAutomatically;

	public virtual void Awake()
	{
		if (playAutomatically)
		{
			TriggerSound();
		}
	}

	void Update()
	{
	}

	public AudioClip PickSound()
	{
		int indexToPlay = Random.Range(0, clips.Length);

		return clips[indexToPlay];
	}

	public void TriggerSound()
	{
		if (clips.Length == 0)
		{
			return;
		}
		int indexToPlay = Random.Range(0, clips.Length);
		GetComponent<AudioSource>().clip = clips[indexToPlay];
		GetComponent<AudioSource>().Play();
	}


	public void TriggerSoundFromPosition(Vector3 position)
	{
		transform.position = position;
		TriggerSound();
	}

	public void StopSound()
	{
		GetComponent<AudioSource>().Stop();
	}
	
	public AudioSource CreateAndPlay(float volume) {
        GameObject go = new GameObject("One shot audio");
        AudioSource source = go.AddComponent<AudioSource>();
		source.bypassEffects = true;
        source.clip = PickSound();
        source.volume = volume;
        source.Play();
        Destroy(go, source.clip.length);
        return source;
    }
	
	public AudioSource CreateAndPlay(AudioClip clip, float volume) {
        GameObject go = new GameObject("One shot audio");
        AudioSource source = go.AddComponent<AudioSource>();
		source.bypassEffects = true;
        source.clip = clip;
        source.volume = volume;
        source.Play();
        Destroy(go, clip.length);
        return source;
    }
}

