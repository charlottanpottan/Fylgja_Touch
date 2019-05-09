using UnityEngine;
using UnityEngine.Video;

public class StartMovieQuestPart : ActorSceneComponent
{
    public VideoPlayer videoPlayer;
    public GameObject moviePlayerToSpawn;

    MoviePlayer moviePlayer;

    protected override void Act()
    {
        Debug.Log("StartMovieQuestPart found");
        GameObject moviePrefab = Instantiate(moviePlayerToSpawn, transform.position, transform.rotation) as GameObject;
        moviePlayer = moviePrefab.GetComponent<MoviePlayer>();
        moviePlayer.PlayMovie(actingInScene.GetPlayerNotifications(), videoPlayer, OnCutscenePlayed);
    }

    public void OnCutscenePlayed()
    {
        Debug.Log("MovieQuestPart says DONE!");
        ComponentDone();
    }

    public override void Skip()
    {
        moviePlayer.StopMovie();
    }

    public override bool CanBeInterrupted()
    {
        return true;
    }
}

