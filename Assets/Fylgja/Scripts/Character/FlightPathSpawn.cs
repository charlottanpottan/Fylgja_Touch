using UnityEngine;
using System.Collections;

public class FlightPathSpawn : MonoBehaviour {
	
	public bool instant = true;
	public FlightPath flightPathToSpawn;
	public FlightRoute[] routes;
	public bool drawGizmo;
	
	// Use this for initialization
	void Start () 
	{
		if(instant)
		{
			DoSpawnAndParent();
		}
	}
	
	// Update is called once per frame
	public virtual void DoSpawnAndParent () 
	{
		FlightPath fp = GameObject.Instantiate(flightPathToSpawn) as FlightPath;
		fp.transform.parent = transform;
		fp.transform.position = routes[0].pathNodes[0].position;
		fp.transform.rotation = routes[0].pathNodes[0].rotation;
		foreach(FlightRoute route in routes)
		{
			route.targetGameObject = fp.gameObject;
		}
		FlightPathTrigger[] triggers = gameObject.GetComponentsInChildren<FlightPathTrigger>();
		foreach(FlightPathTrigger fpt in triggers)
		{
			fpt.targetFlightPath = fp;
		}
		fp.routes = routes;
		fp.Initialize();
	}
	
	void OnDrawGizmos()
	{
		if (drawGizmo)
		{
			foreach (FlightRoute route in routes)
			{
				iTween.DrawPath(route.pathNodes);
			}
		}
	}
}
