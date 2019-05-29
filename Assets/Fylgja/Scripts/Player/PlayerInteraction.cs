using UnityEngine;
using System.Collections.Generic;

public class PlayerInteraction : MonoBehaviour
{
	public GameObject clickOnGroundEffect;
	public GameObject gameplayCameraToSpawn;
	public GameObject ingameCameraToSpawn;
	public GameObject listenerToSpawn;
	public Camera menuCamera;

	public Player player;
	public LayerMask rayCastLayers = -1;
	public VehicleInput vehicleInput;
	public GameObject hudPrefab;

	Interactable highlightedInteractable;
	Minigame activeMinigame;
	bool avatarInteractionEnabled = true;
	bool allowedToUseUI = false;
	int countDownToInteract;

	PlayerHud hud;
	Minimap minimap;
	Subtitles subtitles;

	AudioListener listener;
	LogicCameraInfoApplicator cameraApplicator;
	bool avatarMoveEnabled = true;

    Vector3 interactButtonDownMousePosition;

    public class ListenerStackItem
	{
		Transform transform;
		public string name;

		public ListenerStackItem(Transform transformTarget)
		{
			transform = transformTarget;
			name = transformTarget.name;
		}

		public Transform TargetTransform
		{
			get
			{
				return transform;
			}
		}
	};

	public class CameraItem
	{
		LogicCamera camera;
		string cameraName;


		public CameraItem(LogicCamera cameraToSwitchTo, string _cameraName)
		{
			cameraName = _cameraName;
			DebugUtilities.Assert(cameraToSwitchTo != null, "You can not switch to a null camera");
			camera = cameraToSwitchTo;
		}

		public LogicCamera TargetCamera
		{
			get
			{
				return camera;
			}
		}
		
		public string CameraName
		{
			get
			{
				return cameraName;
			}
		}
	}

	List<ListenerStackItem> listenerStack = new List<ListenerStackItem>();

	List<CameraItem> cameraStack = new List<CameraItem>();
	


	void Start()
	{
		player.playerInteraction = this;
		if (ingameCameraToSpawn != null)
		{
			SetupListener();
			SetupIngameCamera();
		}
	}
	
	public bool AllowedToUseUI
	{
		get
		{
			return allowedToUseUI;
		}
	}

	public void FadeUp()
	{
		cameraApplicator.fadeInOut.FadeIn(4.0f);
	}

	public ListenerStackItem AttachListenerToTransform(Transform transform)
	{
		Debug.Log("** Attach listener to" + transform.name);
		var listenerHandle = new ListenerStackItem(transform);

		listenerStack.Add(listenerHandle);

		AttachListenerToHighestPriority();

		return listenerHandle;

	}

	void AttachListenerToHighestPriority()
	{
		DebugUtilities.Assert(listenerStack.Count >= 1, "Must have at least one listener active");
		var listenerHandle = listenerStack[listenerStack.Count-1];

		if (listener.transform.parent != listenerHandle.TargetTransform)
		{
			DebugUtilities.Assert(listenerHandle.TargetTransform != null, "Listener handle is destroyed:" + listenerHandle.name);
			listener.transform.parent = listenerHandle.TargetTransform;
			listener.transform.localPosition = new Vector3();
			listener.transform.localRotation = new Quaternion();
			listener.gameObject.SetActiveRecursively1(true);
		}
		Debug.Log("Listener is on object name:" +  listenerHandle.TargetTransform.name);
	}

	public void DetachListener(ListenerStackItem item)
	{
		DebugUtilities.Assert(item != null, "Illegal handle: null");
		Debug.Log("** Removing listener from" + item.name);
		listenerStack.Remove(item);
		AttachListenerToHighestPriority();
	}

	public CameraItem AddCameraToStack(LogicCamera camera, string cameraName)
	{
		Debug.Log("Adding Camera " + camera.name + " description:" + cameraName);
		var cameraHandle = new CameraItem(camera, cameraName);

		cameraStack.Add(cameraHandle);

		SwitchToCameraWithHighestPriority();

		return cameraHandle;
	}

	public void RemoveCameraFromStack(CameraItem item)
	{
		Debug.Log("Removing Camera " + item.CameraName + " target:" + item.TargetCamera.name);
		cameraStack.Remove(item);
		SwitchToCameraWithHighestPriority();
	}

	void SwitchToCameraWithHighestPriority()
	{
		DebugUtilities.Assert(cameraStack.Count > 0, "Camera stack is empty");
		var cameraHandle = cameraStack[cameraStack.Count - 1];

		Debug.Log("Active Camera " + cameraHandle.CameraName + " target:" + cameraHandle.TargetCamera.name);
		if (cameraApplicator.CurrentCamera != cameraHandle.TargetCamera)
		{
			DebugUtilities.Assert(cameraHandle.TargetCamera != null, "The target camera is null, how is that possible?");
			cameraApplicator.SetLogicCamera(cameraHandle.TargetCamera);
		}
	}

