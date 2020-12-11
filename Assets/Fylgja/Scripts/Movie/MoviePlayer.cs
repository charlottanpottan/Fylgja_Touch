using UnityEngine;
using System.Collections;
using UnityEngine.Video;

public class MoviePlayer : MonoBehaviour
{
    public delegate void MovieEnd();
    public FadeInFadeOut fadeInFadeOut;
    public MovieEnd endFunction;

    VideoPlayer videoPlayer;
    FadeListener fadeListener;
    bool movieIsStarted;
    bool waitingForPlay = false;
    AvatarToPlayerNotifications playerNotifications;
    PlayerInteraction.ListenerStackItem listenerHandle;
    RenderTexture renderTexture;

    void Awake()
    {
        renderTexture = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);
        renderTexture.Create();
        GetComponent<AudioSource>().ignoreListenerVolume = true;
    }

    void OnGUI()
    {
        if (Event.current.type.Equals(EventType.Repaint))
            Graphics.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), renderTexture);
    }

    public void PlayMovie(AvatarToPlayerNotifications playerNotification, VideoClip videoClip, MovieEnd function)
    {
        if (fadeListener == null)
        {
            fadeListener = GameObject.FindGameObjectWithTag("Listener").GetComponent<FadeListener>();
        }
        fadeListener.SetTargetVolume(0);
        fadeListener.OnFadeListener(0);
        playerNotifications = playerNotification;
        listenerHandle = playerNotifications.AttachListener(transform);

        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        videoPlayer.clip = videoClip;
        videoPlayer.targetTexture = renderTexture;
        videoPlayer.Play();
        Debug.Log("Play Movie:" + videoPlayer.name);

        endFunction = function;
        waitingForPlay = true;
        Time.timeScale = 0;

        fadeInFadeOut.SetToBlack();
    }

    void OnDestroy()
    {
        renderTexture.Release();
    }

    void Update()
    {
        if (waitingForPlay && videoPlayer.isPlaying)
        {
            waitingForPlay = false;
            movieIsStarted = true;
        }
        if (movieIsStarted && !videoPlayer.isPlaying)
        {
            movieIsStarted = false;
            MoviePlayingDone();
        }
    }

    public void StopMovie()
    {
        if (fadeListener == null)
        {
            fadeListener = GameObject.FindGameObjectWithTag("Listener").GetComponent<FadeListener>();
        }
        fadeListener.OnFadeListener(0);
        Debug.Log("STOP MOVIE!!!!");

        videoPlayer.Stop();

        GetComponent<AudioSource>().Stop();
        movieIsStarted = false;
        Time.timeScale = 1;
        endFunction();

        fadeInFadeOut.SetToBlack();
        playerNotifications.DetachListener(listenerHandle);
        listenerHandle = null;
        Destroy(transform.root.gameObject);
    }

    void MoviePlayingDone()
    {
        Debug.Log("Movie has played:" + videoPlayer.name);

        StopMovie();
    }
}
