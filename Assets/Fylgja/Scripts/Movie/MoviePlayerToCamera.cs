using UnityEngine;
using UnityEngine.Video;

public class MoviePlayerToCamera : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Canvas blackBorderCanvas;
    
    PlayerInteraction playerInteraction;
    bool movieIsStarted;
    bool waitingForPlay;
    void Start()
    {
        videoPlayer.targetCamera = Camera.main;
        blackBorderCanvas.worldCamera = Camera.main;
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
