/* Written for "Dawn of the Tyrant" by SixTimesNothing 
/* Please visit www.sixtimesnothing.com to learn more
/*
/* Note: This code is being released under the Artistic License 2.0
/* Refer to the readme.txt or visit http://www.perlfoundation.org/artistic_license_2_0
/* Basically, you can use this for anything you want but if you plan to change
/* it or redistribute it, you should read the license
*/

using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

[CustomEditor(typeof(WaterToolScript))]

public class WaterToolEditor : Editor 
{
	public GameObject terrainObject;
	public WaterToolScript waterScript;
	public Terrain terComponent;
	public TerrainData terData;
	public float[,] terrainHeights;
	
	public void Awake()
	{
		waterScript = (WaterToolScript)target as WaterToolScript;
		terComponent = (Terrain) waterScript.GetComponent(typeof(Terrain));
		terData = terComponent.terrainData;
		terrainHeights = terData.GetHeights(0, 0, terData.heightmapResolution, terData.heightmapResolution);
	}
	
	public void OnSceneGUI()
	{
		
	}
	
	public override void OnInspectorGUI() 
	{
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		Rect createRiverButton = EditorGUILayout.BeginHorizontal();
		createRiverButton.x = createRiverButton.width / 2 - 100;
		createRiverButton.width = 200;
		createRiverButton.height = 18;
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
					
		if (GUI.Button(createRiverButton, "New River")) 
		{		
			waterScript.CreateRiver();
		}
		
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		
		if (GUI.changed) 
		{
			EditorUtility.SetDirty(waterScript);
		}
	}

	// This is a method that returns a point on the terrain that has been selected with the mouse when pressing a certain key
	public Vector3 GetTerrainCollisionInEditor(Event currentEvent, KeyCode keysCode)
	{
		Vector3 returnCollision = new Vector3();

		Camera SceneCameraReceptor = new Camera();
		
		if(Camera.current != null)
		{
			SceneCameraReceptor = Camera.current;
		
			RaycastHit raycastHit = new RaycastHit();
			
			Vector2 newMousePosition = new Vector2(currentEvent.mousePosition.x, Screen.height - (currentEvent.mousePosition.y + 25));
			
			Ray terrainRay = SceneCameraReceptor.ScreenPointToRay(newMousePosition);
			
			if(Physics.Raycast(terrainRay, out raycastHit, 100000))
			{
				returnCollision = raycastHit.point;
				returnCollision.x = Mathf.RoundToInt((returnCollision.x/terData.size.x) * terData.heightmapResolution);
				returnCollision.y = returnCollision.y/terData.size.y;
				returnCollision.z = Mathf.RoundToInt((returnCollision.z/terData.size.z) *  terData.heightmapResolution);
			}
		}
		
//		Debug.Log(returnCollision);
		
		return returnCollision;
	}

}