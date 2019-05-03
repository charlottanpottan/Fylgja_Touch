using UnityEngine;
using System.Collections;

public class FilterContainer : MonoBehaviour
{
	[HideInInspector] public AudioLowPassFilter targetLowPassFilter;
	[HideInInspector] public AudioHighPassFilter targetHighPassFilter;


	public void Start()
	{
		var listenerObject = GameObject.FindGameObjectWithTag("Listener");
		targetLowPassFilter = listenerObject.GetComponent<AudioLowPassFilter>();
		targetHighPassFilter = listenerObject.GetComponent<AudioHighPassFilter>();
	}
}
