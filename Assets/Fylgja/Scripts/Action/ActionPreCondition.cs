using UnityEngine;

public class ActionPreCondition : MonoBehaviour
{
	public bool mustWalkToAction = true;

	public virtual bool AvatarCanWalkToAction(IAvatar avatar)
	{
		return mustWalkToAction;
	}

	public virtual Transform AvatarInteractTransform(IAvatar avatar)
	{
		var directionToAvatar = (avatar.transform.position - transform.position).normalized;
		var radius = collider.bounds.size.magnitude;
		var newInteractTransform = new GameObject().transform;

		newInteractTransform.position = transform.position + directionToAvatar * radius;
		newInteractTransform.rotation = Quaternion.LookRotation(-directionToAvatar);
		return newInteractTransform;
	}
}
