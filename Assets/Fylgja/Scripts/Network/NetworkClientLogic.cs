using UnityEngine;
using System.Collections;

public class NetworkClientLogic : MonoBehaviour
{
	public GameObject objectToSpawn;
	public Transform spawnTransform;

	public void OnConnectedToServer()
	{
		Debug.Log("Client Logic has understood that we're connected!");
		int group = 0;
		spawnTransform.position = new Vector3(spawnTransform.position.x, spawnTransform.position.y + Random.Range(-1.0f, 1.0f), spawnTransform.position.z);
		Network.Instantiate(objectToSpawn, spawnTransform.position, spawnTransform.rotation, group);
	}
}

