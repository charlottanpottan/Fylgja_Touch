using UnityEngine;
using System.Collections;

public class ToggleFullScreenAction : ActionArbitration
{
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
		Screen.fullScreen = !Screen.fullScreen;
		StartCoroutine(ChangeMaterial());
	}

	IEnumerator ChangeMaterial()
	{
		yield return new WaitForSeconds(0.1f);

		if (targetRenderer.material.HasProperty("_BlendRange"))
		{
			if (Screen.fullScreen)
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
