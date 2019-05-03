#define DEBUG

using System;
using UnityEngine;

public class DebugUtilities
{
	public static void Assert(bool condition, string comment)
	{
#if DEBUG
		if (!condition)
		{
			Debug.LogError(comment);
			throw new Exception(comment);
		}
#endif
	}
}
