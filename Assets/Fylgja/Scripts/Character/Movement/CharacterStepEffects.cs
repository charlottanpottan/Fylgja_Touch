using UnityEngine;
using System.Collections;

public class CharacterStepEffects : MonoBehaviour
{
	public AudioHandler groundSounds;
	public AudioHandler waterSounds;
	public GameObject waterEffect;
	public LayerMask mask;
	public int effectLayer;

	// Use this for initialization
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
	}

	public void OnFootStrike(string materialName)
	{
		if (materialName == "water")
		{
			waterSounds.TriggerSound();
			RaycastHit hit;
			if(Physics.Raycast(transform.root.position + Vector3.up * 5f, -Vector3.up, out hit, 10f, mask))
			{
				if(hit.collider.gameObject.layer == effectLayer)
				{
					Instantiate(waterEffect, hit.point, Quaternion.identity);
				}
			}
		}
		else
		{
			groundSounds.TriggerSound();
		}
	}
}

