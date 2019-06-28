using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class AnimationsWatcherWindow : EditorWindow
{	
	public class SerializationParams
	{
		public SerializedObject instance;
		public SerializedProperty useHierarchySelection;
		public SerializedProperty useSpecificGameObject;
		public SerializedProperty specificGameObject;
		public SerializedProperty showChildren;
		public SerializedProperty showAnimationsNotPlaying;
		public SerializedProperty showShortend;
		public SerializedProperty showNormalizedTime;
		public SerializedProperty sortingMethod;
		public SerializedProperty timeScale;
	}
	public SerializationParams m_serializationParams;

    AnimationsWatcher.ConfigParams m_lastConfig = null;

	void initSerializationParams()
	{
		m_serializationParams = new SerializationParams();
		SerializedObject instance = new SerializedObject(AnimationsWatcher.Instance);
		m_serializationParams.instance = instance;
		m_serializationParams.useHierarchySelection = instance.FindProperty("m_configParams.useHierarchySelection");
		m_serializationParams.useSpecificGameObject = instance.FindProperty("m_configParams.useSpecificGameObject");
		m_serializationParams.specificGameObject = instance.FindProperty("m_configParams.specificGameObject");
		m_serializationParams.showChildren = instance.FindProperty("m_configParams.showChildren");
		m_serializationParams.showAnimationsNotPlaying = instance.FindProperty("m_configParams.showAnimationsNotPlaying");
		m_serializationParams.showShortend = instance.FindProperty("m_configParams.showShortend");
		m_serializationParams.showNormalizedTime = instance.FindProperty("m_configParams.showNormalizedTime");
		m_serializationParams.sortingMethod = instance.FindProperty("m_configParams.sortingMethod");
		m_serializationParams.timeScale = instance.FindProperty("m_configParams.timeScale");
	}
	
    [MenuItem("Window/AnimationsWatcher")]
    static void Init()
    {
        AnimationsWatcherWindow window = (AnimationsWatcherWindow)EditorWindow.GetWindow(typeof(AnimationsWatcherWindow));
        window.name = "AnimationsWatcher";
        window.autoRepaintOnSceneChange = true;
    }

    public void Awake()
    {
        initializeAnimationsWatcher();
    }

    void initializeAnimationsWatcher()
    {
        AnimationsWatcher.initialize();
        if (m_lastConfig == null)
        {
            m_lastConfig = AnimationsWatcher.configParams.Copy();
        }
        else
        {
            AnimationsWatcher.Instance.m_configParams = m_lastConfig.Copy();
        }
        initSerializationParams();
    }
	/// <summary>
	/// ConfigGUI is used to paint the configuration options foldout 
	/// </summary>
    void ConfigGUI()
    {
        GUILayout.Label("Selection", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        m_serializationParams.useHierarchySelection.boolValue = EditorGUILayout.Toggle(m_serializationParams.useHierarchySelection.boolValue, GUILayout.Width(25));
        GUILayout.Label("Hierarchy selection");
        if (m_serializationParams.useHierarchySelection.boolValue == true)
        {
            //AnimationsWatcher.configParams.useSpecificGameObject = false;
			m_serializationParams.useSpecificGameObject.boolValue  = false;
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        m_serializationParams.useSpecificGameObject.boolValue = EditorGUILayout.Toggle(m_serializationParams.useSpecificGameObject.boolValue, GUILayout.Width(25));
        GameObject newTargetGameObject = EditorGUILayout.ObjectField("Specific target: ", m_serializationParams.specificGameObject.objectReferenceValue, typeof(GameObject), true) as GameObject;
        if (newTargetGameObject != m_serializationParams.specificGameObject.objectReferenceValue)
        {
            m_serializationParams.useSpecificGameObject.boolValue = true;
			m_serializationParams.specificGameObject.objectReferenceValue = newTargetGameObject;
        }
        if (m_serializationParams.useSpecificGameObject.boolValue == true && m_serializationParams.specificGameObject.objectReferenceValue != null)
        {
            m_serializationParams.useHierarchySelection.boolValue = false;
        }
		else
		{
			m_serializationParams.useSpecificGameObject.boolValue = false;
		}
        EditorGUILayout.EndHorizontal();

        if (m_serializationParams.useHierarchySelection.boolValue == false && m_serializationParams.useSpecificGameObject.boolValue == false)
        {
            m_serializationParams.useHierarchySelection.boolValue = true;
        }

        EditorGUILayout.Separator();

        GUILayout.Label("Display", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        m_serializationParams.showChildren.boolValue = EditorGUILayout.Toggle(m_serializationParams.showChildren.boolValue, GUILayout.Width(25));
        GUILayout.Label("Show with children");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        m_serializationParams.showAnimationsNotPlaying.boolValue = EditorGUILayout.Toggle(m_serializationParams.showAnimationsNotPlaying.boolValue, GUILayout.Width(25));
        GUILayout.Label("Show animations not playing");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        m_serializationParams.showShortend.boolValue = EditorGUILayout.Toggle(m_serializationParams.showShortend.boolValue, GUILayout.Width(25));
        GUILayout.Label("Use shortend labels");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        m_serializationParams.showNormalizedTime.boolValue = EditorGUILayout.Toggle(m_serializationParams.showNormalizedTime.boolValue, GUILayout.Width(25));
        GUILayout.Label("Show animation time");
        EditorGUILayout.EndHorizontal();
	}
	
	/// <summary>
	/// MoreOptionsGUI is used to paint the additional options
	/// </summary>
    void MoreOptionsGUI()
    {
        EditorGUILayout.Separator();
        //AnimationsWatcher.configParams.sortingMethod = EditorGUILayout.IntPopup("Sorting", AnimationsWatcher.configParams.sortingMethod, AnimationsWatcher.configParams.sortOptions, new int[3]{0,1,2});
		EditorGUILayout.IntPopup(m_serializationParams.sortingMethod, AnimationsWatcher.configParams.sortOptionsText, AnimationsWatcher.configParams.sortOptionsInt, new GUIContent("Sorting"));
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Time scale");
        float newTimeScale = EditorGUILayout.Slider(m_serializationParams.timeScale.floatValue, -1.0f, 1.0f);
        if (newTimeScale != AnimationsWatcher.configParams.timeScale)
        {
            m_serializationParams.timeScale.floatValue = newTimeScale;
            Time.timeScale = newTimeScale;
        }
        EditorGUILayout.EndHorizontal();
    }
	
	/// <summary>
	/// AnimationStateGUI is used to paint an animation state data (i.e, layer, weight etc.)
	/// </summary>
	void AnimationStateGUI(AnimationState animState, Animation animComponent)
	{
		string layerText;
		string speedText;
		string wrapModeText;
		string weightText;
		string blendModeText;
		string normalizedTimeText;
		
		if (AnimationsWatcher.configParams.showShortend == true)
		{
			layerText = "L: ";
			speedText = "Sp: ";
			wrapModeText = "WM: ";
			weightText = "W: ";
			blendModeText = "BM: ";
			normalizedTimeText = "T: ";
		}
		else
		{
			layerText = "layer: ";
			speedText = "speed: ";
			wrapModeText = "wrap mode: ";
			weightText = "weight: ";
			blendModeText = "blend mode: ";
			normalizedTimeText = "time: ";
		}
		
		EditorGUILayout.BeginHorizontal();
        GUILayout.Label(animState.name, GUILayout.MinWidth(120.0f), GUILayout.MaxWidth(120.0f));
		GUILayout.Label(layerText + animState.layer);
		GUILayout.Label(speedText + animState.speed);
		GUILayout.Label(blendModeText + animState.blendMode);
		GUILayout.Label(wrapModeText + animState.wrapMode);
		EditorGUILayout.EndHorizontal();
        Rect layoutRect = EditorGUILayout.BeginHorizontal();
		EditorGUILayout.Toggle(animComponent.IsPlaying(animState.name), GUILayout.Width(25.0f));
		layoutRect.xMin += 25.0f;
        EditorGUI.ProgressBar(layoutRect, animState.weight, weightText + string.Format("{0:0.##}", animState.weight));
        EditorGUILayout.EndHorizontal();
		if (AnimationsWatcher.configParams.showNormalizedTime == true)
		{
	        layoutRect = EditorGUILayout.BeginHorizontal();
			GUILayout.Label("", GUILayout.Width(25.0f));
			layoutRect.xMin += 25.0f;
	        EditorGUI.ProgressBar(layoutRect, animState.normalizedTime, normalizedTimeText + string.Format("{0:0.##}", animState.normalizedTime));
	        EditorGUILayout.EndHorizontal();
		}
	}
			
    void OnGUI()
    {
        if (AnimationsWatcher.Instance == null)
        {
            initializeAnimationsWatcher();
        }
		m_serializationParams.instance.Update();
		// Paint configurations
        AnimationsWatcher.configParams.showConfig = EditorGUILayout.Foldout(AnimationsWatcher.configParams.showConfig, "Configuration");
        if (AnimationsWatcher.configParams.showConfig == true)
        {
            ConfigGUI();
        }
        MoreOptionsGUI();
		
		m_serializationParams.instance.ApplyModifiedProperties();

        m_lastConfig = AnimationsWatcher.configParams.Copy();

		// Determine which game objects to show
        List<GameObject> targetGameObjects = new List<GameObject>();
        if (AnimationsWatcher.configParams.useHierarchySelection == true)
        {
			// Traverse the inspector selected objects
			foreach (Object obj in Selection.objects)
			{
				if (obj.GetType() == typeof(GameObject))
				{
            		targetGameObjects.Add((GameObject)obj);
				}
			}
        }
        if (AnimationsWatcher.configParams.useSpecificGameObject == true)
        {
			// Just use the single specified user game object
            targetGameObjects.Add(AnimationsWatcher.configParams.specificGameObject);
        }
		
		// If no object selected or user didn't specify any object then finish quietly
        if (targetGameObjects.Count == 0)
            return;
		
		// Prepare a list with all game objects which have an animation component
		List<GameObject> allGameObjects = new List<GameObject>();
		foreach (GameObject gameObject in targetGameObjects)
		{
        	allGameObjects.AddRange(AnimationsWatcher.GetGameObjectsWithAnimation(gameObject, AnimationsWatcher.configParams.showChildren));
		}
        if (allGameObjects.Count == 0)
        {
            GUILayout.Label("Selected game object(s)", EditorStyles.boldLabel);
            GUILayout.Label("doesn't have animation component", EditorStyles.boldLabel);			
        }
		
		// And now traverse the all apropriate game objects and paint their GUI
        EditorGUILayout.Separator();
		AnimationsWatcher.configParams.scrollPos = EditorGUILayout.BeginScrollView(AnimationsWatcher.configParams.scrollPos);
        foreach (GameObject currentObject in allGameObjects)
        {
            GUILayout.Label(currentObject.name, EditorStyles.boldLabel);
            IComparer<AnimationState> comparer = new AnimationsWatcher.LayerSort();
            switch(AnimationsWatcher.configParams.sortingMethod)
            {
                case 0:
                    comparer = new AnimationsWatcher.LayerSort();
                    break;
                case 1:
                    comparer = new AnimationsWatcher.NameSort();
                    break;
                case 2:
                    comparer = new AnimationsWatcher.WeightSort();
                    break;
            }
            List<AnimationState> objectAnimStates = AnimationsWatcher.GetAnimationsSorted(currentObject.GetComponent<Animation>(), comparer, AnimationsWatcher.configParams.showAnimationsNotPlaying);
            foreach (AnimationState animState in objectAnimStates)
            {
				AnimationStateGUI(animState, currentObject.GetComponent<Animation>());
            }
        }
		EditorGUILayout.EndScrollView();
    }
}
