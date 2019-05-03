using UnityEngine;
using System.Collections;

public class CharacterFirePitMinigame : MonoBehaviour
{
	public GameObject torch;
	public GameObject birchFan;
	public GameObject minigameDone;
	bool toolsAreActivated = false;
	bool waitingForMinigameDoneAnimation;

	void Start()
	{
	}

	void Update()
	{
	}

	public void SetEquipmentEnable(bool enable)
	{
		torch.SetActiveRecursively(enable);
		birchFan.SetActiveRecursively(enable);
		toolsAreActivated = enable;
	}

	public bool HasFirePitTools()
	{
		return toolsAreActivated;
	}

	void OnFirePitMinigameStart()
	{
		SetEquipmentEnable(true);
	}

	public bool AreToolsActivated()
	{
		return toolsAreActivated;
	}

	void OnFirePitMinigameComplete()
	{
		SetEquipmentEnable(false);
//		minigameDone.endFunction = OnCutscenePlayed;
//		minigameDone.PlayCutscene();
	}

	void OnCutscenePlayed()
	{
	}

	void OnMinigameAborted(Minigame game)
	{
		SetEquipmentEnable(false);
	}
}

