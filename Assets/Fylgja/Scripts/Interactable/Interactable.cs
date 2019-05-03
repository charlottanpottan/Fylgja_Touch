using UnityEngine;

public class Interactable : MonoBehaviour
{
	public Renderer rendererToHighlight;
	public int materialIndex = 0;
	public bool canInteractFromAnyDistance = false;
	public bool trigActionOnDown = false;

	private Color initialColor;
	private bool interactionEnabled = true;
	private bool isHighlighted = false;

	void Start()
	{
		if (rendererToHighlight != null)
		{
			initialColor = rendererToHighlight.materials[materialIndex].color;
		}
	}

	public virtual bool TrigActionOnDown()
	{
		return trigActionOnDown;
	}

	public static Interactable GetInteractableFromName(string name)
	{
		var interactableObjects = GameObject.FindGameObjectsWithTag("Interactable");
		foreach (var interactableObject in interactableObjects)
		{
			if (interactableObject.name == name)
			{
				return interactableObject.GetComponentInChildren<Interactable>();
			}
		}

		Debug.LogError("Couldn't find interactable:" + name);
		return null;
	}

	public void StartMouseOverEffect()
	{
		const float intensity = 2.0f;
		
		if(rendererToHighlight != null)
		{
			rendererToHighlight.materials[materialIndex].color = new Color(intensity, intensity, intensity, 1.0f);
		}
		
		isHighlighted = true;
	}

	public void StopMouseOverEffect()
	{
		if(rendererToHighlight != null)
		{
			rendererToHighlight.materials[materialIndex].color = initialColor;
		}
		isHighlighted = false;
	}

	public void OnInteractionMouseEnter()
	{
		setHilight(true);
	}

	public void OnInteractionMouseExit()
	{
		setHilight(false);
	}

	public void setHilight(bool hilight)
	{
		if (hilight && !isHighlighted)
		{
			StartMouseOverEffect();
		}
		else if (!hilight && isHighlighted)
		{
			StopMouseOverEffect();
		}
	}

	public void DisableInteraction()
	{
		interactionEnabled = false;
	}

	public void EnableInteraction()
	{
		interactionEnabled = true;
	}

	public bool IsInteractionEnabled()
	{
		return interactionEnabled;
	}
}
