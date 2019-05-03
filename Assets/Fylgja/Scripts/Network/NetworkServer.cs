using UnityEngine;
using System.Collections;

public class NetworkServer : MonoBehaviour
{
	public NetworkLevel level;

	void Start()
	{
		Network.logLevel = NetworkLogLevel.Full;
		DontDestroyOnLoad(transform.gameObject);
		StartServer();
	}

	void StartServer()
	{
		bool useNat = true;

		Network.InitializeServer(32, 25002, useNat);
		MasterServer.dedicatedServer = true;
		MasterServer.RegisterHost("FylgjaTest", "LinusGame", "Its pretty cool");
		level.StartLoadLevel(new LevelId(0));
	}

	void OnPlayerConnected(NetworkPlayer player)
	{
		Debug.Log("Player connected from " + player.ipAddress + ":" + player.port);
	}

	void OnPlayerDisconnected(NetworkPlayer player)
	{
		Debug.Log("Clean up after player " + player);
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
	}

	void OnServerInitialized()
	{
		Debug.Log("Server is up and running!");
	}
}
