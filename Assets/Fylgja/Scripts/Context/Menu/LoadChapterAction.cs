using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadChapterAction : ActionArbitration
{
	public PlayerLevelLoader playerLoader;

	public int targetCheckpoint;
	
	public AudioClip clickAudio;
	public AudioHandler audioHandler;
	public float audioVolume = 0.2f;
	
	public string[] completedQuestIds;
	public string[] currentQuestIds;
	public string[] currentQuestParts;
	public string[] inventoryItems;

	public override void ExecuteAction(IAvatar avatar)
	{
		if(clickAudio != null)
		{
			DontDestroyOnLoad(audioHandler.CreateAndPlay(clickAudio, audioVolume));
		}
		else
		{
			DontDestroyOnLoad(audioHandler.CreateAndPlay(audioVolume));
		}
		
		playerLoader.playerStorage.data.completedQuestIds = new List<string>(completedQuestIds);
		
		playerLoader.playerStorage.data.inventoryQuestItemIds = new List<string>(inventoryItems);

		playerLoader.playerStorage.data.startedQuests = new List<PlayerStorageDataStartedQuest>();
		
//		foreach (var currentQuestId in currentQuestIds)
//		{
//			var startedQuest = new PlayerStorageDataStartedQuest();
//			startedQuest.questId = currentQuestId;
//			startedQuest.questPart = "";
//			playerLoader.playerStorage.data.startedQuests.Add(startedQuest);
//		}
		
		for(int i = 0; i < currentQuestIds.Length; i++)
		{
			var startedQuest = new PlayerStorageDataStartedQuest();
			startedQuest.questId = currentQuestIds[i];
			if(currentQuestParts.Length > i && currentQuestParts[i] != null)
			{
				startedQuest.questPart = currentQuestParts[i];
			}
			else
			{
				startedQuest.questPart = "";	
			}
			playerLoader.playerStorage.data.startedQuests.Add(startedQuest);
		}
		
		playerLoader.ContinueFromCheckpoint(new CheckpointId(targetCheckpoint));
	}
}
