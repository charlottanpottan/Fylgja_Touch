using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CameraFader : MonoBehaviour
{
    public static CameraFader Instance { get { return instance; } }

    [SerializeField] AnimationCurve animationCurve;

    static CameraFader instance;
    static bool fadingDone = true;
    float elapsedFadeTime = 0;
    float fadeDuration = 1;
    UnityAction action;
    FadeInFadeOut.FadeDirection fadeDirection = FadeInFadeOut.FadeDirection.Nothing;

    Image image;
    Color prevColor = Color.black;
    Color targetColor = Color.black;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.Log("There is already a CameraFade in scene. Destroying new one.");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);

        Canvas canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        GameObject go = new GameObject();
        go.name = "Image";
        go.transform.parent = transform;

        image = go.AddComponent<Image>();
        image.rectTransform.anchorMin = new Vector2(0, 0);
        image.rectTransform.anchorMax = new Vector2(1, 1);
        image.rectTransform.pivot = new Vector2(0, 0);
        image.rectTransform.offsetMax = new Vector2(0, 0);
        image.rectTransform.offsetMin = new Vector2(0, 0);

        targetColor.a = 0;
        prevColor = targetColor;
        image.color = targetColor;
    }

    public void Fade(float duration, UnityAction actionIn, FadeInFadeOut.FadeDirection fadeDirectionIn)
    {
        if (fadeDirectionIn == FadeInFadeOut.FadeDirection.FadeOut)
            targetColor.a = 1;
        if (fadeDirectionIn == FadeInFadeOut.FadeDirection.FadeIn)
            targetColor.a = 0;

        elapsedFadeTime = 0;
        action = actionIn;
        fadeDirection = fadeDirectionIn;
        fadeDuration = duration;
        prevColor = image.color;
        fadingDone = false;
    }

    void Update()
    {
        if (fadingDone)
            return;

        float t = 1;
        if(!Mathf.Approximately(fadeDuration, 0))
            t = elapsedFadeTime / fadeDuration;

        t = Mathf.Clamp(t, 0, 1);
        Color c = Color.Lerp(prevColor, targetColor, t);
        image.color = c;

        if (elapsedFadeTime >= fadeDuration)
        {
            elapsedFadeTime = fadeDuration;
            fadingDone = true;
        }

        if (fadingDone)
        {
            action.Invoke();
        }

        elapsedFadeTime += Time.deltaTime;
    }

    public void SetToBlack()
    {
        targetColor.a = 1;
        image.color = targetColor;
        StopFade();
        fadeDirection = FadeInFadeOut.FadeDirection.FadeOut;
    }

    public void SetToTransparent()
    {
        targetColor.a = 0;
        image.color = targetColor;
        StopFade();
        fadeDirection = FadeInFadeOut.FadeDirection.FadeIn;
    }

    void StopFade()
    {
        if (action != null)
            action.Invoke();
        fadingDone = true;
    }
}
