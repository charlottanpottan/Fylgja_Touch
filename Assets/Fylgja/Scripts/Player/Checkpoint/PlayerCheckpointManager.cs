using UnityEngine;
using System.Collections;

public class PlayerCheckpointManager : MonoBehaviour
{
	public PlayerCheckpoint[] checkpoints;

	void Start()
	{
	}

	void Update()
	{
	}

	public PlayerCheckpoint PlayerCheckpointFromId(CheckpointId checkpointId)
	{
		int checkpointValue = checkpointId.CheckpointIdValue();

		foreach (var checkpoint in checkpoints)
		{
			if (checkpoint.checkpointId == checkpointValue)
			{
				return checkpoint;
			}
		}
		Debug.LogError("Couldn't find checkpoint with id:" + checkpointValue);
		return null;
	}

	public PlayerCheckpoint DebugFirstCheckpoint()
	{
		return checkpoints[0];
	}
}
