using UnityEngine;
using System.Collections;

public class LevelCheckActivator : MonoBehaviour {
	
	public string[] levelsToCheck;
	public float checkTime = 2f;
	public GameObject objectToEnable;
	public FadeInFadeOut fader;
	
	// Use this for initialization
	void Start () 
	{
		InvokeRepeating("CheckLevel", checkTime, checkTime);
		fader.SetToBlack();
		fader.FadeIn(checkTime);
	}
	
	// Update is called once per frame
	void CheckLevel () 
	{
		foreach(string level in levelsToCheck)
		{
			if(!Application.CanStreamedLevelBeLoaded(level))
			{
				return;	
			}
		}
		objectToEnable.SetActiveRecursively(true);
		CancelInvoke("CheckLevel");
		
	}
}
