using UnityEngine;

public class NpcState
{
	protected Npc npc;
	public NpcState nextState = null;

	public NpcState(Npc npc)
	{
		this.npc = npc;
	}

	public virtual NpcState OnTalkRequest(IAvatar avatar)
	{
		return null;
	}
}