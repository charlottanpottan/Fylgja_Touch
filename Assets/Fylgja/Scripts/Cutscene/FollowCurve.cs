using UnityEngine;
using System.Collections;

public class FollowCurve : MonoBehaviour
{
	public iTween.EaseType easeType = iTween.EaseType.linear;
	public iTween.LoopType loopType = iTween.LoopType.none;
	public bool selfDestruct;
	public Transform[] path;
	public float moveSpeed;
	public bool drawGizmo;
	public bool orientToPath = false;
	public float lookAhead;
	public bool immediately;
	public float lookTime;
	public Transform lookTarget = null;

	Hashtable ht = new Hashtable();

	void Awake()
	{
		ht.Add("path", path);
		ht.Add("movetopath", false);
		ht.Add("speed", moveSpeed);
		ht.Add("delay", 0);
		ht.Add("looptype", loopType);
		ht.Add("easetype", easeType);

		if (selfDestruct)
		{
			ht.Add("oncompletetarget", gameObject);
			ht.Add("oncomplete", "DestroySelf");
		}

		if (orientToPath)
		{
			ht.Add("orienttopath", true);
			ht.Add("lookahead", lookAhead);
		}

		if (!orientToPath && lookTarget != null)
		{
			ht.Add("looktarget", lookTarget);
		}
		ht.Add("looktime", lookTime);

		if (immediately)
		{
			StartMove();
		}
	}

	public void StartMove()
	{
		iTween.MoveTo(gameObject, ht);
	}

	void OnDrawGizmos()
	{
		if (drawGizmo)
		{
			iTween.DrawPath(path);
		}
	}

	void DestroySelf()
	{
		DestroyObject(gameObject);
	}
}
