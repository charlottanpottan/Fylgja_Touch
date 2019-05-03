using UnityEngine;
using System.Collections;

public abstract class Vehicle : MonoBehaviour
{
	public Transform seatingTransform;
	IAvatar controllingAvatar;

	void Start()
	{
	}

	void Update()
	{
	}

	public abstract bool IsAvatar
	{
		get;
	}

	public virtual void OnAvatarEnter(IAvatar avatar)
	{
	}

	public virtual void OnAvatarLeave(IAvatar avatar)
	{
	}

	public virtual void FeedInput()
	{
	}

	public virtual void Move(Quaternion rotation, float speed)
	{
	}

	public virtual void StopMoving()
	{
	}
	
	public virtual void SampleAnimation()
	{
	}

	public abstract void SetAllowedToMove(bool allowed);

	public void SetControllingAvatar(IAvatar avatar)
	{
		controllingAvatar = avatar;
	}

	public bool HasControllingAvatar()
	{
		return controllingAvatar != null;
	}
}