	public void TurnOnListener()
	{
		Debug.Log("************ Listener ON");
		listener.enabled = true;
	}

	public void TurnOffListener()
	{
		Debug.Log("************ Listener OFF");
		listener.enabled = false;
	}

	
	public void OnSubtitleStart(string text)
	{
		subtitles.OnSubtitleStart(text);
	}
	
	public void OnSubtitleStop()
	{
		subtitles.OnSubtitleStop();
	}

	public void OnCutsceneStart()
	{
		minimap.transform.parent.gameObject.SetActiveRecursively1(false);
	}

	public void OnCutsceneEnd()
	{
		minimap.transform.parent.gameObject.SetActiveRecursively1(true);
	}

	void SetupListener()
	{
		Debug.Log("Spawning Listener");
		listener = (Instantiate(listenerToSpawn) as GameObject).GetComponent<AudioListener>();
		DebugUtilities.Assert(listener != null, " ListenerGameObject is null, bad spawn");
	}
	
	void SetupIngameCamera()
	{
		var defaultCameraToSpawnObject = Instantiate(ingameCameraToSpawn) as GameObject;
		cameraApplicator = defaultCameraToSpawnObject.GetComponent<LogicCameraInfoApplicator>();
		DebugUtilities.Assert(cameraApplicator != null, "Must have PlayerCamera component on this camera");
	}

	Camera SetupGameplayCamera(GameObject objectToFollow, Transform spawnTransform)
	{
		var gameplayCameraToSpawnObject = Instantiate(gameplayCameraToSpawn) as GameObject;

		foreach(GameObject go in GameObject.FindGameObjectsWithTag("IndoorsTrigger"))
		{
			go.GetComponent<PlayerIndoorsTrigger>().targetCamera = cameraApplicator;
		}

		Debug.Log("Gameplay Camera attached to" + objectToFollow.name);
		var cameraAttachment = objectToFollow.GetComponentInChildren<PlayerCameraAttachment>();
		DebugUtilities.Assert(cameraAttachment.cameraRoot != null, "Must have a valid camera-root");
		DebugUtilities.Assert(cameraApplicator != null, "Must set a camera applicator");
		cameraApplicator.SetObjectToFollow(cameraAttachment.cameraRoot);
		AttachListenerToTransform(objectToFollow.transform.Find("ListenerTransform"));

		var logicCamera = gameplayCameraToSpawnObject.GetComponentInChildren<PlayerCamera>();
		AddCameraToStack(logicCamera, "GameplayCamera");

		return cameraApplicator.GetComponent<Camera>();
	}


	public void OnAssignedVehicle(Vehicle vehicle)
	{
		Debug.Log("Received a new vehicle:" + vehicle.name);
		if (hud == null)
		{
			var hudObject = Instantiate(hudPrefab) as GameObject;
			hud = hudObject.GetComponentInChildren<PlayerHud>();
			hud.quitMinigameButton.gameObject.SetActiveRecursively1(false);

			minimap = hud.GetComponentInChildren<Minimap>();
			subtitles = hud.GetComponentInChildren<Subtitles>();
			var playerCamera = SetupGameplayCamera(vehicle.gameObject, vehicle.transform);
			minimap.cameraToFollow = playerCamera.GetComponent<Camera>();

			var avatarQuest = vehicle.GetComponentInChildren<AvatarQuest>();

			if (avatarQuest != null)
			{
				var minimapCamera = hud.GetComponentInChildren<MinimapCamera>();
				minimapCamera.objectToFollow = avatarQuest;

				var minimapSurface = hud.GetComponentInChildren<MinimapSurface>();
				minimapSurface.avatarToFollow = avatarQuest;
			}
		}
	}

	Transform AvatarTransform()
	{
		return player.AssignedAvatar().transform;
	}

	bool InteractableIsWithinWalkingDistance(Interactable interactable)
	{
		if (player.AssignedAvatar() == null)
		{
			return true;
		}
		Vector3 interactablePosition = interactable.transform.position;
		Vector3 ownPosition = AvatarTransform().position;

		float distance = Vector3.Distance(interactablePosition, ownPosition);
		bool isClose = (distance < 200);

		return isClose;
	}


	public bool CanInteractWith(Interactable interactable)
	{
		if (!interactable.canInteractFromAnyDistance)
		{
			bool isClose = InteractableIsWithinWalkingDistance(interactable);
			if (!isClose)
			{
				Debug.Log("Interactable is not close");
				return false;
			}
		}

		return ActionUtility.IsAnyActionPossible(interactable.gameObject, player.AssignedAvatar());
	}

