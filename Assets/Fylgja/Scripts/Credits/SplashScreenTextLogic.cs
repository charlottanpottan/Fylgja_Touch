using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SplashScreenTextLogic : MonoBehaviour {
	
	private float screenWidth;
	private Rect[] guiTextureSizes;
	public Image[] guiTextures;
	
	// Use this for initialization
	void Start () 
	{
		guiTextureSizes = new Rect[guiTextures.Length];
		for(int i = 0; i < guiTextures.Length; i++)
		{
			//guiTextureSizes[i] = guiTextures[i].pixelInset;
		}
		FixResolution();
	}
	
	// Update is called once per frame
	void LateUpdate () 
	{
		if(screenWidth != Screen.width)
		{
			FixResolution();	
		}
	}
	
	void FixResolution()
	{
		screenWidth = Screen.width;

		for(int i = 0; i < guiTextures.Length; i++)
		{
			//guiTextures[i].pixelInset = new Rect(guiTextureSizes[i].x * screenWidth, guiTextureSizes[i].y * screenWidth, guiTextureSizes[i].width * screenWidth, guiTextureSizes[i].height * screenWidth);
		}

	}
}
