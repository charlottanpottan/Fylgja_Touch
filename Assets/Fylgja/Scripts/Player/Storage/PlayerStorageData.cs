using System.Collections.Generic;

using System.Xml.Serialization;

public class PlayerStorageDataStartedQuest
{
	public string questId;
	public string questPart;
}


public class PlayerStorageData
{
	public CheckpointId startingCheckpointId;
	public List<int> checkpointsPassed;
	public List<string> inventoryItemsId;

	[XmlArray("startedQuests"),XmlArrayItem("PlayerStorageDataStartedQuest")]
	public List<PlayerStorageDataStartedQuest> startedQuests;

	public List<string> completedQuestIds;
	public List<string> inventoryQuestItemIds;

	public bool subtitlesEnabled;
}
