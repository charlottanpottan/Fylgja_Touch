using UnityEngine;
using System.Collections;

public class MoviePlayer : MonoBehaviour
{
	private MovieTexture movie;
	public bool fadeInAfter = false;
	public delegate void MovieEnd();
	
	public FadeInFadeOut fadeInFadeOut;
	public MovieEnd endFunction;
	private FadeListener fadeListener;
	bool movieIsStarted;
	AvatarToPlayerNotifications playerNotifications;
	PlayerInteraction.ListenerStackItem listenerHandle;
	
	void Start()
	{
		audio.ignoreListenerVolume = true;
	}

	void OnGUI()
	{
		if (movie.isPlaying)
		{
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), movie);
		}
	}

	public void PlayMovie(AvatarToPlayerNotifications playerNotification, MovieTexture movieToPlay, MovieEnd function)
	{
		if(fadeListener == null)
		{
			fadeListener = GameObject.FindGameObjectWithTag("Listener").GetComponent<FadeListener>();	
		}
		fadeListener.SetTargetVolume(0);
		fadeListener.OnFadeListener(0);
		playerNotifications = playerNotification;
		listenerHandle = playerNotifications.AttachListener(transform);

		movie = movieToPlay;

		Debug.Log("Play Movie:" + movie.name);
		endFunction = function;
		movie.Play();
		audio.clip = movie.audioClip;
		audio.Play();

		movieIsStarted = true;
		Time.timeScale = 0;
		
		iTween.Stop(GameObject.Find("iTween Camera Fade"));
		fadeInFadeOut.SetToBlack();
	}

	void Update()
	{
		if (movieIsStarted && !movie.isPlaying)
		{
			movieIsStarted = false;
			MoviePlayingDone();
		}
	}

	public void StopMovie()
	{
		if(fadeListener == null)
		{
			fadeListener = GameObject.FindGameObjectWithTag("Listener").GetComponent<FadeListener>();	
		}
		if(fadeInAfter)
		{
			fadeListener.SetTargetVolume(1);
		}
		fadeListener.OnFadeListener(0);
		Debug.Log("STOP MOVIE!!!!");
		movie.Stop();
		audio.Stop();
		movieIsStarted = false;
		Time.timeScale = 1;
		endFunction();
		
		iTween.Stop(GameObject.Find("iTween Camera Fade"));
		fadeInFadeOut.SetToBlack();
		playerNotifications.DetachListener(listenerHandle);
		listenerHandle = null;
		Destroy(transform.root.gameObject);
	}


	void MoviePlayingDone()
	{
		Debug.Log("Movie has played:" + movie.name);
		StopMovie();
	}
}
