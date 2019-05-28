using UnityEngine;

public class PivotCamera : LogicCamera
{
    float verticalSpeed = 3;
    float horizontalSpeed = 3;
    public float distanceSpeed = 0.01f;

    public float minimumVerticalAngle = -10.0f;
    public float maximumVerticalAngle = 55.0f;

    public float minimumDistance = 0.2f;
    public float maximumDistance = 4.0f;

    float rotateX;
    float rotateY;
    float cameraDistance = 2.0f;
    float targetCameraDistance = 2.0f;
    Vector3 prevMousePosition;

    public override void SetCameraPivot(ref LogicCameraInfo cameraInfo, Vector2 targetPivot)
    {
        cameraInfo.pivotRotationX = targetPivot.x;
        cameraInfo.pivotRotationY = targetPivot.y;
        cameraInfo.pivotRotationX = Mathf.Clamp(cameraInfo.pivotRotationX, minimumVerticalAngle, maximumVerticalAngle);
    }

    public override void UpdateCamera(ref LogicCameraInfo cameraInfo)
    {
        targetCameraDistance += -Input.GetAxis("camera_distance") * Time.deltaTime * distanceSpeed;
        targetCameraDistance = Mathf.Clamp(targetCameraDistance, minimumDistance, maximumDistance);

        cameraDistance = Mathf.Lerp(cameraDistance, targetCameraDistance, 0.2f);
        cameraInfo.pivotDistance += cameraDistance;

        if (Input.GetButtonDown("mouse0"))
        {
            prevMousePosition = Input.mousePosition;
        }

        Vector3 currentPosition = Input.mousePosition;
        Vector3 deltaPositon = currentPosition - prevMousePosition;
        prevMousePosition = currentPosition;

        if (Input.GetButton("mouse0"))
        {
            float vertical = -deltaPositon.y;
            float horizontal = deltaPositon.x;

            float addX = vertical * Time.deltaTime * verticalSpeed;
            float addY = horizontal * Time.deltaTime * horizontalSpeed;
            
            cameraInfo.pivotRotationX += addX;
            cameraInfo.pivotRotationY += addY;
            cameraInfo.pivotRotationX = Mathf.Clamp(cameraInfo.pivotRotationX, minimumVerticalAngle, maximumVerticalAngle);
            cameraInfo.pivotRotationIsDefined = true;
        }
    }
}
