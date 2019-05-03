using UnityEngine;

public class CharacterIdling : MonoBehaviour
{
	public Animation walkAnimation;
	public Animation idleAnimation;

	public void Start()
	{
	}

	public void Update()
	{
		/*
		 * bool isIdle = animation.IsPlaying(idleAnimation.name);
		 * if (!isIdle) {
		 *      return;
		 * }
		 */
		Vector3 requestedDirection = new Vector3(Input.GetAxis("horizontal"), Input.GetAxis("vertical"), 0);

		if (requestedDirection.magnitude > 0.1f)
		{
			Debug.Log("Walk forward!");
			// animation.CrossFade("walk");
		}
	}
}
