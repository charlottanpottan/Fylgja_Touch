using UnityEngine;
using System.Collections;

public class SpawnGameObject : MonoBehaviour {
	
	public bool instant = true;
	public bool doParent = true;
	public GameObject gameObjectToSpawn;
	
	// Use this for initialization
	void Start () 
	{
	
		if(instant)
		{
			DoSpawn();
		}
		
	}
	
	// Update is called once per frame
	void DoSpawn () 
	{
		// Debug.Log("Spawning: " + gameObjectToSpawn.name);
		GameObject go = GameObject.Instantiate(gameObjectToSpawn) as GameObject;
		if(doParent)
		{
			go.transform.parent = transform;
		}
		
	}
}
