using UnityEngine;
using System.Collections;

public class MouseCheckboxEffects : MonoBehaviour
{
	public MouseMenuEffects targetMenuEffects;
	
	void OnMouseEnter()
	{
		targetMenuEffects.mouseOver = true;
	}
	
	void OnMouseExit()
	{
		targetMenuEffects.mouseOver = false;
	}
}