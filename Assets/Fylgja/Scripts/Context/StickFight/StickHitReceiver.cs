using UnityEngine;
using System.Collections;

public class StickHitReceiver : MonoBehaviour
{
	public StickFighter fighter;

	public void OnReceivedHitLeft()
	{
		fighter.OnReceivedHitFromLeft();
	}

	public void OnReceivedHitRight()
	{
		fighter.OnReceivedHitFromRight();
	}
}
