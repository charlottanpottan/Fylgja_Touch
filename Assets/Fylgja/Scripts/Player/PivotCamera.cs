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
    float cameraDistance = 3.5f;
    float targetCameraDistance = 3.5f;
    Vector3 prevMousePosition;

    public override void OnCameraSwitch(LogicCameraInfo cameraInfo)
    {
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

            Vector3 eulerAngles = cameraInfo.pivotRotation.eulerAngles;
            eulerAngles.x += addX;
            eulerAngles.y += addY;
            eulerAngles.x = ClampAngle(eulerAngles.x, minimumVerticalAngle, maximumVerticalAngle);

            cameraInfo.pivotRotation.eulerAngles = eulerAngles;
            cameraInfo.pivotRotationIsDefined = true;
        }
    }

    public override void SetCameraPivot(ref LogicCameraInfo cameraInfo, Vector2 targetPivot)
    {
        Vector3 eulerAngles = new Vector3(targetPivot.x, targetPivot.y, 0);
        eulerAngles.x = ClampAngle(eulerAngles.x, minimumVerticalAngle, maximumVerticalAngle);
        cameraInfo.pivotRotation.eulerAngles = eulerAngles;
    }

    public override void SetCameraPivotDistance(ref LogicCameraInfo cameraInfo, float distance, bool instant)
    {
        cameraInfo.pivotDistance = distance;
        if(instant)
          cameraDistance = distance;
        targetCameraDistance = distance;
    }

    float ClampAngle(float angle, float from, float to)
    {
        // accepts e.g. -80, 80
        if (angle < 0f) angle = 360 + angle;
        if (angle > 180f) return Mathf.Max(angle, 360 + from);
        return Mathf.Min(angle, to);
    }
}