	public void InteractableClick(Interactable interactable, Vector3 hitPoint)
	{
		Debug.Log("Clicked Interactable:" + interactable.name);
		var avatar = player.AssignedAvatar();
		if (interactable.canInteractFromAnyDistance || avatar == null)
		{
			var action = interactable.GetComponentInChildren<ActionArbitration>();
			action.ActionRequest(avatar);
		}
		else
		{
			if (!InteractableIsWithinWalkingDistance(interactable))
			{
				return;
			}
			var walkPosition = new Vector3();
			var walkRotation = new Quaternion();
			CalculateTargetPositionAndRotation(out walkPosition, out walkRotation, hitPoint);

			var directionToAvatar = (avatar.transform.position - interactable.transform.position).normalized;
			var radius = interactable.GetComponent<Collider>().bounds.size.magnitude;
			walkPosition = interactable.transform.position + directionToAvatar * radius;
			walkRotation = Quaternion.LookRotation(-directionToAvatar);

			avatar.TryToPerformPrimaryAction(interactable.gameObject, walkPosition, walkRotation);
		}
	}

	void InteractedWithGround(RaycastHit hit)
	{
		ShowClickEffect(hit.point, Quaternion.LookRotation(hit.normal) * Quaternion.Euler(new Vector3(270.0f, 0, 0)));
		var avatar = player.AssignedAvatar();
		MoveVehicleToTarget(hit.point, Quaternion.LookRotation(hit.point - avatar.transform.position));
	}

	void ShowClickEffect(Vector3 position, Quaternion rotation)
	{
//		Vector3 effectRotation = transform.forward - (Vector3.Dot(transform.forward, hit.normal) * hit.normal);
		Instantiate(clickOnGroundEffect, position, Quaternion.Euler(new Vector3(180.0f, 0, 0.0f)) * rotation);
	}

	void InteractableMouseEnter(Interactable interactable)
	{
		// Debug.Log("MouseEnter: " + interactable.name);
		interactable.OnInteractionMouseEnter();
	}

	void InteractableMouseExit(Interactable interactable)
	{
		// Debug.Log("MouseExit: " + interactable.name);
		interactable.OnInteractionMouseExit();
	}

	void InteractableMouseOver(Interactable interactable)
	{
		if (highlightedInteractable != interactable || (highlightedInteractable != null && !CanInteractWith(highlightedInteractable)))
		{
			if (highlightedInteractable)
			{
				InteractableMouseExit(highlightedInteractable);
				highlightedInteractable = null;
			}
		}
		if (interactable != null && highlightedInteractable == null)
		{
			if (CanInteractWith(interactable))
			{
				InteractableMouseEnter(interactable);
				highlightedInteractable = interactable;
			}
		}
	}

	Interactable CheckInteractableFromMousePosition(out RaycastHit hit)
	{
		hit = new RaycastHit();

		var camerasInDepthOrder = new List<Camera>();
		foreach (var interactCamera in Camera.allCameras)
		{
			if (!interactCamera.enabled || interactCamera.targetTexture != null)
			{
				continue;
			}
			camerasInDepthOrder.Add(interactCamera);
		}

		camerasInDepthOrder.Sort(delegate(Camera c1, Camera c2) { return c2.depth.CompareTo(c1.depth); });

		/*
		 * Debug.Log("***** CAMERAS *****");
		 * foreach (var interactCamera in camerasInDepthOrder)
		 * {
		 *      Debug.Log("Camera:" + interactCamera.name + " depth:" + interactCamera.depth);
		 * }
		 * Debug.Log("**************");
		 */

		foreach (var interactCamera in camerasInDepthOrder)
		{
			Ray ray = interactCamera.ScreenPointToRay(Input.mousePosition);
			Debug.DrawRay(ray.origin, ray.direction * 10.0f, Color.yellow);

			if (Physics.Raycast(ray, out hit, 100.0f, rayCastLayers.value))
			{
				var interactable = hit.collider.gameObject.GetComponentInChildren<Interactable>();
				InteractableMouseOver(interactable);
				return interactable;
			}
		}

		InteractableMouseOver(null);
		return null;
	}

	void CalculateTargetPositionAndRotation(out Vector3 walkPosition, out Quaternion walkRotation, Vector3 hitPoint)
	{
		var avatar = player.AssignedAvatar();
		var directionToAvatar = (avatar.transform.position - hitPoint).normalized;
		var radius = 0.05f;
		walkPosition = hitPoint + directionToAvatar * radius;
		walkRotation = Quaternion.LookRotation(-directionToAvatar);
	}

