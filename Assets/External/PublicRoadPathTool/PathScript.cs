/* Written for "Dawn of the Tyrant" by SixTimesNothing 
/* Please visit www.sixtimesnothing.com to learn more
*/

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode()]


public class PathScript : MonoBehaviour 
{
	public void Start()
	{
		
	}
	
	public void NewPath()
	{
		GameObject pathMesh = new GameObject();
		pathMesh.name = "Path";
		pathMesh.AddComponent(typeof(MeshFilter));
		pathMesh.AddComponent(typeof(MeshRenderer));
		pathMesh.AddComponent<AttachedPathScript>();
		
		AttachedPathScript APS = (AttachedPathScript)pathMesh.GetComponent("AttachedPathScript");
		APS.pathMesh = pathMesh;
		APS.parentTerrain = gameObject.name;
		APS.NewPath();
	}
}

	
