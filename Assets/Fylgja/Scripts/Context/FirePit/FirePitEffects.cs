using UnityEngine;
using System.Collections;

public class FirePitEffects : MonoBehaviour
{
	public AnimationClip fire;

	public AudioClip normalFlame;
	public AudioClip fullFlame;

	public GameObject bigFan;
	public GameObject smallFan;
	public GameObject fanFailed;
	public GameObject fireDied;
	public GameObject lostFullFlame;
	public GameObject ignite;

	public Transform effectTransform;

	private AnimationState fireState;

	void Start()
	{
		AnimationState state = animation[fire.name];

		state.wrapMode = WrapMode.ClampForever;
		state.weight = 1.0f;
		state.enabled = true;
		state.speed = 0;
		audio.loop = true;
	}

	void Update()
	{
	}

	void OnFirePitStrength(float strength)
	{
		// Debug.Log("Animation Strength:" + strength);
		AnimationState state = animation[fire.name];

		state.normalizedTime = strength;
	}

	void TriggerEffect(GameObject o)
	{
		Debug.Log("Effect: " + o.name);
		Instantiate(o, effectTransform.position, effectTransform.rotation);
	}

	void OnFirePitFanFailed()
	{
		TriggerEffect(fanFailed);
	}

	void OnFirePitBigFan()
	{
		TriggerEffect(bigFan);
	}

	void OnFirePitSmallFan()
	{
		TriggerEffect(smallFan);
	}

	void OnFirePitDied()
	{
		audio.Stop();
		TriggerEffect(fireDied);
	}

	void OnFirePitIgnited()
	{
		Debug.Log("Effect: ignite flame");

		TriggerEffect(ignite);

		audio.clip = normalFlame;
		audio.Play();
	}

	void OnFirePitFullFlame()
	{
		Debug.Log("Effect: full flame");
		audio.clip = fullFlame;
		audio.Play();
	}

	void OnFirePitLostFullFlame()
	{
		Debug.Log("Effect: lost full flame");
		TriggerEffect(lostFullFlame);
	}
	
	void ResetEffects()
	{
		AnimationState state = animation[fire.name];
		state.normalizedTime = 0;
		audio.Stop();
	}
}
