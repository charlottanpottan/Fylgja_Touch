using UnityEngine;
using System.Collections;

public class GenericEffect : MonoBehaviour
{
	void Start()
	{
	}

	void Update()
	{
	}

	protected void TriggerEffect(GameObject o, Transform effectTransform)
	{
		Instantiate(o, effectTransform.position, effectTransform.rotation);
	}
}
