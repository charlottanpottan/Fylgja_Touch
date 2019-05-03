using UnityEngine;
using System.Collections;

public class MarkAsPermanent : MonoBehaviour
{
	
	public int[] destroyOnTheseLevels;
	
	void Start()
	{
		DontDestroyOnLoad(transform.gameObject);
	}
	
	void OnLevelWasLoaded (int level)
	{
		foreach(int l in destroyOnTheseLevels)
		{
			if(l == level)
			{
				Destroy(gameObject);	
			}
		}
	}
}
