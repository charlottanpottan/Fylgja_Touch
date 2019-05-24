using UnityEngine;

public class Billboard : MonoBehaviour
{
    public void OnEnable()
    {
        Camera.onPostRender += OnCameraPostRender;
    }

    public void OnDisable()
    {
        Camera.onPostRender -= OnCameraPostRender;
    }

    public void OnCameraPostRender(Camera cam)
    {
        if (cam.tag != "MainCamera")
            return;
        transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, Vector3.up);
    }
}