using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugStartScene : MonoBehaviour
{
    public Quest questToTest;

    public AvatarToPlayerNotifications avatarToPlayerNotifications;
    // Start is called before the first frame update
    void Start()
    {
        var playerNotifications = GameObject.FindObjectOfType<AvatarToPlayerNotifications>();
        questToTest.Resume(); // Not sure why it is needed?
        questToTest.PlayScene(avatarToPlayerNotifications);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
