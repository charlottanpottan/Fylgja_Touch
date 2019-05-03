using UnityEngine;
using System.Collections;

public class CreditsLogic : MonoBehaviour {
	
	
	private Animation ownAnimation;
	private bool isInitiated;
	public AnimationClip fadeAnimation;
	public bool canSkip = true;
	
	void Start()
	{
		ownAnimation = GetComponent<Animation>();
		ownAnimation.Play();
		if(GetComponent<AudioSource>() != null)
		{
			GetComponent<AudioSource>().Play();
		}
		isInitiated = true;
	}
	
	void LateUpdate()
	{
		if(isInitiated)
		{
			if(!ownAnimation.isPlaying && Application.CanStreamedLevelBeLoaded("EntryLevel"))
			{
				Application.LoadLevel("EntryLevel");
			}
			if(canSkip == true && !ownAnimation.IsPlaying(fadeAnimation.name) && Input.GetMouseButtonDown(0))
			{
				ownAnimation.Play(fadeAnimation.name);
			}
		}
	}
}
