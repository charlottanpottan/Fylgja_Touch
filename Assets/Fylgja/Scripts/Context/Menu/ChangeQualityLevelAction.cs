using UnityEngine;
using System.Collections;

public class ChangeQualityLevelAction : ActionArbitration
{
	public QualityLevel targetQuality;
	
	public AudioClip clickAudio;
	
	public AudioHandler audioHandler;
	public float audioVolume = 0.2f;

	public Renderer targetRenderer;


	void Awake()
	{
		CheckQualityLevel();
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
		
		QualitySettings.currentLevel = targetQuality;
		Camera.main.SendMessage("ChangeQualityLevel", SendMessageOptions.DontRequireReceiver);
		Debug.Log(QualitySettings.currentLevel);
		transform.parent.BroadcastMessage("CheckQualityLevel");
	}


	public void CheckQualityLevel()
	{
		if (targetRenderer.material.HasProperty("_BlendRange"))
		{
			if (QualitySettings.currentLevel == targetQuality)
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
