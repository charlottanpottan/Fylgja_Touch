using UnityEngine;
using System.Collections;

public class TriggerToParent : MonoBehaviour
{
	public string messageToSend;

	// Use this for initialization
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
	}

	void OnTriggerEnter()
	{
		transform.parent.SendMessage(messageToSend);
	}
}
