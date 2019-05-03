using UnityEngine;

public class BlacksmithTimer : MonoBehaviour
{
	public AnimationClip timerAnimation;

	void Start()
	{
	}

	void Update()
	{
	}

	public void OnSmithingTimeLeftFactorChanged(float timeLeftFactor)
	{
		SetAnimationAt(timeLeftFactor);
	}

	void SetAnimationAt(float t)
	{
		var anim = animation;

		AnimationState state = anim[timerAnimation.name];

		state.normalizedTime = t;
		state.wrapMode = WrapMode.ClampForever;
		state.speed = 0;
		anim.CrossFade(timerAnimation.name);
	}
}

