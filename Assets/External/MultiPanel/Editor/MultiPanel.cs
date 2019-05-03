using UnityEngine;
using UnityEditor;
using System.Collections;

public class MultiPanel : EditorWindow
{
	string addComponentString = "";
	string removeComponentString = "";
	string theLayer = "";
	string theRenameName = "";
	string theRenameNameSuf = "";
	string theYPos = "5.0";
	string newRenderQueue = "";

	//Adding a menu entry
	[MenuItem("Tools/MultiPanel")]
	
	static void Init ()
	{
		// Get existing open window or if none, make a new one:
		MultiPanel window = (MultiPanel)EditorWindow.GetWindow (typeof(MultiPanel));
		window.Show ();
	}
	
	/*
	 * GUI Generation
	 */		
	void OnGUI ()
	{
		GUILayout.Label ("MultiPanel by Owlchemy Labs", EditorStyles.boldLabel);
		GUILayout.Label (" Operations for multiple selected objects", EditorStyles.miniLabel);
		GUILayout.Space (15);
		
		EditorGUILayout.BeginHorizontal ();
		
		/*
	 	* LEFT COLUMN
	 	*/	
		EditorGUILayout.BeginVertical ();
		GUILayout.Space (5);
		
		GUILayout.Label ("Reset Transform");
		
		GUILayout.Space (5);
		
		GUILayout.Label ("Toggle Renderers");
		
		GUILayout.Space (5);
		
		GUILayout.Label ("Set Active Recursively");
		
		GUILayout.Space (15);
		
		GUILayout.Label ("Add Component");
		
		addComponentString = EditorGUILayout.TextField (addComponentString);
		
		GUILayout.Space (10);
		
		GUILayout.Label ("Remove Component");
		
		removeComponentString = EditorGUILayout.TextField (removeComponentString);
		
		GUILayout.Space (25);
		
		GUILayout.Label ("Layer and Tag Assignment");
		theLayer = EditorGUILayout.TextField (theLayer);
		
		GUILayout.Space (5);
		GUILayout.Label ("Rename exactly");
		theRenameName = EditorGUILayout.TextField (theRenameName);
		
		GUILayout.Space (5);
		GUILayout.Label ("Rename w/ numerical suffix");
		theRenameNameSuf = EditorGUILayout.TextField (theRenameNameSuf);
		
		GUILayout.Space (5);
		
		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label ("Set render queue");
		GUILayout.Label ("Default: 3000 ", EditorStyles.miniLabel);
		EditorGUILayout.EndHorizontal ();

		newRenderQueue = EditorGUILayout.TextField (newRenderQueue);
		
		EditorGUILayout.EndVertical ();
		
		/*
	 	* RIGHT COLUMN
	 	*/
		EditorGUILayout.BeginVertical ();
		GUILayout.Space (5);
		
		
		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("World")) {
			ZeroOutTransform ();
		}
		if (GUILayout.Button ("Local")) {
			InheritParentTransform ();
		}
		EditorGUILayout.EndHorizontal ();
		
