using UnityEngine;
using System.Collections;

public class CharacterPetroglypher : MonoBehaviour
{
	public LayerMask rayCastLayers = -1;
	public AudioHandler audioHandler;

	bool isCarving;
	Petroglyph holdingPetroglyph;
	bool dontCheckDrop;
	Vector3 pickingOffset = new Vector3();

	void Start()
	{
	}

	void Update()
	{
		if (!isCarving)
		{
			return;
		}

		MovePetroglyph();
		RotatePetroglyph();
		CheckIfDropPetroglyph();
	}

	bool GetMouseTargetTransform(out Vector3 targetPosition, out Quaternion targetRotation)
	{
		var activeCamera = Camera.main;
		var ray = activeCamera.ScreenPointToRay(Input.mousePosition);

		Debug.DrawRay(ray.origin, ray.direction * 1.0f, Color.magenta);

		RaycastHit hit = new RaycastHit();

		if (Physics.Raycast(ray, out hit, 4.0f, rayCastLayers))
		{
			targetRotation = Quaternion.LookRotation(hit.normal);
			targetPosition = hit.point;
			return true;
		}

		targetPosition = new Vector3();
		targetRotation = new Quaternion();
		return false;
	}

	void CheckIfDropPetroglyph()
	{
		if (holdingPetroglyph == null)
		{
			return;
		}

		if (!dontCheckDrop && Input.GetButtonDown("interact"))
		{
			DropPetroglyph();
		}
		dontCheckDrop = false;
	}

	void DropPetroglyph()
	{
		Debug.Log("Drop Petroglyph");
		audioHandler.TriggerSound();
		holdingPetroglyph = null;
	}


	void RotatePetroglyph()
	{
		if (holdingPetroglyph == null)
		{
			return;
		}

		if (Input.GetButton("mouse0"))
		{
			float vertical = -Input.GetAxis("mouse_vertical") * Time.deltaTime;
			float horizontal = Input.GetAxis("mouse_horizontal") * Time.deltaTime;

			Vector3 eulerRotation = holdingPetroglyph.transform.rotation.eulerAngles;
			eulerRotation.y += ((vertical + horizontal) / 2.0f) * 0.5f;
			holdingPetroglyph.transform.rotation = Quaternion.Euler(eulerRotation);
		}
	}

	void MovePetroglyph()
	{
		if (holdingPetroglyph == null)
		{
			return;
		}

		var activeCamera = Camera.main;

		DebugUtilities.Assert(activeCamera.enabled, "Camera is not enabled!");
		var targetPosition = new Vector3();
		var targetRotation = new Quaternion();

		bool movedWithMouse = GetMouseTargetTransform(out targetPosition, out targetRotation);
		if (movedWithMouse)
		{
			var warpPosition = holdingPetroglyph.transform.position;
			warpPosition.z = targetPosition.z;
			warpPosition.x = targetPosition.x;
			holdingPetroglyph.transform.position = warpPosition + pickingOffset;
		}

		{
			var horizontal = Input.GetAxis("Horizontal");
			var vertical = Input.GetAxis("Vertical");
			targetPosition.x = horizontal * Time.deltaTime;
			targetPosition.y = vertical * Time.deltaTime;
			targetPosition.z = 0;

			pickingOffset += activeCamera.transform.TransformDirection(targetPosition);
			pickingOffset.y = 0;
		}
	}

	public bool IsHoldingPetroglyph()
	{
		return holdingPetroglyph != null;
	}

	void OnPetroglyphMinigameStart()
	{
		isCarving = true;
	}

	void OnPetroglyphMinigameFailed()
	{
		CloseMinigame();
	}

	public void OnPetroglyphMinigameAborted()
	{
		CloseMinigame();
	}

	public void OnPetroglyphMinigameDone()
	{
		CloseMinigame();
	}

	void CloseMinigame()
	{
		isCarving = false;
	}

	public void PickupPetroglyph(Petroglyph petroglyph)
	{
		if (holdingPetroglyph != null)
		{
			DropPetroglyph();
		}
		Debug.Log("Picked up Petroglyph");
		
		audioHandler.TriggerSound();
		
		holdingPetroglyph = petroglyph;

		var targetPosition = new Vector3();
		var targetRotation = new Quaternion();
		bool mouseIsUsed = GetMouseTargetTransform(out targetPosition, out targetRotation);
		if (mouseIsUsed)
		{
			pickingOffset = holdingPetroglyph.transform.position - targetPosition;
			pickingOffset.y = 0;
		}
		dontCheckDrop = true;
	}
}
