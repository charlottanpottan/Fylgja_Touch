using UnityEngine;
using System.Collections;

public class FloatableWaterPlane : MonoBehaviour
{
	public float waterDensity = 1f;
	public float waterDrag = 1.5f;
	public float waterAngularDrag = 1f;
	public Texture2D currents;
	public float currentStrength = 2f;

	private static FloatableWaterPlane s_Instance = null;
	public static FloatableWaterPlane instance {
		get {
			if (s_Instance == null)
			{
				s_Instance = FindObjectOfType(typeof(FloatableWaterPlane)) as FloatableWaterPlane;
				if (s_Instance == null)
				{
					Debug.Log("There's no instance of WaterPlane in the scene");
				}
			}
			return s_Instance;
		}
	}

	void OnApplicationQuit()
	{
		s_Instance = null;
	}
}
