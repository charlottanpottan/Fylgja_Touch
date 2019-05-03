using UnityEngine;
using System.Collections;

public class MouseMenuEffects : MonoBehaviour
{
	public Renderer targetRenderer;
	public float blendRate;
	public AudioHandler audioHandler;
	public float audioVolume = 0.2f;
	public AudioClip mouseOverSound;
	[HideInInspector]
	public bool mouseOver;
	
	void OnMouseEnter()
	{
		if(mouseOverSound != null)
		{
			audioHandler.CreateAndPlay(mouseOverSound, audioVolume);
		}
		else
		{
			audioHandler.CreateAndPlay(audioVolume);
		}
		mouseOver = true;
	}
	
	void OnMouseExit()
	{
		mouseOver = false;
	}

	void LateUpdate()
	{
		if (targetRenderer.material.HasProperty("_BlendRange"))
		{
			if(mouseOver == true)
			{
				targetRenderer.material.SetFloat("_BlendRange", Mathf.Clamp01(targetRenderer.material.GetFloat("_BlendRange") + blendRate));
			}
			else
			{
				targetRenderer.material.SetFloat("_BlendRange", Mathf.Clamp01(targetRenderer.material.GetFloat("_BlendRange") - blendRate));
			}
		}
	}
}
