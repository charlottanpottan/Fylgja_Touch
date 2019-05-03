using UnityEngine;

public class ArrivedAtAreaTrigger : MonoBehaviour
{
	public string areaName;

	void OnTriggerEnter(Collider other)
	{
		var avatar = other.transform.root.GetComponentInChildren<IAvatar>();

		DebugUtilities.Assert(avatar != null, "This trigger should only happen for Avatars:" + name);

		var avatarQuest = avatar.GetComponentInChildren<AvatarQuest>();
		if (avatarQuest == null)
		{
			return;
		}

		avatarQuest.OnTriggeredArea(areaName);
	}
}

