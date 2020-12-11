using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class DummyImplementation: ActorSceneComponentNotification
{
    public GameObject GetActor(string name)
    {
        throw new System.NotImplementedException();
    }

    public SceneActor GetSceneActor(string name)
    {
        throw new System.NotImplementedException();
    }

    public IAvatar GetMainAvatar()
    {
        throw new System.NotImplementedException();
    }

    public AvatarToPlayerNotifications GetPlayerNotifications()
    {
        var notifications = new AvatarToPlayerNotifications();
        notifications.player = new Player();
        notifications.player.playerInteraction = new PlayerInteraction();

        return notifications;
    }

    public FadeInFadeOut FadeInOut()
    {
        throw new System.NotImplementedException();
    }

    public GameObject GetGameObject()
    {
        throw new System.NotImplementedException();
    }

    public bool IsResuming()
    {
        throw new System.NotImplementedException();
    }
}

public class DebugMoviePlayer : MonoBehaviour
{
    public StartMovieQuestPart startMovie;
    
    // Start is called before the first frame update
    void Start()
    {
        startMovie.ActInScene(new DummyImplementation());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
