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

    public override void UpdateCamera(ref LogicCameraInfo info)
    {
        Vector3 hitPosition;
        bool collidedBack = CollisionFromHeadToPosition(info.targetPosition, info.CameraPosition(), out hitPosition);
        float targetPivotDistance;

        if (collidedBack)
        {
            targetPivotDistance = (hitPosition - info.targetPosition).magnitude;
        }
        else
        {
            targetPivotDistance = info.pivotDistance;
        }

        targetPivotDistance = Mathf.Clamp(targetPivotDistance, minDistance, float.MaxValue);

        if (info.cameraSwitched)
        {
            currentPivotDistance = targetPivotDistance;
            info.pivotDistance = currentPivotDistance;
        }
        else
        {
            float lerpSpeed = targetPivotDistance > currentPivotDistance ? moveOutLerpSpeed : moveInLerpSpeed;

            currentPivotDistance = Mathf.Lerp(currentPivotDistance, targetPivotDistance, Time.deltaTime * lerpSpeed);
            info.pivotDistance = currentPivotDistance;
        }
    }

    public override void SetCameraPivot(ref LogicCameraInfo cameraInfo, Vector2 targetPivot)
    {
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
