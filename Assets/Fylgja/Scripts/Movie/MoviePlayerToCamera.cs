using UnityEngine;
using UnityEngine.Video;

public class MoviePlayerToCamera : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Canvas blackBorderCanvas;
    public FadeInFadeOut fadeInFadeOut;

    public delegate void MovieEnd();
    private MovieEnd endFunction;
    
    bool movieIsStarted;
    bool waitingForPlay;
    private FadeListener fadeListener;
    void Start()
    {
        var gameplayCamera = GameObject.FindGameObjectWithTag("GameplayCamera").GetComponent<Camera>();
        videoPlayer.targetCamera = gameplayCamera;
        blackBorderCanvas.worldCamera = gameplayCamera;
    }

    public void PlayMovie(VideoClip videoClip, MovieEnd function)
    {
        if (fadeListener == null)
        {
            fadeListener = GameObject.FindGameObjectWithTag("Listener").GetComponent<FadeListener>();
        }

        var subtitlesPath = $"Subtitles/{videoClip.name}.sv";
        Debug.Log($"loading subtitles from resource '{subtitlesPath}'");
        var textAsset = Resources.Load(subtitlesPath) as TextAsset;
        if (textAsset == null)
        {
            Debug.LogError($"missing subtitles text");
        }

        Debug.Log($"found text:{textAsset.text}");
    
        var subtitlesObject = GameObject.Find("Subtitles");
        Debug.Log($"Found gameobject {subtitlesObject.name}");
        var subtitles = subtitlesObject.GetComponent<Subtitles>();
        subtitles.ShouldBeVisible = true;
        subtitles.OnSubtitleStart(textAsset.text);
        fadeListener.SetTargetVolume(0);
        fadeListener.OnFadeListener(0);

        videoPlayer.clip = videoClip;
        videoPlayer.Play();
        Debug.Log("Play Movie:" + videoPlayer.name);

        endFunction = function;
        waitingForPlay = true;
        Time.timeScale = 0;

        fadeInFadeOut.SetToTransparent(); // SetToBlack();
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
        Destroy(transform.root.gameObject);
    }

    
    void Update()
    {
        videoPlayer.targetCamera = Camera.main;
        blackBorderCanvas.worldCamera = Camera.main;
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

    private void MoviePlayingDone()
    {
        Debug.Log("Movie play stopped");
    }
}
