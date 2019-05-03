using UnityEngine;
using System.Collections;

public class StartActorScene : MonoBehaviour
{
	public GameObject actorScenePrefab;

	void Start()
	{
		if(GetComponent<Collider>() == null)
		{
			Initiate();
		}
	}
	
	void OnTriggerEnter()
	{
		Initiate();
		Destroy(GetComponent<Collider>());
	}
	
	void Initiate()
	{
		var mainActorObject = GameObject.FindGameObjectWithTag("Player");
		DebugUtilities.Assert(mainActorObject != null, "Couldn't fint IAvatar from tag Player");
		var avatar = mainActorObject.GetComponentInChildren<IAvatar>();
		DebugUtilities.Assert(avatar != null, "Found an object with tag Player but didn't have an actor.");
		var actorScene = ActorSceneUtility.CreateSceneWithAvatar(actorScenePrefab, avatar);
		actorScene.PlayScene(avatar.playerNotifications);
	}
}
