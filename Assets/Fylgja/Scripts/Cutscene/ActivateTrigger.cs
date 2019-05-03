using UnityEngine;
using System.Collections;

public class ActivateTrigger : MonoBehaviour
{
	public GameObject go;

	public void TriggerActivate()
	{
		Debug.Log("doitdoit");
		go.BroadcastMessage("StartMove");
	}
}