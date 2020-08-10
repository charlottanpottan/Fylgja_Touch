using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneHaxx : MonoBehaviour
{
    // object is set active from LevelCheckActivator even though is should be inactive
    void OnEnable()
    {
        gameObject.SetActive(false);
    }
}
