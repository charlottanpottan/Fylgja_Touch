using UnityEngine;
using System.Collections;

public class NetworkLevel : MonoBehaviour
{
	const int defaultNetworkGroup = 0;
	int levelInstanceNumber;

	void Start()
	{
	}

	void Update()
	{
	}

	public void StartLoadLevel(LevelId id)
	{
		GetComponent<NetworkView>().RPC("LoadLevel", RPCMode.AllBuffered, id.levelIdValue(), levelInstanceNumber + 1);
	}

	[RPC]
	void LoadLevel(int id, int levelInstanceId)
	{
		var levelId = new LevelId(id);

		Debug.Log("Received network command to load level:" + id + " (" + levelId.levelName() + ")  of instance:" + levelInstanceId);
		// It is important to turn off all network communication
		// Otherwise prefabs are spawned in the loading scene instead of the actual scene.
		Network.SetSendingEnabled(defaultNetworkGroup, false);
		Network.isMessageQueueRunning = false;

		levelInstanceNumber = levelInstanceId;
		Network.SetLevelPrefix(levelInstanceNumber);

		Global.levelId = levelId;
		Application.LoadLevel("Loading");
	}

	public void OnLevelLoaded()
	{
		Debug.Log("Level is loaded, continuing network communication");
		Network.isMessageQueueRunning = true;
		Network.SetSendingEnabled(defaultNetworkGroup, true);
	}
}
