using UnityEngine;
using System.Collections;

public class NetworkClient : MonoBehaviour
{
	void Start()
	{
		Network.logLevel = NetworkLogLevel.Full;
		DontDestroyOnLoad(transform.gameObject);
		StartClient();
	}

	public void StartClient()
	{
		Debug.Log("Starting Network Client!");
		ConnectToMasterServer();
	}

	void Update()
	{
		HostData selectedHost = null;

		if (MasterServer.PollHostList().Length != 0)
		{
			HostData[] hostData = MasterServer.PollHostList();
			for (int i = 0; i < hostData.Length; ++i)
			{
				if (selectedHost == null)
				{
					selectedHost = hostData[i];
				}
			}
			MasterServer.ClearHostList();
			ConnectToServer(selectedHost);
		}
	}

	void ConnectToServer(HostData host)
	{
		DebugOutputHostData(host);
		Network.Connect(host);
	}

	void DebugOutputHostData(HostData host)
	{
		Debug.Log(" Host:" + host.gameName);
		Debug.Log(" useNAT:" + host.useNat);
		Debug.Log(" IP:" + host.ip);
		Debug.Log(" PORT:" + host.port);
		Debug.Log(" players:" + host.connectedPlayers);
		Debug.Log(" comment:" + host.comment);
	}

	void ConnectToMasterServer()
	{
		Debug.Log("Connecting to master server!");
		MasterServer.ClearHostList();
		MasterServer.RequestHostList("FylgjaTest");
	}

	void OnFailedToConnect(NetworkConnectionError error)
	{
		Debug.Log("Could not connect to server: " + error);
	}

	void OnFailedToConnectToMasterServer(NetworkConnectionError info)
	{
		Debug.Log("Could not connect to master server: " + info);
	}

	void OnConnectedToServer()
	{
		Debug.Log("Connected to server");
	}

	void OnDisconnectedFromServer(NetworkDisconnection info)
	{
		if (info == NetworkDisconnection.LostConnection)
		{
			Debug.Log("Lost connection to the server");
		}
		else
		{
			Debug.Log("Successfully diconnected from the server");
		}
	}
}
