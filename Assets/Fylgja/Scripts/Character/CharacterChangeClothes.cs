using UnityEngine;
using System.Collections;

[System.Serializable]
public class Clothes
{
	public string clothesName;
	public Material[] clothesMaterials;
	public Mesh clothesMesh;
}

public class CharacterChangeClothes : MonoBehaviour {
	
	public SkinnedMeshRenderer targetRenderer;
	public Clothes[] clothes;
	
	void Start()
	{
		ChangeClothes(Application.loadedLevelName);
	}
	
	void ChangeClothes(string targetClothes)
	{
		foreach(Clothes c in clothes)
		{
			if(c.clothesName == targetClothes)
			{
				targetRenderer.materials = c.clothesMaterials;
				targetRenderer.sharedMesh = c.clothesMesh;
			}
		}
	}
	
	
	
}
