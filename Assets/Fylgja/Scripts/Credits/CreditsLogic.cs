using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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
                SceneManager.LoadScene("EntryLevel");
			}
			if(canSkip == true && !ownAnimation.IsPlaying(fadeAnimation.name) && Input.GetMouseButtonDown(0))
			{
				ownAnimation.Play(fadeAnimation.name);
			}
		}
	}
}
