using UnityEngine;
using System.Collections;

public class PlayerCameraCollision : LogicCamera
{
    public LayerMask layerMask;
    public float moveInLerpSpeed = 1.5f;
    public float moveOutLerpSpeed = 1f;
    public float minDistance = 0.5f;

    float currentPivotDistance = 0;

    void Update()
    {
    }

    public override void OnCameraSwitch(LogicCameraInfo cameraInfo)
    {
        currentPivotDistance = cameraInfo.pivotDistance;
    }

    public override void UpdateCamera(ref LogicCameraInfo cameraInfo)
    {
        Vector3 hitPosition;
        bool collidedBack = CollisionFromHeadToPosition(cameraInfo.targetPosition, cameraInfo.CameraPosition(), out hitPosition);
        float targetPivotDistance;

        if (collidedBack)
        {
            targetPivotDistance = (hitPosition - cameraInfo.targetPosition).magnitude;
        }
        else
        {
            targetPivotDistance = cameraInfo.pivotDistance;
        }

        targetPivotDistance = Mathf.Clamp(targetPivotDistance, minDistance, float.MaxValue);

        if (cameraInfo.cameraSwitched)
        {
            currentPivotDistance = targetPivotDistance;
            cameraInfo.pivotDistance = currentPivotDistance;
        }
        else
        {
            float lerpSpeed = targetPivotDistance > currentPivotDistance ? moveOutLerpSpeed : moveInLerpSpeed;

            currentPivotDistance = Mathf.Lerp(currentPivotDistance, targetPivotDistance, Time.deltaTime * lerpSpeed);
            cameraInfo.pivotDistance = currentPivotDistance;
        }
    }

    public override void SetCameraPivot(ref LogicCameraInfo cameraInfo, Vector2 targetPivot)
    {
    }

    public override void SetCameraPivotDistance(ref LogicCameraInfo cameraInfo, float distance)
    {
		currentPivotDistance = distance;
	}

    bool CollisionFromHeadToPosition(Vector3 fromPosition, Vector3 toPosition, out Vector3 hitPosition)
    {
        var hitInfo = new RaycastHit();
        Vector3 direction = (toPosition - fromPosition);
        Ray ray = new Ray(fromPosition, direction.normalized);
        bool didHitSomething = Physics.Raycast(ray, out hitInfo, direction.magnitude, layerMask);

        if (didHitSomething)
        {
            Debug.DrawRay(fromPosition, (hitInfo.point - fromPosition), Color.green);
            hitPosition = hitInfo.point;
            return true;
        }
        else
        {
            Debug.DrawRay(fromPosition, direction, Color.white);
        }

        hitPosition = new Vector3();

        return false;
    }
}
