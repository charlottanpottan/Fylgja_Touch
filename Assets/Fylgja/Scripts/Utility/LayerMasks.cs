using UnityEngine;

public class LayerMasks
{
	public static LayerMask Default = 1 << Layers.Default;
	public static LayerMask Interactables = 1 << Layers.Interactables;
	public static LayerMask Disabled = 1 << Layers.Disabled;
	public static LayerMask Ground = 1 << Layers.Ground;
	public static LayerMask Player = 1 << Layers.Player;
	public static LayerMask Hud = 1 << Layers.Hud;

	public LayerMasks()
	{
	}
}