	void ClickedOnObject(RaycastHit hit)
	{
		var avatar = player.AssignedAvatar();
		if (!avatar)
		{
			return;
		}

		Vector3 walkPosition;
		Quaternion walkRotation;

		CalculateTargetPositionAndRotation(out walkPosition, out walkRotation, hit.point);
		MoveVehicleToTarget(walkPosition, walkRotation);
	}

	void MoveVehicleToTarget(Vector3 targetPosition, Quaternion targetRotation)
	{
		var avatar = player.AssignedAvatar();
		if (!avatar)
		{
			return;
		}
		
		var vehicle = avatar.ControlledVehicle();
//		ShowClickEffect(targetPosition, targetRotation);
		var moveToPoint = vehicle.GetComponentInChildren<VehicleMoveToPoint>();
		moveToPoint.MoveToTarget(targetPosition, targetRotation);
	}

	void CheckClickInput()
	{
		RaycastHit hit = new RaycastHit();
		var mouseOverInteractable = CheckInteractableFromMousePosition(out hit);


		bool interactButtonUp = Input.GetButtonUp("interact");
		bool interactButtonDown = Input.GetButtonDown("interact");
        if (interactButtonDown)
            interactButtonDownMousePosition = Input.mousePosition;


        if (!interactButtonUp && !interactButtonDown)
		{
			return;
		}

		if (!hit.collider)
		{
			return;
		}

		var clickedOnObject = hit.collider.gameObject;
		if (clickedOnObject == null)
		{
			return;
		}
		var clickedOnLayer = clickedOnObject.layer;
		bool foundGround = clickedOnLayer == Layers.Ground || clickedOnLayer == Layers.Water;
		
		if ((mouseOverInteractable != null && mouseOverInteractable.canInteractFromAnyDistance && !foundGround && allowedToUseUI) || (avatarInteractionEnabled))
		{
		}
		else
		{
			mouseOverInteractable = null;
			foundGround = false;
		}
		
		if (foundGround && avatarMoveEnabled)
		{
            float mousePositionDelta = (interactButtonDownMousePosition - Input.mousePosition).magnitude;

            if (interactButtonUp && mousePositionDelta < 10)
			{
				InteractedWithGround(hit);
			}
		}
		else if (mouseOverInteractable == null)
		{
			if (interactButtonDown)
			{
				ClickedOnObject(hit);
			}
		}
		else if ((interactButtonUp || interactButtonDown) && (highlightedInteractable != null))
		{
			if ( (interactButtonDown && highlightedInteractable.TrigActionOnDown()) || (interactButtonUp && !highlightedInteractable.TrigActionOnDown()))
			{
				InteractableClick(highlightedInteractable, hit.point);
			}
		}
	}

	void InputForVehicle()
	{
		if (player.AssignedAvatar() == null)
		{
			return;
		}
		Vehicle vehicle = player.AssignedAvatar().ControlledVehicle();
		if (!vehicle)
		{
			return;
		}
		vehicleInput.SetVehicle(vehicle);
		vehicleInput.OnInput();
	}
	

	void Update()
	{
		if(subtitles != null)
		{
			subtitles.ShouldBeVisible = player.playerStorage.playerData().subtitlesEnabled;
		}
		
		if (countDownToInteract > 0)
		{
			countDownToInteract--;
			return;
		}

		CheckClickInput();
		InputForVehicle();
	}

	public void ToggleMenu()
	{
		menuCamera.enabled = !menuCamera.enabled;
	}

	public void QuitMinigame()
	{
		activeMinigame.QuitMinigame();
	}

	public void OnMinigameStart(Minigame game)
	{
		Debug.Log("Playerinteraction noticed that minigame has started");
		hud.quitMinigameButton.gameObject.SetActiveRecursively1(true);
		activeMinigame = game;
	}

	public void OnMinigameFailed(Minigame game)
	{
		CloseMinigame();
	}

	public void OnMinigameAborted(Minigame game)
	{
		CloseMinigame();
	}

	public void OnMinigameDone(Minigame game)
	{
		CloseMinigame();
	}


	void CloseMinigame()
	{
		Debug.Log("Playerinteraction noticed that minigame has ended");
		activeMinigame = null;
		hud.quitMinigameButton.gameObject.SetActiveRecursively1(false);
	}

	public void OnAllowedToMove(bool move)
	{
		avatarMoveEnabled = move;
	}
	
	public void OnAllowedToUseUI(bool allowed)
	{
		allowedToUseUI = allowed;
		UpdateInteraction();
	}
	
	void UpdateInteraction()
	{
		var showCursor = allowedToUseUI || avatarInteractionEnabled;
		Cursor.visible = showCursor;
	}

	public void OnAllowedToInteract(bool move)
	{
		avatarInteractionEnabled = move;
		if (move)
		{
			countDownToInteract = 10;
		}
		UpdateInteraction();
	}
}
