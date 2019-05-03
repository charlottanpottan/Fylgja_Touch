using UnityEngine;
using System.Collections;

public class LevelId
{
	private int levelId;

	public LevelId(int id)
	{
		DebugUtilities.Assert(id == 0 || id == 1 || id == 2, "Illegal LevelId");
		levelId = id;
	}

	private LevelId()
	{
	}

	public string levelName()
	{
		switch (levelId)
		{
		case 0:
			return "BronzeAge";
		case 1:
			return "IronAge";
		case 2:
			return "Credits";
		}
		Debug.LogError("Illegal LevelId");
		return "";
	}

	public int levelIdValue()
	{
		return levelId;
	}
}
