using UnityEngine;
using System.Collections;

public class BlacksmithSledgehammer : MonoBehaviour
{
	private Transform hitCenter;
	[HideInInspector]
	public bool canHit = false;
	public CharacterBlacksmith smither;

	void Start()
	{
		hitCenter = GameObject.Find("SmithPoint").transform;
	}

	void Update()
	{
	}

	void OnTriggerEnter(Collider collider)
	{
		var sword = collider.transform.root.GetComponentInChildren<BlacksmithSword>();
		if (sword == null)
		{
			// Debug.LogWarning("Got collision with something else other than a BlacksmithSword:" + collider.name);
			return;
		}

		if (!sword.IsSwordDone() && canHit)
		{
			canHit = false;
			bool swordIsDone = sword.HitWithSledgeHammer(hitCenter.position);
			smither.OnSledgehammerHitSword(swordIsDone);
		}
	}
}

