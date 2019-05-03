using UnityEngine;
using System.Collections;

public class FadeListener : MonoBehaviour
{
//	public AudioListener audioListener;
//	public iTween.EaseType easeType;
//	public float fadeInTime = 3.0f;
	
	public float fadeSpeed = 3;
	
	float volume;
	float targetVolume;
	
	void Awake()
	{
		OnFadeListener(0);
	}

	void Start()
	{
		targetVolume = 1.0f;
	}
	
	public void SetTargetVolume(float v)
	{
		targetVolume = v;	
	}

	void Update()
	{
//		var delta = targetVolume - volume;
		if (Mathf.Abs(targetVolume - AudioListener.volume) < 0.001f)
		{
			return;
		}
//		volume += delta * Time.fixedDeltaTime;
//		OnFadeListener(volume);
		OnFadeListener(Mathf.Lerp(AudioListener.volume, targetVolume, fadeSpeed * Time.deltaTime));
	}

	public void OnFadeListener(float fadeValue)
	{
		AudioListener.volume = fadeValue;
	}
}
