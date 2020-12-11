using UnityEngine;
using UnityEngineInternal;

using System.Collections;

public class LogicCameraInfoApplicator : MonoBehaviour
{
    public float tiltFactor = -0.6f;
    public float heightOffset = 1.0f;
    public FadeInFadeOut fadeInOut;

    LogicCamera logicCamera;
    Transform objectToFollow;
    Vector3 oldTargetPosition;

    float rotationVelocityX = 0.0f;
    float rotationVelocityY = 0.0f;

    float rotationX = 0.0f;
    float rotationY = 0.0f;

    [HideInInspector]
    public float currentFov;

    Vector3 targetShakePosition = new Vector3(0, 0, 0);
    Vector3 shakePosition = new Vector3(0, 0, 0);
    float targetShakePositionLerp = 1.0f;


    LogicCameraInfo cameraInfo = new LogicCameraInfo();

    void Awake()
    {
        currentFov = GetComponent<Camera>().fieldOfView;
        fadeInOut.SetToBlack();
    }

    void Start()
    {
        fadeInOut.FadeIn(1f);
    }

    public void SetLogicCamera(LogicCamera logicCameraToFollow)
    {
        DebugUtilities.Assert(logicCameraToFollow != null, "Camera can not be null!");
        if (logicCamera)
        {
            //    cameraInfo.pivotRotation = logicCamera.transform.rotation;
        }
        logicCamera = logicCameraToFollow;
        // logicCamera.OnCameraSwitch(cameraInfo);
        cameraInfo.cameraSwitched = true;
    }

    public void SetObjectToFollow(Transform newObjectToFollow)
    {
        objectToFollow = newObjectToFollow;
        oldTargetPosition = objectToFollow.transform.position;
    }

    public LogicCamera CurrentCamera
    {
        get
        {
            return logicCamera;
        }
    }



    void ShakeCamera()
    {
        if (targetShakePositionLerp >= 1.0f)
        {
            shakePosition = targetShakePosition;
            targetShakePosition = Random.insideUnitSphere * 0.0005f;
            targetShakePosition.z = 0;
            targetShakePositionLerp = 0.0f;
        }
        var deltaPosition = Vector3.Lerp(shakePosition, targetShakePosition, targetShakePositionLerp);
        targetShakePositionLerp += Time.deltaTime * 7.0f;
        // transform.position += camera.transform.TransformDirection(deltaPosition);

        rotationVelocityX += -rotationVelocityX * 5.0f * Time.deltaTime - rotationX;
        rotationVelocityY += -rotationVelocityY * 5.0f * Time.deltaTime - rotationY;

        if (Random.value < 0.2f)
        {
            const float xShake = 1.0f;
            const float yShake = 0.5f;

            rotationVelocityX += Random.value * xShake - xShake / 2.0f;
            rotationVelocityY += Random.value * yShake - yShake / 2.0f;
        }

        rotationX += rotationVelocityX * Time.deltaTime;
        rotationY += rotationVelocityY * Time.deltaTime;
        var deltaRotation = Quaternion.Euler(rotationX, rotationY, 0);
        var newRotation = transform.rotation * deltaRotation;
        transform.rotation = newRotation;
    }

    void LateUpdate()
    {
        if (logicCamera == null)
        {
            return;
        }
        if (objectToFollow != null)
        {
            cameraInfo.targetVelocity = (objectToFollow.position - oldTargetPosition) / Time.deltaTime;
            oldTargetPosition = objectToFollow.position;
            cameraInfo.targetPosition = objectToFollow.position;
            cameraInfo.targetRotation = objectToFollow.rotation;
        }
        else
        {
            cameraInfo.targetPosition = new Vector3();
            cameraInfo.targetRotation = new Quaternion();
            cameraInfo.targetVelocity = new Vector3();
        }
        cameraInfo.pivotDistance = 0;
        cameraInfo.pivotOffset = 0;
        cameraInfo.pivotRotationIsDefined = false;
        cameraInfo.useSourcePosition = false;
        cameraInfo.useTargetPosition = false;

        cameraInfo.fov = currentFov;

        logicCamera.UpdateCamera(ref cameraInfo);

        GetComponent<Camera>().fieldOfView = cameraInfo.fov;
        if (cameraInfo.useSourcePosition)
        {
            var deltaPosition = cameraInfo.targetPosition - cameraInfo.sourcePosition;
            if (deltaPosition.magnitude > 0.01f)
            {
                transform.rotation = Quaternion.LookRotation(deltaPosition);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            transform.position = cameraInfo.sourcePosition;
            ShakeCamera();
        }
        else
        {
            var newPosition = cameraInfo.CameraPosition();
            var newRotation = cameraInfo.PivotRotation();
            transform.position = newPosition;
            transform.rotation = newRotation;
        }
        cameraInfo.cameraSwitched = false;
    }

    public void CameraSetPivot(Vector2 targetPivot)
    {
        logicCamera.SetCameraPivot(ref cameraInfo, targetPivot);
    }

    public void CameraSetPivotDistance(float distance, bool instant)
    {
        logicCamera.SetCameraPivotDistance(ref cameraInfo, distance, instant);
    }

#if UNITY_EDITOR
    void OnGUI()
    {
        GUIStyle guiStyle = new GUIStyle();
        guiStyle.fontSize = 30;
        GUI.color = Color.white;
        GUILayout.Label("Distance " + cameraInfo.pivotDistance, guiStyle);
    }
#endif
}
