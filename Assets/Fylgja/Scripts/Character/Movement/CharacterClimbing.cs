using UnityEngine;

public class CharacterClimbing : MonoBehaviour
{
	private CharacterController characterController;
	private bool wasClimbing;
	private bool climbStarted;
	private Transform startClimbingTransform;

	public void Start()
	{
		characterController = GetComponent(typeof(CharacterController)) as CharacterController;
	}

	public void Update()
	{
		bool isClimbing = (animation["climb"].weight > 0.9f) || animation.IsPlaying("grab");

		if (!climbStarted && isClimbing && !wasClimbing)
		{
			StartedToClimb();
		}
		wasClimbing = isClimbing;

		if (!isClimbing)
		{
			return;
		}
		if (characterController.isGrounded)
		{
			animation.CrossFade("walk");
		}
		float verticalMovement = Input.GetAxis("vertical") * Time.deltaTime;
		characterController.Move(new Vector3(0, verticalMovement, 0));
	}

	public void Climb(Transform transform)
	{
		startClimbingTransform = transform;
		climbStarted = false;
		animation.CrossFade("climb");
	}

	public bool IsClimbing()
	{
		return wasClimbing;
	}

	private void StartedToClimb()
	{
		climbStarted = true;
		characterController.transform.position = startClimbingTransform.position;
		characterController.transform.rotation = startClimbingTransform.rotation;

		characterController.transform.Translate(Vector3.up * 1.8f);
		characterController.transform.Translate(Vector3.forward * -0.5f);

		characterController.Move(new Vector3(0, 0.1f, 0));
		// Change camera here
	}

	public void Grab(Transform transform)
	{
		if (wasClimbing)
		{
			animation.CrossFade("grab");
		}
	}
}
