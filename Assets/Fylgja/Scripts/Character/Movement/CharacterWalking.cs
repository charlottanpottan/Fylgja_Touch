using UnityEngine;

public class CharacterWalking : Vehicle
{
	public LegAnimator legAnimator;

	public float maxWalkSpeed = 3.0f;
	public float turnFactor = 10.0f;
	
	public CharacterStepDetector[] stepDetectors;

	private CharacterController characterController;
	private bool wantsToClimb;
	private Quaternion requestedRotation = Quaternion.identity;
	private float requestedSpeed = 0;
	private float currentSpeed = 0;
	private bool allowedToMove = true;
	private string currentAnimationGroup = "normal";

	public void Start()
	{
		FetchCharacterController();
		requestedRotation = transform.rotation;
	}

	public void FetchCharacterController()
	{
		characterController = GetComponent<CharacterController>();
	}

	public override void FeedInput()
	{
	}

	public override bool IsAvatar
	{
		get
		{
			return true;
		}
	}
	
	void SmoothRotation()
	{
		float y = transform.eulerAngles.y;
		float deltaY = Angle.AngleDiff(requestedRotation.eulerAngles.y, transform.eulerAngles.y);

		float absDeltaY = Mathf.Abs(deltaY);
		const float minimumFramesToReachRotation = 2.0f;
		float maxSpeed = absDeltaY / Time.deltaTime / minimumFramesToReachRotation;
		float rampedSpeed = 5.0f + absDeltaY * absDeltaY;
		
		float speed = Mathf.Sign(deltaY) * Mathf.Clamp(rampedSpeed, 0, maxSpeed);
//		Debug.Log("SmoothRotation delta:" + deltaY + " target:" + requestedRotation.eulerAngles.y + " source:" + transform.eulerAngles.y + " speed:" + speed * Time.deltaTime);
		y += speed * Time.deltaTime;
			
		transform.eulerAngles = new Vector3(0, y, 0);
	}
	
	void Update()
	{
		if (characterController == null || !allowedToMove)
		{
			return;
		}
		
		Vector3 moveDirection = requestedRotation * Vector3.forward;
		Vector3 velocityVector;
		if (characterController.isGrounded)
		{
			SmoothRotation();
			
			if (allowedToMove)
			{
				float deltaSpeed = (requestedSpeed - currentSpeed) * Time.deltaTime * 4.0f;
				currentSpeed += deltaSpeed;
			}
			else
			{
				currentSpeed = 0;
			}
		}
		else
		{
			currentSpeed = 0;
		}
		
		velocityVector = moveDirection * (currentSpeed * maxWalkSpeed);

		const float gravity = 8.92f;
		velocityVector.y -= gravity;

		characterController.Move(velocityVector * Time.deltaTime);
	}

	public override void Move(Quaternion desiredOrientation, float desiredSpeed)
	{
		Vector3 direction = desiredOrientation * Vector3.forward;
		direction.y = 0;
		direction = direction.normalized;

		requestedRotation = Quaternion.LookRotation(direction);
		requestedSpeed = desiredSpeed;
	}

	public override void StopMoving()
	{
		requestedSpeed = 0;
		currentSpeed = 0;
		requestedRotation = transform.rotation;
	}

	public override void SetAllowedToMove(bool allowed)
	{
		//Debug.Log("Walking: SetAllowedToMove:" + allowed + " on " + name);
		allowedToMove = allowed;
		if (!allowedToMove)
		{
			foreach(var detector in stepDetectors)
			{
				detector.IsEnabled(false);
			}
			StopMoving();
		}
		else
		{
			foreach(var detector in stepDetectors)
			{
				detector.IsEnabled(true);
			}
			BlendToLocomotion();
		}
		// legAnimator.enabled = allowed;
	}

	public void OnSnapTo(Transform t)
	{
		requestedRotation = t.rotation;
	}

	public void OnFirePitMinigameStart()
	{
		currentAnimationGroup = "fireminigame";
		BlendToLocomotion();
	}

	public void OnMinigameFailed()
	{
		SwitchToNormalWalk();
	}

	public void OnMinigameAborted()
	{
		SwitchToNormalWalk();
	}

	public void OnMinigameDone()
	{
		SwitchToNormalWalk();
	}

	void SwitchToNormalWalk()
	{
		currentAnimationGroup = "normal";
	}

	public void TurnOffLocomotion()
	{
		Debug.Log("Turning off locomotion!");
		animation.Stop("fireminigame");
		animation.Stop("normal");
		animation.Stop("LocomotionSystem");
	}

	public void BlendToLocomotion()
	{
		if (!allowedToMove)
		{
			Debug.Log("Ignoring blend to locomotion since we are not allowed to move anyway");
			return;
		}

		Debug.Log("Turning blending to locomotion:" + currentAnimationGroup);
		animation.Play("LocomotionSystem");
		animation.CrossFade(currentAnimationGroup);
	}
}
