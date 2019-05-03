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

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс тип декали
/// </summary>
public class DecalType : MonoBehaviour
{
    //Foldout
    public bool _materialFoldout = true;
    public bool _meshComponentsFoldout = false;
    public bool _meshEditingFoldout = false;
    public bool _randomFoldout = false;
    public bool _decalCombiningAndLifeManagementFoldout = false;
    public bool _flowFoldout = false;
    public bool i_colors = true;// need colors
    public bool i_boneWeights = true;// need weight
    public bool i_uv2 = false;// need second uv
    public Material i_material;
    public float i_bitOffset = 0.002F;// offset along normal
    public float i_normalThreshold = 90F;
    public UVGenerationMode i_uvGenerationMode = UVGenerationMode.Projective;
    public UVGenerationMode i_uv2GenerationMode = UVGenerationMode.Preserve;
    public DecalBoneWeightQuality i_decalBoneWeightQuality = DecalBoneWeightQuality.Bone_4;
    public Vector2 i_uvOffset = Vector2.zero;
    public Vector2 i_uvScale = new Vector2(1, 1);    
    public Vector2 i_uv2Offset = Vector2.zero;
    public Vector2 i_uv2Scale = new Vector2(1, 1);
    public RandomMode i_randomMode = RandomMode.None;
    public int i_layer = 0;// layer for dynamic decal
    public bool i_fade = true;
    public int i_atlasTilingU = 1;
    public int i_atlasTilingV = 1;
    public Vector3 i_randomVector = new Vector3(0, 0, 0);
    public float i_randomSize = 0;
    public float i_fadingTime = 1;
    public int i_combineEvery = 10;
    public int i_destroyGenerationDelay = 5;
    public int i_maxSkinnedDecals = 4;
    public float i_expeditorLifeTime = 20;
    public List<GameObject> i_ignoreList;
    private GameObject _objectForCombining;
    public GameObject ObjectForCombining
    {
        get { return _objectForCombining; }
        set
        {
             _objectForCombining = value;
             if (_objectForCombining&&_objectForCombining.GetComponent<DecalType>())
                 _objectForCombining = null;
        }
    }
    public MeshTypes i_projectionMask = MeshTypes.Mesh;
    public DecalDragProjectionType _decalDragProjectionType = DecalDragProjectionType.AlongEditorCameraRay;
    // Flow
    public bool i_flow = false;
    public Vector3 i_gravity = new Vector3(0, -1, 0);//Gravity direction in World Space     
    public int i_mapSize = 256;//RT Height
    public float i_minLevel = 0.7F;//Min blood level
    public float i_speedIncrease = 0.13F;//How fast blood increase
    public float i_speedDecrease = 0.175F;//How fast blood decrease
    public float i_glue = 5;// Glue
    public float i_sourceBumpContrib = 0.3F;//How source bump normal affect on flow
    public int i_growFramesCount = 10;//How long blood will be grow
    public float i_growSpeed = 0.5F;//How fast blood will be grow
    public float i_lifeTime = 5;// How lont decal will be alive
    public FlowType i_flowType;// Flow type for decal
    public Shader i_gravityShader;// Shader for gravity map     

    public static int i = 0;
}
/// <summary>
/// Randomize type
/// </summary>
public enum RandomMode
{
    None,
    Evenly,
    PerComponent
}
/// <summary>
/// UV gen type
/// </summary>
public enum UVGenerationMode
{
    Projective = 0,
    Preserve,
    Normalized,
}
/// <summary>
/// UV2 gen type
/// </summary>
public enum UV2GenerationMode
{
    FromUV,
    FromUV2
}
/// <summary>
/// Proj type
/// </summary>
public enum DecalDragProjectionType
{
    AlongEditorCameraRay,
    AlongSurfaceNormal
}
/// <summary>
/// Mesh types
/// </summary>
[System.Flags]
public enum MeshTypes 
{
    Mesh = 1,
    SkinedMesh = 2
}
/// <summary>
/// Quality
/// </summary>
public enum DecalBoneWeightQuality
{
    Bone_1,
    Bone_2,
    Bone_4
}

