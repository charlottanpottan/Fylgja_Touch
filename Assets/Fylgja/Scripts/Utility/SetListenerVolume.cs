using UnityEngine;
using System.Collections;

public class SetListenerVolume : MonoBehaviour {
	
	public float targetVolume = 1;
	
	void Awake()
	{
		AudioListener.volume = targetVolume;	
	}
}
