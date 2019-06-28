using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerLevelLoader : MonoBehaviour
{
	public PlayerStorage playerStorage;


	public void NewGame()
	{
		playerStorage.NewGame();
		Load();
	}

	public void ContinueGame()
	{
		Load();
	}

	public void ContinueFromCheckpoint(CheckpointId checkpointId)
	{
		playerStorage.SetStartCheckpoint(checkpointId);
		Load();
	}

	void Load()
	{
		Global.levelId = playerStorage.LevelIdFromCheckpoint(playerStorage.playerData().startingCheckpointId);
        SceneManager.LoadScene("Loading");
	}
}
