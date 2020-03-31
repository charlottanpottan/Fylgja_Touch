using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AvatarMessageSender : MonoBehaviour, IPointerDownHandler
{
    public IAvatar Avatar;
    [SerializeField] string message = "WantsToDuck";

    public void OnPointerDown(PointerEventData ped)
    {
        Avatar.gameObject.transform.parent.BroadcastMessage(message, SendMessageOptions.DontRequireReceiver);
    }
}
