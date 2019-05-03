using UnityEngine;
using System.Collections;

public class CharacterHorse : MonoBehaviour {

	public AnimationClip ridingPose;
	Horse horseToControl;
	
	void OnEnterHorse(Horse horse)
	{
		horseToControl = horse;
		horseToControl.gameObject.layer = 8;
	}
	
	void Update()
	{
		if(horseToControl != null)
		{
			RidingIdle();
		}
	}
	
	void RidingIdle()
	{
		if(GetComponent<Animation>().IsPlaying(ridingPose.name))
		{
			return;
		}
		GetComponent<Animation>().CrossFade(ridingPose.name, 0.1f);
	}
	
	void OnLeaveHorse()
	{
		horseToControl.gameObject.layer = 0;
		horseToControl = null;
	}
}
