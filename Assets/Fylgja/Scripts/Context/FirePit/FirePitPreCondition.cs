using UnityEngine;
using System.Collections;

public class FirePitPreCondition : ActionPreCondition
{
	public Transform interactLeft;
	public Transform interactRight;

	public override Transform AvatarInteractTransform(IAvatar avatar)
	{
		return ClosestInteractionTransform(avatar.transform.position);
	}

	public Transform ClosestInteractionTransform(Vector3 position)
	{
		Transform[] interactionTransforms = new Transform[] { interactLeft, interactRight };

		return FindClosestTransform(interactionTransforms, position);
	}

	Transform FindClosestTransform(Transform[] transforms, Vector3 position)
	{
		float closestRange = 9999.0f;
		Transform closestTransform = transforms[0];

		foreach (var transform in transforms)
		{
			float distance = (transform.position - position).magnitude;
			if (distance < closestRange)
			{
				closestTransform = transform;
				closestRange = distance;
			}
		}

		return closestTransform;
	}
}

