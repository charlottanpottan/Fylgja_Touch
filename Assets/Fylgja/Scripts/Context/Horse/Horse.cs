using UnityEngine;

public class Horse : Vehicle
{
	public float maxWalkSpeed = 3.0f;
	public float turnFactor = 10.0f;
	public float acceleration = 1.0f;
	public float backwardSpeed = 0.3f;
	public CharacterController characterController;
	
	public AudioSource walkAudio;
	public float walkMultiply = 1;
	public float walkFadeSpeed = 1;
	public AudioSource runAudio;
	public float runMultiply = 1;
	public float walkToRunThreshold = 0.1f;

	private Quaternion requestedRotation = Quaternion.identity;
	private float requestedSpeed = 0;
	private float currentSpeed = 0;
	private bool allowedToMove = true;
	private string currentAnimationGroup = "normal";

	public void Start()
	{
		requestedRotation = transform.rotation;
	}

	public override bool IsAvatar
	{
		get
		{
			return false;
		}
	}


	public void Update()
	{
		Vector3 moveDirection = new Vector3(0, 0, 1);
		Vector3 velocityVector;

		if (characterController.isGrounded)
		{
			if (allowedToMove)
			{
				moveDirection = transform.rotation * Vector3.forward;
				float deltaSpeed = (requestedSpeed - currentSpeed) * Time.deltaTime * acceleration;
				currentSpeed += deltaSpeed;
				if (Mathf.Abs(currentSpeed) > 0.05f)
				{
					transform.rotation = Quaternion.Slerp(transform.rotation, requestedRotation, turnFactor * Time.deltaTime);
				}
			}
			else
			{
				currentSpeed = 0;
			}
		}
		else
		{
			currentSpeed = 0;
		} velocityVector = moveDirection * (currentSpeed * maxWalkSpeed);

		const float gravity = 8.92f;
		velocityVector.y -= gravity;

		characterController.Move(velocityVector * Time.deltaTime);
		
		float actualVelocity = new Vector3(characterController.velocity.x, 0, characterController.velocity.z).magnitude;
		
		actualVelocity = actualVelocity / maxWalkSpeed;
		
		walkAudio.volume = Mathf.Lerp(walkAudio.volume,actualVelocity * walkMultiply, walkFadeSpeed * Time.deltaTime);
		if(actualVelocity >= walkToRunThreshold)
		{
			runAudio.volume = (actualVelocity - walkToRunThreshold) * runMultiply;
		}
		else
		{
			runAudio.volume = 0;
		}
	}
	
	public override void OnAvatarEnter(IAvatar avatar)
	{
		Debug.Log("Horse: Avatar Enter");
		avatar.transform.root.BroadcastMessage("OnEnterHorse", this);
	}

	public override void OnAvatarLeave(IAvatar avatar)
	{
		Debug.Log("Horse: Avatar Leave");
		avatar.transform.root.BroadcastMessage("OnLeaveHorse", this);
	}

	public override void SetAllowedToMove(bool allowed)
	{
	}

	public override void Move(Quaternion desiredRotation, float desiredSpeed)
	{
		float angleDifference = Quaternion.Angle(desiredRotation, transform.rotation);

		if (angleDifference > 115.0f)
		{
			desiredRotation = desiredRotation * Quaternion.AngleAxis(180, Vector3.up);
			desiredSpeed = -backwardSpeed;
		}
		else
		{
		}
		requestedRotation = desiredRotation;
		requestedSpeed = desiredSpeed;
	}

	public override void StopMoving()
	{
		requestedSpeed = 0;
	}

    public void StopMovingInstant()
    {
        currentSpeed = 0;
        requestedSpeed = 0;
    }

    public void BlendToLocomotion()
	{
		GetComponent<Animation>().CrossFade(currentAnimationGroup);
	}
}
