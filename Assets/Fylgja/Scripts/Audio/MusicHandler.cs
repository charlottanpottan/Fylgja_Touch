using UnityEngine;
using System.Collections;


public class MusicHandler : MonoBehaviour
{
    public AudioSource targetAudioSource;
    public string targetByName;
    public enum EarlyType
    {
        None,
        FadeTo,
        PlayAndFadeTo
    }
    public enum LateType
    {
        None,
        Stop,
        PlayAudio,
        PlayAudioFade,
        Pause
    }
    public bool triggerOnExitInstead = false;
    public MusicHandler pairedHandler;
    public iTween.EaseType easeType = iTween.EaseType.easeInOutSine;
    public EarlyType firstAction;
    public LateType secondAction;
    public bool destroyColliderAfterUse;
    public float fadeToTime;
    public float fadeToVolume;
    public AudioClip playAudioClipToPlay;
    public float playAudioDelay;
    public float playAudioVolume;
    public float playAudioFadeFromVolume;
    public float playAudioFadeTime;

    void Awake()
    {
        if (targetAudioSource == null)
        {
            GameObject audioSource = GameObject.Find(targetByName);
            if (audioSource)
                targetAudioSource = GameObject.Find(targetByName).GetComponent<AudioSource>();
            else
                Debug.LogError("No AudioSource with name " + targetByName + " on GameObject " + name);
        }
    }


    IEnumerator DoChanges()
    {
        if (destroyColliderAfterUse && GetComponent<Collider>())
        {
            Destroy(GetComponent<Collider>());
            Destroy(GetComponent<Rigidbody>());
        }
        if (firstAction >= MusicHandler.EarlyType.FadeTo && targetAudioSource.volume != fadeToVolume)
        {
            //iTween.AudioTo(targetAudioSource.gameObject, fadeToVolume, targetAudioSource.pitch, fadeToTime);
            if (firstAction == MusicHandler.EarlyType.PlayAndFadeTo)
            {
                targetAudioSource.Play();
            }
            iTween.AudioTo(targetAudioSource.gameObject, iTween.Hash("volume", fadeToVolume, "pitch", targetAudioSource.pitch, "time", fadeToTime, "easetype", easeType));
            yield return new WaitForSeconds(fadeToTime);
        }

        switch (secondAction)
        {
            case LateType.PlayAudio:
                yield return new WaitForSeconds(playAudioDelay);

                targetAudioSource.clip = playAudioClipToPlay;
                targetAudioSource.volume = playAudioVolume;
                targetAudioSource.Play();
                break;

            case LateType.PlayAudioFade:
                yield return new WaitForSeconds(playAudioDelay);

                targetAudioSource.clip = playAudioClipToPlay;
                targetAudioSource.volume = playAudioFadeFromVolume;
                targetAudioSource.Play();
                //iTween.AudioTo(targetAudioSource.gameObject, playAudioVolume, targetAudioSource.pitch, playAudioFadeTime);
                iTween.AudioTo(targetAudioSource.gameObject, iTween.Hash("volume", playAudioVolume, "pitch", targetAudioSource.pitch, "time", playAudioFadeTime, "easetype", easeType));
                yield return new WaitForSeconds(playAudioFadeTime);

                break;

            case LateType.Stop:
                targetAudioSource.Stop();
                break;

            case LateType.Pause:
                targetAudioSource.Pause();
                break;
        }
        if (destroyColliderAfterUse)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter()
    {
        if (!triggerOnExitInstead)
        {
            if (pairedHandler)
            {
                pairedHandler.StopAllCoroutines();
            }
            StopAllCoroutines();
            StartCoroutine(DoChanges());
        }
    }

    void OnTriggerExit()
    {
        if (triggerOnExitInstead)
        {
            if (pairedHandler)
            {
                pairedHandler.StopAllCoroutines();
            }
            StopAllCoroutines();
            StartCoroutine(DoChanges());
        }
    }

    void TriggerChanges()
    {
        if (pairedHandler)
        {
            pairedHandler.StopAllCoroutines();
        }
        StartCoroutine(DoChanges());
    }
}
