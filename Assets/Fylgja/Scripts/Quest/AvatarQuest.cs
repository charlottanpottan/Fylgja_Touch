using UnityEngine;
using System.Collections.Generic;

public class AvatarQuest : MonoBehaviour
{
	public QuestInventory questInventory;
	public QuestParchment parchment;

	Dictionary<string, QuestProgress> progressForQuests = new Dictionary<string, QuestProgress>();
	List<string> completedQuests = new List<string>();
	List<string> interactableTypesToLookFor = new List<string>();
	List<GameObject> goalObjects = new List<GameObject>();

	public Quest LastParchmentQuest()
	{
		if (progressForQuests.Count == 0)
		{
			return null;
		}

		var e = progressForQuests.GetEnumerator();
		e.MoveNext();
		return e.Current.Value.quest;
	}

	public ActorScene CreateQuest(ActorScene scene)
	{
		var instantiatedCutscene = ActorSceneUtility.CreateSceneWithAvatar(scene.gameObject, GetComponent<IAvatar>());

		var instantiatedQuestObject = instantiatedCutscene as Quest;
		if (instantiatedQuestObject != null)
		{
			ReceiveQuest(instantiatedQuestObject);
		}

		return instantiatedCutscene;
	}

	public ActorScene FetchQuest(string name)
	{
		var questObject = GameObject.Find(name + "(Clone)");
		if (questObject == null)
		{
			return null;
		}

		var actorScene = questObject.GetComponent<ActorScene>();

		return actorScene;
	}


	void ReceiveQuest(Quest quest)
	{
		SetQuestHelper(quest);
	}

	public void SetQuestButDontReport(Quest quest)
	{
		quest.Resume();
		SetQuestHelper(quest);
	}

	void SetQuestHelper(Quest quest)
	{
		Debug.Log("Received a new Quest:" + quest.questName);
		SendMessage("OnStartedQuest", quest.questName);
		var questProgress = new QuestProgress(quest, null);
		questProgress.quest = quest;
		progressForQuests.Add(quest.questName, questProgress);
		quest.endOfSceneNotification += OnCompletedQuest;
		quest.sceneAbortedNotification += OnFailedQuest;
		IAvatar avatar = GetComponentInChildren<CharacterAvatar>();
		quest.activeLineNotification += OnSetupNextLine;
		quest.PlayScene(avatar.playerNotifications);
	}

	public void SetCompletedQuests(ICollection<string> quests)
	{
		completedQuests = new List<string>(quests);
	}

	public void OnSetupNextLine(ActorScene scene, ActorSceneComponent part)
	{
		if (part is ActorCompleteMinigame || part is ActorShowQuestParchment || part is ActorPosition)
		{
			Debug.Log("Note: We are not saving progress for type:" + part.GetType().ToString());
			return;
		}
		var questPartNotification = new QuestPartNotification(scene as Quest, part);
		SendMessage("OnStartedQuestPart", questPartNotification);
	}

	public ActorSceneComponent ActiveQuestPart(string questName)
	{
		QuestProgress questProgress;

		progressForQuests.TryGetValue(questName, out questProgress);

		return questProgress.questPart;
	}


	private void OnFailedQuest(ActorScene failedScene)
	{
		var failedQuest = failedScene as Quest;
		Debug.Log("Sorry, you failed quest '" + failedScene.name + "'. You have to start all over again.");
		SendMessage("OnQuestClosed", failedQuest.questName);
		progressForQuests.Remove(failedQuest.questName);
	}

	private void OnCompletedQuest(ActorScene scene)
	{
		var completedQuest = scene as Quest;
		Debug.Log("Yeahoo: You completed quest:" + completedQuest.name);

		completedQuest.endOfSceneNotification -= OnCompletedQuest;
		SendMessage("OnQuestCompleted", completedQuest.questName);
		SendMessage("OnQuestClosed", completedQuest.questName);
		completedQuests.Add(completedQuest.questName);
		progressForQuests.Remove(completedQuest.questName);
	}

	public ICollection<string> CompletedQuests()
	{
		return completedQuests;
	}

	public ICollection<string> StartedQuests()
	{
		var startedQuests = new List<string>();
		foreach (var questProgressPair in progressForQuests)
		{
			startedQuests.Add(questProgressPair.Key);
		}

		return startedQuests;
	}

	public void OnTriggeredArea(string areaName)
	{
		Debug.Log("Reached area:" + areaName);
		foreach (var questProgressPair in progressForQuests)
		{
			var component = questProgressPair.Value.quest.ActingComponent();
			if (component == null)
			{
				continue;
			}
			component.SendMessage("OnTriggeredArea", areaName, SendMessageOptions.DontRequireReceiver);
		}
	}

	public void OnInteractWith(Interactable interactable)
	{
		Debug.Log("Interacted with:" + interactable.name);
		foreach (var questProgressPair in progressForQuests)
		{
			var component = questProgressPair.Value.quest.ActingComponent();
			if (component == null)
			{
				continue;
			}
			component.SendMessage("OnInteractWith", interactable, SendMessageOptions.DontRequireReceiver);
		}
	}

	public void OnInventoryAdd(string itemName)
	{
		Debug.Log("You picked up: " + itemName);
		foreach (var questProgressPair in progressForQuests)
		{
			var component = questProgressPair.Value.quest.ActingComponent();
			if (component == null)
			{
				continue;
			}
			component.SendMessage("OnInventoryAdd", itemName, SendMessageOptions.DontRequireReceiver);
		}
	}


	public ICollection<string> InteractableTypesToLookFor()
	{
		return interactableTypesToLookFor;
	}

	public bool IsLookingFor(QuestItem item)
	{
		return IsLookingFor(item.name);
	}

	public bool IsLookingFor(Npc npc)
	{
		return IsLookingFor(npc.name) || goalObjects.Contains(npc.gameObject);
	}
	
	public bool IsLookingFor(VehicleEnterArbitration vehicle)
	{
		return IsLookingFor(vehicle.name) || goalObjects.Contains(vehicle.gameObject);
	}

	bool IsLookingFor(string interactableName)
	{
		return interactableTypesToLookFor.Contains(interactableName);
	}

	public void AddInteractableTypeToLookFor(string interactableName)
	{
		interactableTypesToLookFor.Add(interactableName);
	}

	public void RemoveInteractableTypeToLookFor(string interactableName)
	{
		interactableTypesToLookFor.Remove(interactableName);
	}

	public void AddGoalObject(GameObject interactable)
	{
		Debug.Log("Adding goal object:" + interactable.name);
		goalObjects.Add(interactable);
	}

	public void RemoveGoalObject(GameObject interactable)
	{
		Debug.Log("Removing goal object:" + interactable.name);
		goalObjects.Remove(interactable);
	}

	public ICollection<GameObject> GoalObjects()
	{
		return goalObjects;
	}
}