		GUILayout.Space (5);
		if (GUILayout.Button ("Toggle")) {
			ToggleRenderers ();
		}
		GUILayout.Space (5);
		if (GUILayout.Button ("Toggle")) {
			ToggleAllSelected ();
		}
		GUILayout.Space (34);
		if (GUILayout.Button ("Add")) {
			AddComponent ();
		}
		GUILayout.Space (29);
		if (GUILayout.Button ("Remove")) {
			RemoveComponent ();
		}
		GUILayout.Space (44);
		
		
		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Layer")) {
			ChangeLayer ();
		}
		if (GUILayout.Button ("Tag")) {
			ChangeTag ();
		}
		EditorGUILayout.EndHorizontal ();
		
		
		GUILayout.Space (24);
		if (GUILayout.Button ("Rename")) {
			Rename (false);
		}
		
		GUILayout.Space (24);
		if (GUILayout.Button ("Rename")) {
			Rename (true);
		}
		
		GUILayout.Space (24);
		if (GUILayout.Button ("Set")) {
			ChangeRenderQueue();
		}
		
		EditorGUILayout.EndVertical ();
		EditorGUILayout.EndHorizontal ();
	}

	/*
     * Functions
     */
	void ChangeRenderQueue(){
		Transform[] transforms = Selection.GetTransforms (SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable);
		
		foreach (Transform tr in transforms) {
			if(tr.gameObject.GetComponent(typeof(SetRenderQueue)) == null){
				//add component w/ value
				SetRenderQueue src = tr.gameObject.AddComponent<SetRenderQueue>();
				src.queue = int.Parse(newRenderQueue);	
			} else {
				//get component and set value
				SetRenderQueue src = tr.GetComponent<SetRenderQueue>();
				src.queue = int.Parse(newRenderQueue);
			}
		}
	}
	
	void MoveToYPos ()
	{
		float yPos = float.Parse (theYPos);
		Transform[] transforms = Selection.GetTransforms (SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable);
		
		foreach (Transform tr in transforms) {
			tr.position = new Vector3 (tr.position.x, yPos, tr.position.z);
		}
	}

	void ZeroOutTransform ()
	{
		Transform[] transforms = Selection.GetTransforms (SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable);
		
		foreach (Transform tr in transforms) {
			tr.position = Vector3.zero;
			tr.localScale = Vector3.one;
			tr.rotation = Quaternion.identity;
		}
	}

	void InheritParentTransform ()
	{
		Transform[] transforms = Selection.GetTransforms (SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable);
		
		foreach (Transform tr in transforms) {
			tr.transform.position = tr.parent.transform.position;
			tr.transform.rotation = tr.parent.transform.rotation;
			tr.transform.localScale = tr.parent.transform.localScale;
		}
	}

	void ToggleRenderers ()
	{
		Transform[] transforms = Selection.GetTransforms (SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable);
		
		foreach (Transform transform in transforms) {
			if (transform.renderer)
				transform.renderer.enabled = !transform.renderer.enabled;
		}
	}

	void AddComponent ()
	{
		int total = 0;
		foreach (Transform currentTransform in Selection.transforms) {
			//add component
			Component existingComponent = currentTransform.GetComponent (addComponentString);
			if (!existingComponent) {
				currentTransform.gameObject.AddComponent (addComponentString);
				total++;
			}
		}
		if (total == 0)
			Debug.Log ("No components added.");
		else
			Debug.Log (total + " components of type \"" + addComponentString + "\" created.");
	}

	void RemoveComponent ()
	{
		int total = 0;
		foreach (Transform currentTransform in Selection.transforms) {
			//remove component
			Component existingComponent = currentTransform.GetComponent (removeComponentString);
			if (existingComponent) {
				DestroyImmediate (existingComponent);
				total++;
			}
		}
		if (total == 0)
			Debug.Log ("No components destroyed.");
		else
			Debug.Log (total + " components of type \"" + removeComponentString + "\" removed.");
	}

	void ToggleAllSelected ()
	{
		foreach (Transform t in Selection.transforms) {
			t.gameObject.SetActiveRecursively (!t.gameObject.active);
		}
	}

	void ChangeLayer ()
	{
		Object[] selectedObjects;
		
		selectedObjects = Selection.GetFiltered (typeof(GameObject), SelectionMode.TopLevel);
		
		foreach (GameObject go in selectedObjects) {
			LayerMask aLayer = LayerMask.NameToLayer (theLayer);
			if (aLayer.value != -1)
				go.layer = aLayer;
		}
	}

	void ChangeTag ()
	{
		Object[] selectedObjects;
		
		selectedObjects = Selection.GetFiltered (typeof(GameObject), SelectionMode.TopLevel);
		
		foreach (GameObject go in selectedObjects) {
			go.tag = theLayer;
		}
	}
	
	void Rename (bool numberedSuffix)
	{
		Transform[] transforms = Selection.GetTransforms (SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable);
		
		ArrayList transformArray = new ArrayList ();
		
		foreach (Transform tr in transforms) {
			transformArray.Add (tr);
		}
		
		for (int i = 0; i < transformArray.Count; i++) {
			if(numberedSuffix){
				/*if(theRenameNameSuf.Substring(0,1) == "#"){
					theRenameNameSuf = theRenameNameSuf.Replace("#", "");
					((Transform)transformArray[i]).name = i.ToString () + theRenameNameSuf;
				}
				else if(theRenameNameSuf.Substring(theRenameNameSuf.Length-1,1) == "#"){
					theRenameNameSuf = theRenameNameSuf.Replace("#", "");*/
					((Transform)transformArray[i]).name = theRenameNameSuf + i.ToString ();
				//} else  {
				//	((Transform)transformArray[i]).name = theRenameNameSuf;
				//}
			}
			else
				((Transform)transformArray[i]).name = theRenameName;
		}
		
		//This hack below will ensure proper name sorting :/
		Transform origParent = ((Transform)transformArray[0]).parent;
		
		for (int i = 0; i < transformArray.Count; i++) {
			if(((Transform)transformArray[i]).parent != null){
				((Transform)transformArray[i]).parent = (((Transform)transformArray[i]).parent).parent;
				((Transform)transformArray[i]).parent = origParent;
			} else { 
				Debug.LogWarning("Objects must be parented one level deep in the heirarchy!");
			}
		}
	}
}
