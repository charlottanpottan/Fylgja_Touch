using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour
{
	public GameObject menuObject;
	public bool entryMenu;
	public Player player;
	
	AllowedToInteractModifier allowedToInteract;
	
	void Awake()
	{
		if (!entryMenu)
		{
			ContinueGame();
		}
	}

	void Update()
	{
		if (!entryMenu && Input.GetButtonDown("menuToggle"))
		{
			ToggleMenu();
		}
	}
	
	void ShowMenu(bool on)
	{
		menuObject.SetActiveRecursively1(on);
	}

	public void ToggleMenu()
	{
		Debug.Log("Toggle menu");
		if (menuObject.active != true)
		{
			PauseGame();
		}
		else
		{
			ResumeGame();
		}
	}
	
	void PauseGame()
	{
		ShowMenu(true);
		Time.timeScale = 0.0f;
		DebugUtilities.Assert(allowedToInteract == null, "Was already restricting interaction");
		allowedToInteract = new AllowedToInteractModifier();
		player.AssignedAvatar().AddAllowedToInteractModifier(allowedToInteract);
		player.playerInteraction.OnAllowedToUseUI(true);
	}
	
	void ResumeGame()
	{
		player.AssignedAvatar().RemoveAllowedToInteractModifier(allowedToInteract);
		allowedToInteract = null;
		ContinueGame();
	}
	
	void ContinueGame()
	{
		ShowMenu(false);
		Time.timeScale = 1.0f;
		player.playerInteraction.OnAllowedToUseUI(false);
	}
}