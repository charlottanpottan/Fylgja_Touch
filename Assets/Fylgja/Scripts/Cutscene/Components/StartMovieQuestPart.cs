using UnityEngine;

public class StartMovieQuestPart : ActorSceneComponent
{
	public MovieTexture movie;
	public GameObject moviePlayerToSpawn;
	MoviePlayer moviePlayer;

	protected override void Act()
	{
		Debug.Log("StartMovieQuestPart found");
		var moviePrefab = Instantiate(moviePlayerToSpawn, transform.position, transform.rotation) as GameObject;
		moviePlayer = moviePrefab.GetComponent<MoviePlayer>();
		moviePlayer.PlayMovie(actingInScene.GetPlayerNotifications(), movie, OnCutscenePlayed);
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
