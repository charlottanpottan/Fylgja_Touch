using UnityEngine;
using System.Collections;

public class NewGameAction : ActionArbitration
{
	public PlayerLevelLoader playerLoader;
	public AudioHandler audioHandler;
	public AudioClip clickAudio;
	public float audioVolume = 0.2f;

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
		playerLoader.NewGame();
	}
}