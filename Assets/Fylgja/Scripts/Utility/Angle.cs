using UnityEngine;
using System.Collections;

public class Angle
{
	
	static float AngleClamp(float angle)
	{
		if (angle > 180.0f)
		{
			angle -= 360.0f;
		}
		else if (angle < -180.0f)
		{
			angle += 360.0f;
		}
		
		return angle;
	}
	
	public static float AngleDiff(float angle1, float angle2)
	{
		return AngleClamp(angle1 - angle2);
	}
	
	public static float AngleMiddle(float angle1, float angle2)
	{
		return angle1 + AngleClamp(angle2 - angle1) / 2.0f;
	}
	
	// Use this for initialization
	void Start ()
	{
		
	}

	// Update is called once per frame
	void Update ()
	{
		
	}
}

