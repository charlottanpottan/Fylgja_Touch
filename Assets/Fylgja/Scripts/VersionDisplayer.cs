using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionDisplayer : MonoBehaviour
{
    [SerializeField] int version;

    void OnGUI()
    {
        GUILayout.Space(30);
        GUILayout.Label("Version " + version);
    }
}
