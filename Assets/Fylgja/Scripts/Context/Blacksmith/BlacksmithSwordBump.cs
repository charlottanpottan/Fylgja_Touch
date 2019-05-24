using UnityEngine;
using System.Collections;

public class BlacksmithSwordBump : MonoBehaviour
{
	public AnimationClip bumpAnimation;
	public Animation animationOnSword;
    public GameObject star;
	public float maxHealth = 10.0f;
	public float minHealth = 0.0f;
	public int bumpLayer;
	float health;

	void Start()
	{
		health = Random.Range(minHealth, maxHealth);
		UpdateBump();
	}

	void Update()
	{
	}

	public void HitBump(float factor)
	{
		health -= factor;
		health = Mathf.Max(0.0f, health);
		UpdateBump();
	}

	void UpdateBump()
	{
		float t = (health / maxHealth);

		SetAnimation(t);
	}

	void SetAnimation(float t)
	{
		var anim = animationOnSword;

		AnimationState state = anim[bumpAnimation.name];
		
		if(health < 0.1f)
		{
			state.layer = bumpLayer;
			state.normalizedTime = 0;
			state.wrapMode = WrapMode.ClampForever;
			state.speed = 0;
			anim.CrossFade(bumpAnimation.name);
            star.SetActive(false);
        }
		else
		{
			state.layer = bumpLayer;
			state.normalizedTime = t;
			state.wrapMode = WrapMode.ClampForever;
			state.speed = 0;
			anim.CrossFade(bumpAnimation.name);
		}
	}

	public bool IsBumpDone()
	{
		return(health < 0.1f);
	}
}
