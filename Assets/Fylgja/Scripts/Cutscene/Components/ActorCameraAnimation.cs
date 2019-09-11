using UnityEngine;
using System.Collections;

public class ActorCameraAnimation : ActorSceneComponent
{
	public AnimationClip cameraAnimation;
	public int cameraDepth = 40;
	public LayerMask cameraCullingMask = -1153434113;
    public bool fadeIn = false;
    public bool fadeOut = false;
    public float fadeDuration = 0.3f;

    LogicCamera cameraToAnimate;
	Animation animate;
	GameObject instantiatedObject;

	PlayerInteraction.CameraItem cameraToAnimateHandle;
	PlayerInteraction.ListenerStackItem listenerHandle;

	bool isActing;
    bool skip = false;
    FadeInFadeOut fadeInFadeOut;
    bool isFading = false;

	protected override void Act()
	{
		transform.rotation = new Quaternion();
		transform.position = new Vector3();
		instantiatedObject = Instantiate(new GameObject(), new Vector3(), new Quaternion()) as GameObject;
        instantiatedObject.name = "ActorCameraAnimation_" + name;

        fadeInFadeOut = instantiatedObject.AddComponent<FadeInFadeOut>();
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
        if (fadeIn)
            fadeInFadeOut.FadeIn(fadeDuration);
	}
	

	public override void Skip()
	{
        skip = true;
        animate[cameraAnimation.name].speed = 0;
        //animate[cameraAnimation.name].normalizedTime = 1.0f;
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

        if (!isFading && animate[cameraAnimation.name].length - animate[cameraAnimation.name].time < fadeDuration)
        {
            isFading = true;
            fadeInFadeOut.FadeOut(fadeDuration);
        }

        if (!animate.IsPlaying(cameraAnimation.name) || skip)
		{
			OnAnimationDone();
            fadeInFadeOut.FadeIn(fadeDuration);
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

