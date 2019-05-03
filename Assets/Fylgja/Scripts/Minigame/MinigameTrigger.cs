using UnityEngine;
using System.Collections;

public class MinigameTrigger : MonoBehaviour
{
	public Minigame minigame;

	void Start()
	{
	}

	void Update()
	{
	}

	public void OnTriggerEnter(Collider other)
	{
		Debug.Log("Someone got into the minigame trigger");
		var avatar = other.gameObject.GetComponentInChildren(typeof(CharacterAvatar)) as CharacterAvatar;
		if (avatar != null)
		{
			minigame.StartMinigame(avatar);
		}
	}
}

