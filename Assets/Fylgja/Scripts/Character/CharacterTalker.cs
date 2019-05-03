using UnityEngine;




public class CharacterTalker : MonoBehaviour
{
	public Transform faceRoot;
	public Animation animationNode;
	public AnimationState talk;
	public AudioSource audioSource;

	public void Start()
	{
	}

	public void Update()
	{
	}

	public void Speak(AudioClip talkAudio, AnimationClip talkAnimation)
	{
		Debug.Log("Speaking '" + talkAnimation.name + "'");
		talk = animationNode[talkAnimation.name];
		talk.AddMixingTransform(faceRoot);
		talk.blendMode = AnimationBlendMode.Blend;
		talk.layer = 2;
		animationNode.CrossFade(talk.name);
		audioSource.PlayOneShot(talkAudio);
	}
}
