public class AllowedToMoveModifier
{
	string debugName;
	public AllowedToMoveModifier(string _debugName)
	{
		debugName = _debugName;
	}
	
	public override string ToString()
	{
		return debugName;
	}
}

