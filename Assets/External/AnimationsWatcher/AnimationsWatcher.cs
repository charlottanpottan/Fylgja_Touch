using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AnimationsWatcher : ScriptableObject
{
	[System.Serializable]
	public class ConfigParams
	{
		public bool showConfig = true;
		public bool useHierarchySelection = true;
		public bool useSpecificGameObject = false;
		public GameObject specificGameObject = null;
		public bool showChildren = false;
		public bool showAnimationsNotPlaying = false;
		public bool showShortend = false;
		public bool showNormalizedTime = false;
		public int sortingMethod = 0;
		public GUIContent[] sortOptionsText = {
		   new GUIContent("Sort by layer"),
		   new GUIContent("Sort by name"),
		   new GUIContent("Sort by weight"),
		};
		public int[] sortOptionsInt = {0, 1, 2};
		
		public float timeScale = 1.0f;
		public Vector2 scrollPos = Vector2.zero;

        public ConfigParams Copy()
        {
            ConfigParams copy = new ConfigParams();
            copy.showConfig = showConfig;
            copy.useHierarchySelection = useHierarchySelection;
            copy.useSpecificGameObject = useSpecificGameObject;
            copy.specificGameObject = specificGameObject;
            copy.showChildren = showChildren;
            copy.showAnimationsNotPlaying = showAnimationsNotPlaying;
            copy.showShortend = showShortend;
            copy.showNormalizedTime = showNormalizedTime;
            copy.sortingMethod = sortingMethod;
            copy.timeScale = timeScale;
            copy.scrollPos = scrollPos;
            return copy;
        }
    
    }
	
	public ConfigParams m_configParams = new ConfigParams();
		
	public static ConfigParams configParams
	{
		get
		{
			if (Instance == null)
			{
				initialize();
			}
			return Instance.m_configParams;
		}
	}
	public static AnimationsWatcher Instance = null;
	
	public static void initialize()
	{
		if (Instance == null)
		{
			Instance = CreateInstance(typeof(AnimationsWatcher)) as AnimationsWatcher;
		}
	}

	
	/// <summary>
	/// NameSort, LayerSort and WeightSort are used to sort a list of animation states by name, 
	/// layer or weight respectively.
	/// </summary>
    public class NameSort : IComparer<AnimationState>
    {
        public int Compare(AnimationState animState1, AnimationState animState2)
        {
            return animState1.name.CompareTo(animState2.name);
        }
    }

    public class LayerSort : IComparer<AnimationState>
    {
        public int Compare(AnimationState animState1, AnimationState animState2)
        {
            return animState2.layer.CompareTo(animState1.layer);
        }
    }

    public class WeightSort : IComparer<AnimationState>
    {
        public int Compare(AnimationState animState1, AnimationState animState2)
        {
            return animState2.weight.CompareTo(animState1.weight);
        }
    }
	
	/// <summary>
	/// GetAnimationsSorted returns all animation states of an animation component sorted on demand.
	/// Unless the includeAll parameter is checked only animation state which clip is being played will be 
	/// included in the returned list. Sorting function is one of LayerSort, NameSort or WeightSort.
	/// </summary>
    public static List<AnimationState> GetAnimationsSorted(Animation animComponent, IComparer<AnimationState> comparer, bool includeAll)
    {
        List<AnimationState> animations = new List<AnimationState>();
        foreach (AnimationState animState in animComponent)
        {
            if (includeAll == true || animComponent.IsPlaying(animState.name))
            {
                animations.Add(animState);
            }
        }
        animations.Sort(comparer);
        return animations;
    }
	
	/// <summary>
	/// GetGameObjectsWithAnimation returns a list of game objects which have an animation componet.
	/// The search starts from a root game object and then continue verify its children.
	/// If includeChildren is false then only the root object will be verified.
	/// </summary>
    public static List<GameObject> GetGameObjectsWithAnimation(GameObject root, bool includeChildren)
    {
        List<GameObject> animatedObjects = new List<GameObject>();
        if (root.animation != null)
        {
            animatedObjects.Add(root);
        }
        if (includeChildren == true)
        {
            foreach (Transform child in root.transform)
            {
                animatedObjects.AddRange(GetGameObjectsWithAnimation(child.gameObject, includeChildren));
            }
        }
        return animatedObjects;
    }
}
