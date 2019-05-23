using UnityEngine;
using System.Collections;

public class FireTrigger : MonoBehaviour
{
    public GameObject[] objectsToActivate;
    public AnimationClip animationToPlay;

    // Use this for initialization
    void Start()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FireTrigger"))
        {
            ActivateObjects();
        }
    }

    public void ActivateObjects()
    {
        foreach (GameObject go in objectsToActivate)
        {
            GetComponent<Animation>().Play(animationToPlay.name);
            if (!go)
                Debug.Log("no go to activate on FireTrigger!!!!!!!!!!!!!!!!!!!!!!!!!!!............... " + name + " parent " + transform.parent.name);
            else
                go.active = true;
        }
    }

    public void DeactivateObjects()
    {
        foreach (GameObject go in objectsToActivate)
        {
            if (!go)
                Debug.Log("no go to deactivate on FireTrigger!!!!!!!!!!!!!!!!!!!!!!!!!!!............... " + name + " parent " + transform.parent.name);
            else
                go.active = false;
        }
    }
}
