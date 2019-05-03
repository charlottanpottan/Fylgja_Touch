using UnityEngine;
using System.Collections;

public class FlightPathTrigger : MonoBehaviour
{
	public int targetIndex;
	public FlightPath targetFlightPath;

	void OnTriggerEnter()
	{
		targetFlightPath.targetIndex = targetIndex;
		if (iTween.Count(targetFlightPath.gameObject) == 0)
		{
			targetFlightPath.FollowRoute();
		}
	}
}
