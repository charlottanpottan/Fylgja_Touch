using UnityEngine;

public class Player : MonoBehaviour
{
	public PlayerStorage playerStorage;
	public AvatarToPlayerNotifications playerNotifications;
	public PlayerInteraction playerInteraction;

	IAvatar avatar;

	void Start()
	{
	}

	void Update()
	{
		if (avatar == null)
		{
			var logic = GameObject.Find("LevelLogic");
			logic.SendMessage("OnPlayerEnter", this);
		}
	}

	public void AssignAvatar(IAvatar avatarToControl)
	{
		Debug.Log("Player: We have an avatar:" + avatarToControl.name);
		avatar = avatarToControl;
		avatar.player = this;
		avatarToControl.playerNotifications = playerNotifications;
	}

	public IAvatar AssignedAvatar()
	{
		return avatar;
	}
}
