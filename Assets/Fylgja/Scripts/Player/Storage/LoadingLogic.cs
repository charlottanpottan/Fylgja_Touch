using UnityEngine;
using System.Collections;

public class LoadingLogic : MonoBehaviour
{
	//public GUIText text;
	public LevelLoader targetLoader;
	//public Renderer targetRenderer;


	void Start()
	{
		targetLoader.LoadLevel(Global.levelId);
	}

//	void LateUpdate()
//	{
//		targetRenderer.material.mainTextureOffset = new Vector2(-1f + Application.GetStreamProgressForLevel(Global.levelId.levelName()), 0);
//		text.text = "Progress: " + Application.GetStreamProgressForLevel(Global.levelId.levelName()) * 100 + "%";
//	}
}
