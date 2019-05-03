using UnityEngine;
using System.Collections;

public class MinimapCamera : MonoBehaviour
{
	public AvatarQuest objectToFollow;
	public float magicOffset = 0.5f;
	public float magicMultiplier = 4.0f;

	public float lowestY = 10.0f;
	public float highestY = 22.0f;

	public MinimapSurface surface;

	float targetY;
	float y;

	void Start()
	{
		targetY = lowestY;
		y = targetY;
	}

	void Update()
	{
		if (!objectToFollow)
		{
			return;
		}
		Vector3 objectPos = objectToFollow.gameObject.transform.position;

		targetY = CalculateY();
		if (targetY <= 0.0f)
		{
			targetY = highestY;
		}
		targetY = Mathf.Clamp(targetY, lowestY, highestY);

		y += (targetY - y) * Time.deltaTime;

		Vector3 pos = new Vector3(objectPos.x, y, objectPos.z);
		transform.position = pos;
	}

	float CalculateY()
	{
		float radius = MaxDistanceGoalObjects() + magicOffset;
		float y = CameraUtility.CalculateOptimalDistance(GetComponent<Camera>(), radius * magicMultiplier);

		return y;
	}

	float DistanceToObject(Interactable interactable)
	{
		DebugUtilities.Assert(interactable != null, "Must have a valid interactable");
		DebugUtilities.Assert(objectToFollow != null, "Must have a valid object to follow");
		var distance = (interactable.transform.position - objectToFollow.transform.position).magnitude;
		return distance;
	}

	float MaxDistanceGoalObjects()
	{
		var interactables = surface.InteractablesShownOnMap();
		float maxDistance = 0;

		foreach (var interactable in interactables)
		{
			var distance = DistanceToObject(interactable);
			if (distance > maxDistance)
			{
				maxDistance = distance;
			}
		}

		return maxDistance;
	}
}

