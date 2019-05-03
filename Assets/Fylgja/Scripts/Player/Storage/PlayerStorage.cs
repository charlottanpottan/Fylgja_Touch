using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

public class PlayerStorage : MonoBehaviour
{
	public PlayerStorageData data = new PlayerStorageData();
	public string[] debugCompletedQuestIds;
	public string debugCurrentQuest;
	public string debugCurrentQuestPart;
	public int debugCheckpointId = -1;
	const string PlayerPrefsStateName = "state";

	void Awake()
	{
		if (SaveGameExists())
		{
			Load();
		}
		else
		{
			ClearStorageData();
		}
	}


	void ClearStorageData()
	{
		data.startingCheckpointId = new CheckpointId(0);
		data.checkpointsPassed = new List<int>();
		data.checkpointsPassed.Add(0);
		data.inventoryItemsId = new List<string>();
		data.startedQuests = new List<PlayerStorageDataStartedQuest>();
		var startedQuest = new PlayerStorageDataStartedQuest();
		startedQuest.questId = "Quest1TalkToAstrid";
		startedQuest.questPart = "";
		data.startedQuests.Add(startedQuest);
		data.completedQuestIds = new List<string>();
		data.inventoryQuestItemIds = new List<string>();
		// DebugLogOutput("Cleared player profile");
	}

	public void NewGame()
	{
		ClearStorageData();
		Save();
	}

	public void Save()
	{
		// DebugLogOutput("Saved Player Storage");

		var serializer = new XmlSerializer(typeof(PlayerStorageData));
		StringWriter stringWriter = new StringWriter();
		serializer.Serialize(stringWriter, data);
		stringWriter.Close();

		var stateString = stringWriter.ToString();
		Debug.Log("Save:" + stateString);
		PlayerPrefs.SetString(PlayerPrefsStateName, stateString);
	}

	void Load()
	{
		var stateString = PlayerPrefs.GetString(PlayerPrefsStateName);
		Debug.Log("Load:" + stateString);

		var serializer = new XmlSerializer(typeof(PlayerStorageData));
		var stringReader = new StringReader(stateString);
		data = (PlayerStorageData) serializer.Deserialize(stringReader);

		LoadDebugValues();
		// DebugLogOutput("Loaded Player Storage");
	}

	void LoadDebugValues()
	{
		if (debugCompletedQuestIds.Length != 0)
		{
			data.completedQuestIds = new List<string>(debugCompletedQuestIds);
		}

		if (debugCurrentQuest != "")
		{
			data.startedQuests = new List<PlayerStorageDataStartedQuest>();
			var debugQuest = new PlayerStorageDataStartedQuest();
			debugQuest.questId = debugCurrentQuest;
			debugQuest.questPart = debugCurrentQuestPart;
			data.startedQuests.Add(debugQuest);
		}

		if (debugCheckpointId != -1)
		{
			data.startingCheckpointId = new CheckpointId(debugCheckpointId);
		}
	}

	void DebugLogOutput(string description)
	{
		Debug.Log("*** " + description + " ***");
		Debug.Log("+ startingCheckpoint:" + data.startingCheckpointId.CheckpointIdValue());

		foreach (var startedQuest in data.startedQuests)
		{
			Debug.Log(" active:" + startedQuest.questId + " part:'" + startedQuest.questPart + "'");
		}

		foreach (var questId in data.completedQuestIds)
		{
			Debug.Log("+ completed:" + questId);
		}

		Debug.Log("**************************");
	}


	public bool SetStartCheckpoint(CheckpointId id)
	{
		data.startingCheckpointId = id;
		bool visitedBefore = HasVisitedCheckpoint(id);
		if (!visitedBefore)
		{
			data.checkpointsPassed.Add(id.CheckpointIdValue());
		}
		Save();
		return !visitedBefore;
	}

	public bool ShowSubtitles
	{
		set
		{
			data.subtitlesEnabled = value;
			Save();
		}

		get
		{
			return data.subtitlesEnabled;
		}
	}

	PlayerStorageDataStartedQuest FindStartedQuest(string questName)
	{
		foreach (var startedQuest in data.startedQuests)
		{
			if (startedQuest.questId == questName)
			{
				return startedQuest;
			}
		}

		return null;
	}

	bool HasStartedQuest(string questName)
	{
		var questInfo = FindStartedQuest(questName);

		return questInfo != null;
	}

	public void AddStartedQuest(string questName)
	{
		Debug.Log("ADDING QUEST:" + questName);
		if (HasStartedQuest(questName))
		{
			return;
		}
		var startedQuest = new PlayerStorageDataStartedQuest();
		startedQuest.questId = questName;
		startedQuest.questPart = "";
		data.startedQuests.Add(startedQuest);
	}

	public void RemoveStartedQuest(string questName)
	{
		foreach (var startedQuest in data.startedQuests)
		{
			if (startedQuest.questId == questName)
			{
				data.startedQuests.Remove(startedQuest);
				return;
			}
		}
	}

	public void SetStartedQuestPart(string questName, string questPartName)
	{
		DebugUtilities.Assert(questName != "", "QUEST NAME CAN NOT BE EMPTY");
		var questInfo = FindStartedQuest(questName);
		DebugUtilities.Assert(questInfo !=  null, "Couldn't find quest:" + questName);
		questInfo.questPart = questPartName;
		Save();
	}


	public void AddInventoryQuestItem(string name)
	{
		data.inventoryQuestItemIds.Add(name);
		Save();
	}

	public void RemoveInventoryQuestItem(string name)
	{
		data.inventoryQuestItemIds.Remove(name);
		Save();
	}

	public void CompletedQuest(string questId)
	{
		DebugUtilities.Assert(questId.Trim().Length != 0, "QuestId is empty!");
		if (data.completedQuestIds.Contains(questId))
		{
			Debug.LogWarning("You are trying to add a completed quest to a profile that already has done it. Strange");
		}
		else
		{
			data.completedQuestIds.Add(questId);
		}
		var startedQuest = FindStartedQuest(questId);
		DebugUtilities.Assert(startedQuest != null, " You can not complete '" + questId + "' since you haven't started it");
		data.startedQuests.Remove(startedQuest);
		Debug.Log("PlayerStorage understood that you completed quest: " + questId);
		Save();
	}

	public LevelId LevelIdFromCheckpoint(CheckpointId id)
	{
		if (id.CheckpointIdValue() == 99)
		{
			return new LevelId(2);
		}
		return new LevelId(id.CheckpointIdValue() > 3 ? 1 : 0);
	}

	public bool HasVisitedCheckpoint(CheckpointId id)
	{
		return data.checkpointsPassed.Contains(id.CheckpointIdValue());
	}

	public bool SaveGameExists()
	{
		return PlayerPrefs.HasKey(PlayerPrefsStateName);
	}

	public PlayerStorageData playerData()
	{
		return data;
	}
}
