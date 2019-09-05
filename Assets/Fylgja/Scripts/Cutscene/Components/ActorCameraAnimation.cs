using UnityEngine;
using System.Collections;

public class ActorCameraAnimation : ActorSceneComponent
{
	public AnimationClip cameraAnimation;
	public int cameraDepth = 40;
	public LayerMask cameraCullingMask = -1153434113;

	LogicCamera cameraToAnimate;
	Animation animate;
	GameObject instantiatedObject;

	PlayerInteraction.CameraItem cameraToAnimateHandle;
	PlayerInteraction.ListenerStackItem listenerHandle;

	bool isActing;

	protected override void Act()
	{
		transform.rotation = new Quaternion();
		transform.position = new Vector3();
		instantiatedObject = Instantiate(new GameObject(), new Vector3(), new Quaternion()) as GameObject;
        instantiatedObject.name = "ActorCameraAnimation_" + name;

		instantiatedObject.AddComponent<FadeInFadeOut>();
		//cameraToAnimate.cullingMask = cameraCullingMask;
		//cameraToAnimate.farClipPlane = 500;
		//cameraToAnimate.depth = cameraDepth;
		animate = instantiatedObject.AddComponent<Animation>();
		var cameraToAnimate = instantiatedObject.AddComponent<LogicCameraFromGameObject>();
		animate.AddClip(cameraAnimation, cameraAnimation.name);
		animate.Play(cameraAnimation.name);
		animate[cameraAnimation.name].enabled = true;

		cameraToAnimateHandle = actingInScene.GetPlayerNotifications().AddCameraToStack(cameraToAnimate, "ActorCameraAnimation");
		ChildListenerToCameraAnimation();

		isActing = true;
	}
	

	public override void Skip()
	{
		animate[cameraAnimation.name].normalizedTime = 1.0f;
	}

	public override bool CanBeInterrupted()
	{
		return true;
	}

	void ChildListenerToCameraAnimation()
	{
		listenerHandle = actingInScene.GetPlayerNotifications().AttachListener(Camera.main.transform);
	}

	void KickoutListenerFromCameraAnimation()
	{
		Debug.Log("Removing listener from scene component: " + name);
		DebugUtilities.Assert(actingInScene != null, "Must have a acting scene");
		DebugUtilities.Assert(actingInScene.GetPlayerNotifications() != null, "Must have a player notifications");
		actingInScene.GetPlayerNotifications().DetachListener(listenerHandle);
		listenerHandle = null;
	}

	void Update()
	{
		if (!isActing)
		{
			return;
		}

		if (!animate.IsPlaying(cameraAnimation.name))
		{
			OnAnimationDone();
		}
	}

	public override void Dispose()
	{
		Destroy(instantiatedObject);
		instantiatedObject = null;
		KickoutListenerFromCameraAnimation();
		actingInScene.GetPlayerNotifications().RemoveCameraFromStack(cameraToAnimateHandle);
		cameraToAnimateHandle = null;
		base.Dispose();
	}

	void Close()
	{
		isActing = false;
		ComponentDone();
	}

	void OnAnimationDone()
	{
		Close();
	}
}

