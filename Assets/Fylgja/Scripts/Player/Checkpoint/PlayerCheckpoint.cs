using UnityEngine;
using System.Collections;

public class PlayerCheckpoint : MonoBehaviour
{
	public Transform spawnTransform;
	public string description;
	public int checkpointId;

	void Start()
	{
	}

	void Update()
	{
	}

	public void WarpCharacter(GameObject character)
	{
		character.transform.position = spawnTransform.position;
		character.transform.rotation = spawnTransform.rotation;
	}

	void OnTriggerEnter(Collider collider)
	{
		var avatar = collider.gameObject.GetComponentInChildren<CharacterAvatar>();

		if (avatar == null)
		{
			return;
		}
		avatar.PassedCheckpoint(new CheckpointId(checkpointId));
	}
}
