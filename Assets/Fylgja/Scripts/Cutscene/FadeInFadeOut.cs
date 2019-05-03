using UnityEngine;
using System.Collections;

public class FadeInFadeOut : MonoBehaviour
{
	public delegate void DelegateFadeIn();
	public DelegateFadeIn OnFadeIn;

	public delegate void DelegateFadeOut();
	public DelegateFadeOut OnFadeOut;
	
	private FadeListener fadeListener;


	enum FadeDirection
	{
		Nothing,
		FadeOut,
		FadeIn
	}

	FadeDirection fadeDirection;

	void Awake()
	{
		iTween.CameraFadeAdd();
	}
	
	void Start()
	{
		if(GameObject.FindGameObjectWithTag("Listener") != null)
			fadeListener = GameObject.FindGameObjectWithTag("Listener").GetComponent<FadeListener>();
	}

	Hashtable ht = new Hashtable();

	public void FadeIn(float fadeTime)
	{
		Debug.Log(Time.time + ": FadeIn(" + fadeTime + ")");
		ht.Clear();
		ht.Add("name", "CameraFade");
		ht.Add("amount", 0);
		ht.Add("time", fadeTime);
		ht.Add("easetype", iTween.EaseType.easeOutQuad);
		ht.Add("oncomplete", "OnFadeComplete");
		ht.Add("oncompletetarget", gameObject);
		fadeDirection = FadeInFadeOut.FadeDirection.FadeIn;
		iTween.CameraFadeTo(ht);
	}

	public void FadeOut(float fadeTime)
	{
		Debug.Log(Time.time + ": FadeOut(" + fadeTime + ")");
		ht.Clear();
		ht.Add("name", "CameraFade");
		ht.Add("amount", 1);
		ht.Add("time", fadeTime);
		ht.Add("easetype", iTween.EaseType.easeInQuad);
		ht.Add("oncomplete", "OnFadeComplete");
		ht.Add("oncompletetarget", gameObject);

		fadeDirection = FadeInFadeOut.FadeDirection.FadeOut;
		iTween.CameraFadeTo(ht);
	}
	
	public void SetTargetVolume(float targetVolume)
	{
		fadeListener.SetTargetVolume(targetVolume);
	}

	void OnDestroy()
	{
		Debug.Log("OnDestroy, telling iTween game object is gone");
		iTween.Stop(gameObject);
	}

	public void SetToBlack()
	{
		GUITexture guiT = GameObject.Find("iTween Camera Fade").GetComponent<GUITexture>();
		guiT.color = new Vector4(guiT.color.r, guiT.color.g, guiT.color.b, 1);
	}

	public void SetToTransparent()
	{
		GUITexture guiT = GameObject.Find("iTween Camera Fade").GetComponent<GUITexture>();
		guiT.color = new Vector4(guiT.color.r, guiT.color.g, guiT.color.b, 0);
	}

	void OnFadeInComplete()
	{
		Debug.Log(Time.time + ": FadeInComplete");
		if (OnFadeIn != null)
		{
			OnFadeIn();
		}
	}

	void OnFadeOutComplete()
	{
		Debug.Log(Time.time + ": FadeOutComplete");
		if (OnFadeOut != null)
		{
			OnFadeOut();
		}
	}

	void OnFadeComplete()
	{
		Debug.Log(Time.time + ": FadeCompleted!");

		//DebugUtilities.Assert(fadeDirection != FadeInFadeOut.FadeDirection.Nothing, "Faded for unknown reason!!");
		var doneWithDirection = fadeDirection;

		fadeDirection = FadeInFadeOut.FadeDirection.Nothing;

		switch (doneWithDirection)
		{
		case FadeDirection.FadeIn:
			OnFadeInComplete();
			break;
		case FadeDirection.FadeOut:
			OnFadeOutComplete();
			break;
		default:
			Debug.LogWarning("Faded for unknown reason");
			break;
		}

	}
}