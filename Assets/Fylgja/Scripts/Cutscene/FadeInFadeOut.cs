using UnityEngine;
using System.Collections;

public class FadeInFadeOut : MonoBehaviour
{
    public delegate void DelegateFadeIn();
    public DelegateFadeIn OnFadeIn;

    public delegate void DelegateFadeOut();
    public DelegateFadeOut OnFadeOut;

    private FadeListener fadeListener;

    public enum FadeDirection
    {
        Nothing,
        FadeOut,
        FadeIn
    }

    FadeDirection fadeDirection;

    void Awake()
    {
        if(!CameraFader.Instance)
        {
            GameObject go = new GameObject();
            go.name = "CameraFader";
            go.AddComponent<CameraFader>();
        }

        if (GameObject.FindGameObjectWithTag("Listener") != null)
            fadeListener = GameObject.FindGameObjectWithTag("Listener").GetComponent<FadeListener>();
    }

    public void FadeIn(float fadeTime)
    {
        Debug.Log(Time.time + ": FadeIn(" + fadeTime + ")");
        fadeDirection = FadeInFadeOut.FadeDirection.FadeIn;
        CameraFader.Instance.Fade(fadeTime, OnFadeComplete, FadeDirection.FadeIn);
    }

    public void FadeOut(float fadeTime)
    {
        Debug.Log(Time.time + ": FadeOut(" + fadeTime + ")");
        fadeDirection = FadeInFadeOut.FadeDirection.FadeOut;
        CameraFader.Instance.Fade(fadeTime, OnFadeComplete, FadeDirection.FadeOut);
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
        CameraFader.Instance.SetToBlack();
    }

    public void SetToTransparent()
    {
        CameraFader.Instance.SetToTransparent();
    }

    void OnFadeInComplete()
    {
        //Debug.Log(Time.time + ": FadeInComplete");
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
        // Debug.Log(Time.time + ": FadeCompleted!");

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