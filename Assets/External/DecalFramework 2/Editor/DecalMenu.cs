/*
******************************************************************
Copyright (c) 2010, Alexey Dobrolyubov
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions
are met:

    * Redistributions of source code must retain the above copyright
      notice, this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above
      copyright notice, this list of conditions and the following
      disclaimer in the documentation and/or other materials provided
      with the distribution.
    * Neither the name of the names of its contributors may be used
      to endorse or promote products
      derived from this software without specific prior written
      permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
"AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS
FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE
COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING,
BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN
ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
POSSIBILITY OF SUCH DAMAGE.

******************************************************************
*/

using System.IO;
using UnityEngine;
using UnityEditor;

public class DecalMenu : EditorWindow
{
    public static string _error = "";

    protected static DecalMenu _menu;


    /// Creates the decal in asset.
    [MenuItem("Assets/Create/Decal")]
    public static void CreateDecalAsset()
    {
        string pathFolder = AssetDatabase.GetAssetPath(Selection.activeObject);

        if (string.IsNullOrEmpty(pathFolder))
        {
            pathFolder = "Assets/";
        }
        else
        {
            string extention = Path.GetExtension(pathFolder);

            if (string.IsNullOrEmpty(extention))
                pathFolder = pathFolder + "/";
            else
                pathFolder = Path.GetDirectoryName(pathFolder) + "/";
        }

        // Create decal
        string path = AssetDatabase.GenerateUniqueAssetPath(pathFolder + "New Decal Type" + ".prefab");
        UnityEngine.Object decalPrefabObject = EditorUtility.CreateEmptyPrefab(path);
        GameObject gObject = new GameObject();
        GameObject decal = EditorUtility.ReplacePrefab(gObject, decalPrefabObject);
        decal.AddComponent<DecalType>();
        DestroyImmediate(gObject);
        AssetDatabase.Refresh();
        Selection.activeObject = decal;
    }
    /// Create Decal in scene
    [MenuItem("GameObject/Create Other/Decal")]
    public static void CreateDecalScene()
    {
        GameObject gObject = new GameObject("Decal",typeof(MeshFilter),typeof(MeshRenderer));
        gObject.AddComponent<DecalType>();
        Selection.activeObject = gObject;
    }
    /// Show About
    [MenuItem("Decal Framework/About")]
    public static void FrameshiftAbout()
    {
        Init();
    }
    /// Init Window
    private static void Init()
    {
        DecalMenu window = EditorWindow.GetWindow(typeof(DecalMenu)) as DecalMenu;
        window.Show();
    }

    private void OnGUI()
    {
        GUIStyle label = new GUIStyle(EditorStyles.label);
        label.alignment = TextAnchor.UpperLeft;

        GUI.Label(new Rect(10, 10, 300, 80), "Frameshift Decal Framework v 1.5.\n\nConsider any suggestion for collaboration.\nAll proposals and suggestions please send by mail\nor Skype.", label);
        GUI.Label(new Rect(10, 85, 300, 20), "     http://unity3dstore.com", label);
        GUI.Label(new Rect(10, 100, 300, 20), "     support@unity3dstore.com", label);
        GUI.Label(new Rect(10, 115, 300, 20), "     Skype: mentalauto", label);
    }
}
