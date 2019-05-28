using UnityEngine;
using System.Collections;

public enum FylgjaQualityLevel
{
    VeryLow,
    Low,
    Medium,
    High,
    VeryHigh,
    Ultra
}

[System.Serializable]
public class GraphicsLevel
{
	public FylgjaQualityLevel onLevel;
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
			if (QualitySettings.GetQualityLevel() >= (int)o.onLevel)
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
