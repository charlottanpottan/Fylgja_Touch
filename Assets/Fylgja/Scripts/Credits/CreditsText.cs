using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreditsText : MonoBehaviour {

	public Color targetColor;
	public float textScale;

	public Text[] textTargets;

	// Use this for initialization
	void LateUpdate () {
		foreach(var text in textTargets)
		{
			text.fontSize = Mathf.RoundToInt(textScale * Screen.width);
			text.material.color = targetColor;
		}
	}
}
