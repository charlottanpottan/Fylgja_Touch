using UnityEngine;
using System.Collections.Generic;

public class ActionUtility : MonoBehaviour
{
	static public bool IsAnyActionPossible(GameObject obj, IAvatar avatar)
	{
		return ActionsPossible(obj, avatar).Count > 0;
	}

	static public Action FindPrimaryAction(GameObject obj, IAvatar avatar)
	{
		var actions = ActionsPossible(obj, avatar);

		if (actions.Count == 0)
		{
			return null;
		}
		else
		{
			return actions[0];
		}
	}

	static public List<Action> ActionsPossible(GameObject obj, IAvatar avatar)
	{
		var possibleActions = new List<Action>();

		var actions = obj.GetComponentsInChildren<Action>();

		foreach (var action in actions)
		{
			if (action.arbitration == null || action.arbitration.IsActionPossible(avatar))
			{
				possibleActions.Add(action);
			}
		}
		return possibleActions;
	}
}

