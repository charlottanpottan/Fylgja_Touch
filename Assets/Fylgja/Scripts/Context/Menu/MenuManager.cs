using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
	public GameObject menuObject;
	public bool entryMenu;
	public Player player;
	[SerializeField] Button openMainMenuButton = null;

	AllowedToInteractModifier allowedToInteract;
	
	void Awake()
	{
		openMainMenuButton.onClick.AddListener(ToggleMenu);
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
		if(on)
			openMainMenuButton.gameObject.SetActive(false);
		else
			openMainMenuButton.gameObject.SetActive(SystemInfo.deviceType == DeviceType.Handheld);
	}

	public void ToggleMenu()
	{
		Debug.Log("Toggle menu");
		if (menuObject.activeSelf != true)
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