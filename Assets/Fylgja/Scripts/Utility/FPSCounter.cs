using UnityEngine;
using System.Collections;

public class FPSCounter : MonoBehaviour
{
	private TextMesh targetText;

	void Awake()
	{
		targetText = GetComponent<TextMesh>();
	}

	void LateUpdate()
	{
		targetText.text = "Fps:" + 1 / Time.smoothDeltaTime;
	}
}
