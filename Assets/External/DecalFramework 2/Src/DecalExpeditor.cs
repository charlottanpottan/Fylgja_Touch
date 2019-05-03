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

public abstract class DecalExpeditor : MonoBehaviour
{
    protected DecalType _decalType;
    public DecalType DecalType
    {
        get { return _decalType; }
        set { _decalType = value; }
    }
    protected List<Mesh> _allDecalMeshes = new List<Mesh>();// All current live decals
    protected List<GameObject> _freeDecals = new List<GameObject>();// Free decals
    protected int _N = 0;
    protected float _timeSinceLastAdd;
    private DecalHolder _decalHolder;
    public DecalHolder Holder
    {
        get { return _decalHolder; }
        set { _decalHolder = value; }
    }


    public abstract void PushNewDecalMesh(Mesh newDecalMesh);
    public abstract void DeleteCombinnedMesh();

    private void Update()
    {          
        if (Time.time - _timeSinceLastAdd > _decalType.i_expeditorLifeTime)
        {

            foreach (Mesh mesh in _allDecalMeshes)
            {
                Destroy(mesh);
            }


            DeleteCombinnedMesh();

            _decalHolder.DecalType2DecalObject.Remove(_decalType);

            Destroy(this.gameObject);
        }
    }
}
