using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshOnScreenPlacer : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] MeshRenderer borderMeshRenderer;
    [SerializeField] float screenX = 1f;
    [SerializeField] float screenY = 1f;
    [SerializeField] float xOffset = 0;
    [SerializeField] float yOffset = 0;

    void Update()
    {
        if (!cam)
            return;

        Bounds bounds = borderMeshRenderer.bounds;
        Vector2 min = cam.WorldToScreenPoint(bounds.min);
        Vector2 max = cam.WorldToScreenPoint(bounds.max);

        float xBoundsDiff = max.x - min.x;
        float yBoundsDiff = max.y - min.y;

        float z = transform.position.z;
        Vector3 screenPosition = new Vector3((screenX * cam.pixelWidth) + xBoundsDiff * xOffset, (screenY * cam.pixelHeight) + yBoundsDiff * yOffset, 0);
        Vector3 wordPosition = cam.ScreenToWorldPoint(screenPosition);
        wordPosition.z = z;
        transform.position = wordPosition;
    }
}
