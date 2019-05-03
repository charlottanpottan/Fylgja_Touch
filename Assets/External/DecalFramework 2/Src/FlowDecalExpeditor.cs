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

using System;
using System.Collections.Generic;
using UnityEngine;

public class FlowDecalExpeditor : MonoBehaviour
{
    private Shader _gravityShader;//Shader for gravity
    private DecalType _decalType;// DecalType
    public DecalType DecalType
    {
        get { return _decalType; }
        set { _decalType = value; }
    }
    private Vector4 _mainTexUVParams;// Offset and Scale factor for random uv of mainTex
    public Vector4 MainTexUVParams
    {
        get { return _mainTexUVParams; }
        set { _mainTexUVParams = value; }
    }
    private bool _renderOrder = false;//Ping Pong order
    private bool _takeHeightFromTexture = true;//When get height from map
    private RenderTexture i_gravityMap;
    private RenderTexture i_gravityMap2;
    private int frameOnStart;//Номер фрейма на старте
    private Camera gravityCamera;

    private void Start()
    {
        // Disable renderer first start(artefacts)
        renderer.enabled = false;

        _gravityShader = Shader.Find("Frameshift/Decal/1.5/Gravity");
        if(!_gravityShader)
        {
            throw new NullReferenceException("Can't find Frameshift/Decal/1.5/Gravity shader");
        }

        SetupRenderTargets();
        SetupMaterials();
        frameOnStart = Time.frameCount;

        gravityCamera = CreateCamera();
    }
    private void Update()
    {
        //Change decal layer
        int nativeLayer = gameObject.layer;
        gameObject.layer = 31;

        //Set gravity params  
        Vector4 gravityInObjectSpace = transform.InverseTransformDirection(_decalType.i_gravity);
        if (_takeHeightFromTexture)
            gravityInObjectSpace.w = 1;
        else
            gravityInObjectSpace.w = 0;

        Shader.SetGlobalVector("_fGravityInObject", gravityInObjectSpace);
        Shader.SetGlobalVector("_fParams", new Vector4(_decalType.i_minLevel, _decalType.i_speedIncrease, _decalType.i_speedDecrease, _decalType.i_glue));
        Shader.SetGlobalVector("_fParams2", new Vector4(_decalType.i_sourceBumpContrib, _decalType.i_growSpeed, (float)_decalType.i_flowType, 1));
        // Random offset params
        Shader.SetGlobalVector("_fUVParams", _mainTexUVParams);
        //Render to gravity
        if (_renderOrder)
        {
            gravityCamera.targetTexture = i_gravityMap;
            i_gravityMap2.SetGlobalShaderProperty("_GravityMap");
        }
        else
        {
            gravityCamera.targetTexture = i_gravityMap2;
            i_gravityMap.SetGlobalShaderProperty("_GravityMap");
        }
        gravityCamera.RenderWithShader(_gravityShader, "Fluid");

        //Swap buffers
        _renderOrder = !_renderOrder;

        //Restore layer
        gameObject.layer = nativeLayer;

        if (Time.frameCount >= frameOnStart + _decalType.i_growFramesCount)
            _takeHeightFromTexture = false;

        renderer.enabled = true;
    }
    private void OnDisable()
    {
        i_gravityMap.Release();
        i_gravityMap2.Release(); 
    }
    /// Create RT camera
    private Camera CreateCamera()
    {
        //Gravity camera
        Camera gravityCamera = null;
        GameObject gGravityCam = new GameObject("GRAVITY CAMERA", typeof(Camera));
        gGravityCam.transform.parent = this.transform;
        gGravityCam.transform.localPosition = Vector3.zero;
        gGravityCam.transform.localRotation = Quaternion.identity;
        gravityCamera = gGravityCam.camera;
        gravityCamera.enabled = false;
        gravityCamera.hideFlags = HideFlags.HideAndDontSave;
        gravityCamera.orthographic = true;
        gravityCamera.farClipPlane = 100;
        gravityCamera.nearClipPlane = -100F;
        gravityCamera.orthographicSize = transform.localScale.y / 2;
        gravityCamera.cullingMask = (1 << 31);
        gravityCamera.backgroundColor = Color.black;
        return gravityCamera;
    }
    /// Delete RT camera
    private void DestroyCameras(Camera gravityCamera)
    {
        Destroy(gravityCamera.gameObject);
    }
    /// Setup RT's
    private void SetupRenderTargets()
    {
        float aspect = transform.localScale.x / transform.localScale.y;
        int mapWidth = (int)(_decalType.i_mapSize * aspect);
        i_gravityMap = new RenderTexture(mapWidth, _decalType.i_mapSize, 0);
        i_gravityMap2 = new RenderTexture(mapWidth, _decalType.i_mapSize, 0);

        i_gravityMap.isPowerOfTwo = false;
        i_gravityMap2.isPowerOfTwo = false;

        i_gravityMap.hideFlags = HideFlags.HideAndDontSave;
        i_gravityMap2.hideFlags = HideFlags.HideAndDontSave;

        //i_gravityMap.filterMode = FilterMode.Point;
        //i_gravityMap2.filterMode = FilterMode.Point;   
    }
    /// Setup Material
    private void SetupMaterials()
    {
        renderer.material.SetTexture("_HeightMap", i_gravityMap2);
        // Random offset params
        renderer.material.SetVector("_fMainTexUVParams", _mainTexUVParams);
    }
}
/// Type for flow
public enum FlowType
{
Drop,
Spread
}