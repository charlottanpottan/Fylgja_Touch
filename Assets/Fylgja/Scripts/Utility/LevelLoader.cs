using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour
{
	[HideInInspector]
	public AsyncOperation loadLevelAsyncOp;
	LevelId levelToLoadId;

	void Update()
	{
		if (levelToLoadId != null)
		{
			var levelName = levelToLoadId.levelName();

			if (Application.CanStreamedLevelBeLoaded(levelName))
			{
				StartCoroutine(LoadLevelAsync(levelName));
			}
		}
	}

	public void LoadLevel(LevelId id)
	{
		levelToLoadId = id;
	}

	IEnumerator LoadLevelAsync(string levelName)
	{
		levelToLoadId = null;
		Debug.Log("Start async level load:" + levelName);
		loadLevelAsyncOp = Application.LoadLevelAsync(levelName);
		yield return loadLevelAsyncOp;

		Debug.Log("Async Level Load Done:" + levelName);
	}
}
