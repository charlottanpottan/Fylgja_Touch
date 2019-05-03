using UnityEngine;
using System.Collections;

public class QuitGameAction : ActionArbitration
{
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
        Application.Quit();
	}
}
