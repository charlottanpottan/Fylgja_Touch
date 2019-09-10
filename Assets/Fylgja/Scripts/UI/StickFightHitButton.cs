using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StickFightHitButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public IAvatar Avatar;
    [SerializeField] string message = "WantsToHitLeft";
    [SerializeField] string duckMessage = "WantsToDuck";
    [SerializeField] float timeBeforeDuck = 0.3f;

    bool mouseDown = false;
    float timeSinceMouseDown = 0;
    bool sentDuckMessage = false;

    public void OnPointerDown(PointerEventData ped)
    {
        mouseDown = true;
        timeSinceMouseDown = 0;
    }
    public void OnPointerUp(PointerEventData ped)
    {
        mouseDown = false;
        if (!sentDuckMessage)
            Avatar.gameObject.transform.parent.BroadcastMessage(message, SendMessageOptions.DontRequireReceiver);
        sentDuckMessage = false;
    }

    void Update()
    {
        if (!mouseDown)
            return;

        timeSinceMouseDown += Time.deltaTime;
        if (!sentDuckMessage && timeSinceMouseDown > timeBeforeDuck)
        {
            Avatar.gameObject.transform.parent.BroadcastMessage(duckMessage, SendMessageOptions.DontRequireReceiver);
            sentDuckMessage = true;
        }
    }
}
