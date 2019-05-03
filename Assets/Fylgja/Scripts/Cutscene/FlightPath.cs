using UnityEngine;
using System.Collections;
using System;


[System.Serializable]
public class FlightRoute
{
	public GameObject targetGameObject;
	public Transform[] pathNodes;
	[HideInInspector]
	public Transform[] reversePathNodes;
	public float moveSpeed = 8;
	public float lookAhead = 0.1f;
	public float lookTime = 0.1f;
	public iTween.EaseType easeType = iTween.EaseType.easeOutSine;

	public void DoMovement(bool inReverse)
	{
		if (inReverse)
		{
			iTween.MoveTo(targetGameObject, iTween.Hash("orienttopath", true, "speed", moveSpeed, "lookahead", lookAhead, "looktime", lookTime, "path", reversePathNodes, "easetype", easeType, "movetopath", false, "oncomplete", "FollowRoute", "oncompletetarget", targetGameObject));
		}
		else
		{
			iTween.MoveTo(targetGameObject, iTween.Hash("orienttopath", true, "speed", moveSpeed, "lookahead", lookAhead, "looktime", lookTime, "path", pathNodes, "easetype", easeType, "movetopath", false, "oncomplete", "FollowRoute", "oncompletetarget", targetGameObject));
		}
	}
}

public class FlightPath : MonoBehaviour
{
	private int currentIndex = 0;
	public FlightRoute[] routes;
	public bool drawGizmo;
	public AnimationClip moveAnim;
	public AnimationClip waitAnim;
	[HideInInspector]
	public int targetIndex;
	
	void Awake()
	{
		Initialize();	
	}
	
	public void Initialize()
	{
		foreach (FlightRoute route in routes)
		{
			route.reversePathNodes = new Transform[route.pathNodes.Length];

			Array.Copy(route.pathNodes, route.reversePathNodes, route.pathNodes.Length);

			Array.Reverse(route.reversePathNodes);
		}

		GetComponent<Animation>()[moveAnim.name].wrapMode = WrapMode.Loop;

		GetComponent<Animation>()[waitAnim.name].wrapMode = WrapMode.PingPong;
	}

	public void FollowRoute()
	{
		if (targetIndex < currentIndex)
		{
			GetComponent<Animation>().CrossFade(moveAnim.name);
			routes[currentIndex - 1].DoMovement(true);
			currentIndex--;
			return;
		}
		if (targetIndex > currentIndex)
		{
			GetComponent<Animation>().CrossFade(moveAnim.name);
			routes[currentIndex].DoMovement(false);
			currentIndex++;
			return;
		}
		if (targetIndex == currentIndex)
		{
			GetComponent<Animation>().CrossFade(waitAnim.name);
			transform.rotation = Quaternion.Euler(new Vector3(0,transform.rotation.eulerAngles.y,0));
			return;
		}
	}

	void OnDrawGizmos()
	{
		if (drawGizmo)
		{
			foreach (FlightRoute route in routes)
			{
				iTween.DrawPath(route.pathNodes);
			}
		}
	}
}
