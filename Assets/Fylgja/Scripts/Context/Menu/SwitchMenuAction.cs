using UnityEngine;
using System.Collections;

public class SwitchMenuAction : ActionArbitration
{
	public iTween.EaseType easeType;
	public float transitionTime;
	public GameObject targetCamera;
	public Transform targetTransform;
	public AudioClip clickAudio;
	public AudioHandler audioHandler;
	public float audioVolume = 0.2f;

	public override void ExecuteAction(IAvatar avatar)
	{
		if(clickAudio != null)
		{
			audioHandler.CreateAndPlay(clickAudio, audioVolume);
		}
		else
		{
			audioHandler.CreateAndPlay(audioVolume);
		}
		iTween.Stop(targetCamera);
		iTween.MoveTo(targetCamera, iTween.Hash("easetype", easeType, "time", transitionTime, "position", targetTransform, "ignoretimescale", true));
	}
}
