using UnityEngine;
using System.Collections;

[System.Serializable]
public class GraphicsLevel
{
	public QualityLevel onLevel;
	public Behaviour[] componentsToActivate;
}

public class CameraLevelActivator : MonoBehaviour
{
	public GraphicsLevel[] graphicsLevels;

	void Start()
	{
		ChangeQualityLevel();
	}

	void ChangeQualityLevel()
	{
		foreach (var o in graphicsLevels)
		{
			foreach (var foo in o.componentsToActivate)
			{
				foo.enabled = false;
			}
		}
		foreach (var o in graphicsLevels)
		{
			if (QualitySettings.currentLevel >= o.onLevel)
			{
				HandleComponents(o.componentsToActivate);
			}
		}
	}

	void HandleComponents(Behaviour[] toActivate)
	{
		Debug.Log("Initializing Quality Level!");
		foreach (var o in toActivate)
		{
			o.enabled = true;
			Debug.Log(o + " and " + enabled);
		}
	}
}
