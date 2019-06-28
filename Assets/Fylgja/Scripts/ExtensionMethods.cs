using UnityEngine;
using System.Collections;

public static class ExtensionMethods
{
    // SetActiveRecursively is deprecated but using SetActive in all places where 
    // SetActiveRecursively is used created a lot of bugs. 
    // Using this function until there is time to look more into this.
    public static void SetActiveRecursively1(this GameObject rootObject, bool active)
    {
        rootObject.SetActive(active);
        foreach (Transform childTransform in rootObject.transform)
        {
            SetActiveRecursively1(childTransform.gameObject, active);
        }
    }
}