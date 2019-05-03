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

public class DynamicDecalExpeditor : DecalExpeditor
{   
    private int _currentBasis = 0;

    /// Send new created decal
    public override void PushNewDecalMesh(Mesh newDecalMesh)
    {

        _timeSinceLastAdd = Time.time;

        newDecalMesh.name = _decalType.name + " Decal Mesh";

        _allDecalMeshes.Add(newDecalMesh);

        // Craete free decal
        _freeDecals.Add(MakeDecalObject(newDecalMesh));

        ++_N;

        // Combine
        if (_N == _decalType.i_combineEvery)
        {
            CombineFreeDecals();
            _N = 0;
        }
    }
    /// Delete
    public override void DeleteCombinnedMesh()
    {
        Destroy(this.GetComponent<MeshFilter>().sharedMesh);
    }

    /// Create free gameobject
    private GameObject MakeDecalObject(Mesh decalMesh)
    {
        GameObject newFreeDecal = new GameObject("Free Decal");
        newFreeDecal.layer = DecalType.i_layer;
        newFreeDecal.transform.parent = transform;
        newFreeDecal.transform.localPosition = Vector3.zero;
        newFreeDecal.transform.localRotation = Quaternion.identity;
        newFreeDecal.transform.localScale = new Vector3(1, 1, 1);
        MeshFilter mFilter = newFreeDecal.AddComponent<MeshFilter>();
        MeshRenderer mRenderer = newFreeDecal.AddComponent<MeshRenderer>();
        mFilter.sharedMesh = decalMesh;
        mRenderer.sharedMaterial = renderer.sharedMaterial;
        mRenderer.castShadows = false;

        return newFreeDecal;
    }
    /// combine
    private void CombineFreeDecals()
    {

        ++_currentBasis;


        if (_currentBasis > _decalType.i_destroyGenerationDelay)
        {
            SendToDestroy();
        }

        foreach (GameObject freeDecal in _freeDecals)
        {
           // MeshFilter mFilter = freeDecal.GetComponent<MeshFilter>();

            Destroy(freeDecal);
        }

        _freeDecals.Clear();

        MeshFilter combinedMeshFilter = this.GetComponent<MeshFilter>();

        Destroy(combinedMeshFilter.sharedMesh);

        combinedMeshFilter.sharedMesh = DecalCreator.CreateCombinedMesh(_allDecalMeshes, null);

        combinedMeshFilter.sharedMesh.RecalculateBounds();
    }
    private void SendToDestroy()
    {
        // first N send to destroy
        for (int i = _decalType.i_combineEvery - 1; i >= 0; --i)
        {
            GameObject soonWillBeDestroyedDecal = MakeDecalObject(_allDecalMeshes[i]);
            soonWillBeDestroyedDecal.name = "SoonWillBeDestroyed";
            soonWillBeDestroyedDecal.layer = DecalType.i_layer;
            DecalDestroyer destroyer = soonWillBeDestroyedDecal.AddComponent("DecalDestroyer") as DecalDestroyer;
            destroyer.TimeToDestroy = 1 + i;
            destroyer.Fade = _decalType.i_fade;
            destroyer.FadingTime = _decalType.i_fadingTime;
            _allDecalMeshes.RemoveAt(i);
        }
    }    
}
