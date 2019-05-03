using UnityEngine;
using System.Collections.Generic;

public class MinimapIcon
{
	public GameObject icon;
	public GameObject targetObject;
}

public class MinimapSurface : MonoBehaviour
{
	public AvatarQuest avatarToFollow;
	public GameObject questMarker;
	public float clampDistanceInMeters = 150.0f;
	public Camera minimapCamera;
	public Transform minimapRotation;
	public float worldToTexelFactor;
	public float minimapBorderRadius;

	IDictionary<GameObject, MinimapIcon> icons = new Dictionary<GameObject, MinimapIcon>();

	void Start()
	{
	}


	public List<Interactable> InteractablesShownOnMap()
	{
		var interactables = new List<Interactable>();

		foreach (var iconPair in icons)
		{
			var o = iconPair.Key;
			if (o == null) {
				continue;
			}
			interactables.Add(o.GetComponentInChildren<Interactable>());
		}
		return interactables;
	}

	void LateUpdate()
	{
		RemoveUninterestingObjects();
		if (!avatarToFollow)
		{
			return;
		}
		FindNewObjectsOfInterest();
	}

	Vector3 WorldToMinimapUiPosition(Vector3 pos)
	{
		var cameraPosition = new Vector3(minimapCamera.transform.position.x, 0, minimapCamera.transform.position.z);
		var iconPosition = new Vector3(pos.x, 0, pos.z);
		var delta = iconPosition - cameraPosition;

		delta.y = delta.z;
		delta.z = 0.0f;

		var rotatedDelta = minimapRotation.TransformDirection(delta);
		var deltaFactor = CameraUtility.CalculateFactor(minimapCamera) * worldToTexelFactor;
		rotatedDelta *= deltaFactor;

		var radius = rotatedDelta.magnitude;
		if (radius > minimapBorderRadius)
		{
			rotatedDelta.Normalize();
			rotatedDelta *= minimapBorderRadius;
		}
		return rotatedDelta;
	}

	void ClampedPosition(MinimapIcon icon)
	{
		var direction = (icon.targetObject.transform.position - avatarToFollow.transform.position);

		direction.y = 0;

		/*
		 * var distance = direction.magnitude;
		 *
		 * var position = direction;
		 *
		 * if (distance > clampDistanceInMeters) {
		 *      direction = direction.normalized;
		 *      position = direction * clampDistanceInMeters;
		 * }
		 */
	}

	void FindNewObjectsOfInterest()
	{
		var interactableTypeNames = avatarToFollow.InteractableTypesToLookFor();

		var colliders = Physics.OverlapSphere(transform.position, 1000.0f, LayerMasks.Interactables);

		foreach (var collider in colliders)
		{
			var interactableComponent = collider.gameObject.GetComponentInChildren<Interactable>();
			DebugUtilities.Assert(interactableComponent != null, "You have an object that is layer interactable but is missing a interactable component:" + collider.gameObject.name);
			if (!interactableTypeNames.Contains(interactableComponent.name))
			{
				continue;
			}
			ShowObject(collider.gameObject);
		}

		var goalObjects = avatarToFollow.GoalObjects();
		foreach (var goalObject in goalObjects)
		{
			DebugUtilities.Assert(goalObject != null, "You have a null interactable as goalObject");
			ShowObject(goalObject.gameObject);
		}
	}

	bool IsInteresting(GameObject targetObject)
	{
		var interactable = targetObject.GetComponentInChildren<Interactable>();

		if (avatarToFollow.InteractableTypesToLookFor().Contains(interactable.name))
		{
			return true;
		}
		var goalObjects = avatarToFollow.GoalObjects();
		foreach (var goalObject in goalObjects)
		{
			if (goalObject == interactable)
			{
				return true;
			}
		}
		return false;
	}

	void RemoveUninterestingObjects()
	{
		var iconsToRemove = new List<MinimapIcon>();

		foreach (var iconPair in icons)
		{
			var icon = iconPair.Value;
			if (icon.targetObject == null || !IsInteresting(icon.targetObject))
			{
				iconsToRemove.Add(icon);
			}
		}

		foreach (var icon in iconsToRemove)
		{
			icons.Remove(icon.targetObject);
			Destroy(icon.icon);
		}
	}

	void ShowObject(GameObject obj)
	{
		MinimapIcon foundIcon = null;

		icons.TryGetValue(obj, out foundIcon);

		if (foundIcon == null)
		{
			foundIcon = SpawnIcon(obj);
			icons.Add(obj, foundIcon);
		}
		var targetPosition = foundIcon.targetObject.transform.position;
		var iconPosition = WorldToMinimapUiPosition(targetPosition);
		foundIcon.icon.transform.localPosition = iconPosition;
		//ClampedPosition(foundIcon);
	}

	MinimapIcon SpawnIcon(GameObject o)
	{
		GameObject iconObject = GameObject.Instantiate(questMarker) as GameObject;

		iconObject.transform.parent = transform;
		iconObject.transform.localPosition = o.transform.position;
		iconObject.transform.localRotation = new Quaternion();

		/*
		 * iconObject.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
		 * iconObject.transform.localRotation = Quaternion.Euler(new Vector3(-90.0f, 0.0f, 0.0f));
		 */

		MinimapIcon newIcon = new MinimapIcon();
		newIcon.icon = iconObject;
		newIcon.targetObject = o;

		return newIcon;
	}
}
