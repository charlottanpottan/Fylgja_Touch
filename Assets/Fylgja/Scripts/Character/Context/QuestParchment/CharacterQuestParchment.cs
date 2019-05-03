using UnityEngine;
using System.Collections;

public class CharacterQuestParchment : MonoBehaviour
{
	public GameObject parchmentToSpawn;
	public PlayerQuestParchment playerQuestParchment;
	private Transform transformToSpawnParchement;
	private Camera parchmentCamera;
	public AvatarQuest avatarQuest;
	public delegate void NotifyOnClose(CharacterQuestParchment parchment);
	public NotifyOnClose notifyOnClose;
	
	void Start()
	{
		transformToSpawnParchement = GameObject.FindGameObjectWithTag("QuestParchmentLocator").transform;
		parchmentCamera = GameObject.FindGameObjectWithTag("QuestParchmentCamera").camera;
	}

	void OnQuestParchmentOpen()
	{
		Debug.Log("CharacterQuestParchment detected open");
		var questParchment = SpawnSpecificParchment(parchmentToSpawn);
		var activeQuest = avatarQuest.LastParchmentQuest();
		questParchment.SetQuest(activeQuest);
	}

	public QuestParchment SpawnSpecificParchment(GameObject parchmentToSpawn)
	{
		var instantiatedParchment = Instantiate(parchmentToSpawn) as GameObject;
		var questParchment = instantiatedParchment.GetComponent<QuestParchment>();

		instantiatedParchment.transform.parent = transformToSpawnParchement;
		instantiatedParchment.transform.localPosition = new Vector3(0, 0, 0);
		instantiatedParchment.transform.localRotation = new Quaternion();

		parchmentCamera.enabled = true;

		return questParchment;
	}

	void OnQuestParchmentClose()
	{
		Debug.Log("CharacterQuestParchment detected close");
		transformToSpawnParchement.BroadcastMessage("OnQuestParchmentClose", SendMessageOptions.DontRequireReceiver);
		playerQuestParchment.SetCanOpenParchmentState(true);
		if (notifyOnClose != null)
		{
			notifyOnClose(this);
		}
	}

	public void Dispose()
	{
		parchmentCamera.enabled = false;
	}
}
