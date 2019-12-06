using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapPlacer : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] MeshRenderer borderMeshRenderer;

    void Update()
    {
        if (!cam)
            return;

        Bounds bounds = borderMeshRenderer.bounds;
        Vector2 min = cam.WorldToScreenPoint(bounds.min);
        Vector2 max = cam.WorldToScreenPoint(bounds.max);

        float diff = max.y - min.y;

        float z = transform.position.z;
        Vector3 screenPosition = new Vector3(cam.pixelWidth - diff / 1.8f, cam.pixelHeight - diff / 1.8f, 0);
        Vector3 wordPosition = cam.ScreenToWorldPoint(screenPosition);
        wordPosition.z = z;
        transform.position = wordPosition;
    }
}
