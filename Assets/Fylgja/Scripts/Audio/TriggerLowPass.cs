using UnityEngine;
using System.Collections;

public class TriggerLowPass : MonoBehaviour
{
	private AudioLowPassFilter targetLowPassFilter;
	private AudioHighPassFilter targetHighPassFilter;
	public iTween.EaseType easeType;
	public bool fadeLowPass = false;
	public float lowPassTargetValue = 2000;
	public float lowPassStartValue = 22000;
	public float lowPassFadeTime = 1;
	public bool fadeHighPass = false;
	public float highPassTargetValue = 1000;
	public float highPassStartValue = 0;
	public float highPassFadeTime = 1;

	void OnTriggerEnter(Collider other)
	{
		if (targetHighPassFilter == null || targetLowPassFilter == null)
		{
			AssignTargetFilters(other.gameObject);
		}
		iTween.Stop(gameObject);
		if (fadeLowPass && targetLowPassFilter != null)
		{
			targetLowPassFilter.cutoffFrequency = lowPassStartValue;
			targetLowPassFilter.enabled = true;
			iTween.ValueTo(gameObject, iTween.Hash("from", lowPassStartValue, "to", lowPassTargetValue, "time", lowPassFadeTime, "onupdatetarget", gameObject, "onupdate", "FadeLowPass", "easetype", easeType));
		}
		if (fadeHighPass && targetHighPassFilter != null)
		{
			targetHighPassFilter.cutoffFrequency = highPassStartValue;
			targetHighPassFilter.enabled = true;
			iTween.ValueTo(gameObject, iTween.Hash("from", highPassStartValue, "to", highPassTargetValue, "time", highPassFadeTime, "onupdatetarget", gameObject, "onupdate", "FadeHighPass", "easetype", easeType));
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (targetHighPassFilter == null || targetLowPassFilter == null)
		{
			AssignTargetFilters(other.gameObject);
		}

		if (fadeLowPass)
		{
			iTween.ValueTo(gameObject, iTween.Hash("from", lowPassTargetValue, "to", lowPassStartValue, "time", lowPassFadeTime, "onupdatetarget", gameObject, "onupdate", "FadeLowPass", "oncompletetarget", gameObject, "oncomplete", "DeactivateLowPassFilter", "easetype", easeType));
		}
		if (fadeHighPass)
		{
			iTween.ValueTo(gameObject, iTween.Hash("from", highPassTargetValue, "to", highPassStartValue, "time", highPassFadeTime, "onupdatetarget", gameObject, "onupdate", "FadeHighPass", "oncompletetarget", gameObject, "oncomplete", "DeactivateHighPassFilter", "easetype", easeType));
		}
	}

	void AssignTargetFilters(GameObject other)
	{
		FilterContainer filters;

		filters = other.GetComponent<FilterContainer>();

		Debug.Log(other.name);

		if (targetLowPassFilter == null)
		{
			targetLowPassFilter = filters.targetLowPassFilter;
		}
		if (targetHighPassFilter == null)
		{
			targetHighPassFilter = filters.targetHighPassFilter;
		}
	}

	void FadeLowPass(float fadeValue)
	{
		targetLowPassFilter.cutoffFrequency = fadeValue;
	}

	void FadeHighPass(float fadeValue)
	{
		targetHighPassFilter.cutoffFrequency = fadeValue;
	}

	void DeactivateLowPassFilter()
	{
		targetLowPassFilter.enabled = false;
	}

	void DeactivateHighPassFilter()
	{
		targetHighPassFilter.enabled = false;
	}
}
