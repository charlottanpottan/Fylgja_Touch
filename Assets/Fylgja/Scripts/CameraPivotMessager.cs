using UnityEngine;
using System.Collections;

public class CameraPivotMessager : MonoBehaviour
{
    LogicCameraInfoApplicator logicCameraInfoApplicator;

    void Awake()
    {
        logicCameraInfoApplicator = Camera.main.gameObject.GetComponent<LogicCameraInfoApplicator>();
    }

    void SetPivot(Vector2 targetPivot)
    {
        logicCameraInfoApplicator.CameraSetPivot(targetPivot);
    }

    void SetPivotDistance(float distance)
    {
        logicCameraInfoApplicator.CameraSetPivotDistance(distance, false);
    }

    void SetPivotDistanceInstant(float distance)
    {
        logicCameraInfoApplicator.CameraSetPivotDistance(distance, true);
    }
}
