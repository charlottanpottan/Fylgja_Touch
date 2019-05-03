using UnityEngine;
using System.Collections;

public class ToggleSubtitlesAction : ActionArbitration
{
	public PlayerInteraction playerInteraction;
	public Renderer targetRenderer;
	public AudioHandler audioHandler;
	public float audioVolume = 0.2f;
	public AudioClip clickAudio;

	void Start()
	{
		ChangeMaterial();
	}

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
		Debug.Log("Subtitles: " + playerInteraction.player.playerStorage.ShowSubtitles);
		playerInteraction.player.playerStorage.ShowSubtitles = !playerInteraction.player.playerStorage.ShowSubtitles;
		ChangeMaterial();
	}

	void ChangeMaterial()
	{

		if (targetRenderer.material.HasProperty("_BlendRange"))
		{
			if (playerInteraction.player.playerStorage.ShowSubtitles)
			{
				targetRenderer.material.SetFloat("_BlendRange", 1f);
			}
			else
			{
				targetRenderer.material.SetFloat("_BlendRange", 0f);
			}
		}
	}
}
