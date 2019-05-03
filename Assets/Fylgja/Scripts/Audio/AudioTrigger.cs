using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
	public AudioSource audioSource;
	public AudioClip sound;

	void OnTriggerEnter(Collider collider)
	{
		if (audioSource.clip.name != sound.name)
		{
			audioSource.Stop();
			audioSource.clip = sound;
			audioSource.Play();
		}
	}
}
