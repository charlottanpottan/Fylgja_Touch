using UnityEngine;
using System.Collections;

public class CreditsText : MonoBehaviour {
	
	public Color targetColor;
	public float textScale;
	
	public GUIText[] textTargets;
	
	// Use this for initialization
	void LateUpdate () {
		foreach(GUIText text in textTargets)
		{
			text.fontSize = Mathf.RoundToInt(textScale * Screen.width);
			text.material.color = targetColor;
		}
	}
}
