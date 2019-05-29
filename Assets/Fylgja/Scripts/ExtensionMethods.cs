using UnityEngine;
using System.Collections;

public static class ExtensionMethods
{
    public static void SetActiveRecursively1(this GameObject rootObject, bool active)
    {
        rootObject.SetActive(active);
        foreach (Transform childTransform in rootObject.transform)
        {
            SetActiveRecursively1(childTransform.gameObject, active);
        }
    }
}