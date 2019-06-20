using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StickFightHitButton : MonoBehaviour
{
    public IAvatar Avatar;
    [SerializeField] string message = "WantsToHitLeft";

    void Awake()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClicked);
    }

    void OnButtonClicked()
    {
        Avatar.gameObject.transform.parent.BroadcastMessage(message, SendMessageOptions.DontRequireReceiver);
    }
}
