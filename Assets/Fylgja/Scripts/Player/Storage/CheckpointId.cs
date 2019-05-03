using System.Xml.Serialization;

public class CheckpointId
{
	public int checkpointId;

	public CheckpointId(int id)
	{
		DebugUtilities.Assert((id >= 0 && id < 16) || id == 99, "Illegal CheckpointId:" + id);
		checkpointId = id;
	}

	CheckpointId()
	{
	}

	public int CheckpointIdValue()
	{
		return checkpointId;
	}
}
