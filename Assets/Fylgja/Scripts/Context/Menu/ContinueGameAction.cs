using UnityEngine;
using System.Collections;

public class ContinueGameAction : ActionArbitration
{
	public PlayerLevelLoader playerLoader;
	public MenuManager targetMenuManager;
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
		if (targetMenuManager.entryMenu)
		{
			playerLoader.ContinueGame();
		}
		else
		{
			targetMenuManager.ToggleMenu();
		}
	}
}
