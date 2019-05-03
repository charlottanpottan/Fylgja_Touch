using UnityEngine;
using System.Collections;

public class StickEnd : MonoBehaviour
{
	public enum StickSide
	{
		Left,
		Right
	}

	public StickSide stickSide;

	void OnTriggerEnter(Collider other)
	{
		var receiver = other.GetComponent<StickHitReceiver>();

		if (receiver == null)
		{
			return;
		}

		if (stickSide == StickEnd.StickSide.Left)
		{
			receiver.OnReceivedHitRight();
		}
		else
		{
			receiver.OnReceivedHitLeft();
		}
	}
}

