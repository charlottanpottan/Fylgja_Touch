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
        Debug.Log("Movie starts. Lets PlayerInteraction know that we are in a form of cutscene");
        playerInteraction = GameObject.FindObjectOfType<PlayerInteraction>();
        playerInteraction.OnCutsceneStart();
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
