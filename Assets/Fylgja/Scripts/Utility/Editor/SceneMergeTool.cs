using UnityEngine;
using UnityEditor;
using System.Collections;

public class SceneMergeTool : MonoBehaviour
{
	[MenuItem("Tools/Load Scene Additive")]

	static void Apply()
	{
		string strScenePath = AssetDatabase.GetAssetPath(Selection.activeObject);

		if (strScenePath == null || !strScenePath.Contains(".unity"))
		{
			EditorUtility.DisplayDialog("Select Scene", "You Must Select a Scene!", "Ok");
			EditorApplication.Beep();
			return;
		}

		Debug.Log("Opening" + strScenePath + " additively");
		EditorApplication.OpenSceneAdditive(strScenePath);
	}
}
