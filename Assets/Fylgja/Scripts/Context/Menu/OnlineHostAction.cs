using UnityEngine;
using System.Collections;

public class OnlineHostAction : ActionArbitration
{
	public GameObject serverToSpawn;
	public AudioHandler audioHandler;
	public float audioVolume = 0.2f;
	public AudioClip clickAudio;

	public override void ExecuteAction(IAvatar avatar)
	{
		if(clickAudio != null)
		{
			DontDestroyOnLoad(audioHandler.CreateAndPlay(clickAudio, audioVolume));
		}
		else
		{
			DontDestroyOnLoad(audioHandler.CreateAndPlay(audioVolume));
		}
		Instantiate(serverToSpawn);
	}
}

