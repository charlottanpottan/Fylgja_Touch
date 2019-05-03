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
/// $b$Holder (parent) for all DecalExpeditors on certain GameObject$bb$
/// </summary>
public class DecalHolder : MonoBehaviour
{
    private Dictionary<DecalType, GameObject> _decalType2DecalObject = new Dictionary<DecalType, GameObject>();
    public Dictionary<DecalType, GameObject> DecalType2DecalObject
    {
        get { return _decalType2DecalObject; }
    }

    /// <summary>
    /// $b$Get certain DecalExpeditor on this DecalHolder (parented to this GameObject)$bb$
    /// </summary>
    /// <param name="decalType">DecalType for search DecalExpeditor</param>
    /// <returns>$b$DecalExpeditor (parent) for all Decals of type decalType on this GameObject (Holder).$bb$</returns>
    public GameObject GetExpeditor(DecalType decalType)
    {
        GameObject result;
        _decalType2DecalObject.TryGetValue(decalType,out result);
        return result;
    }
    /// <summary>
    /// $b$Get all DecalExpeditors on this DecalHolder (parented to this GameObject)$bb$
    /// </summary>
    /// <returns>$b$All DecalExpeditors (parents) for all DecalTypes on this GameObject (Holder).$bb$</returns>
    public GameObject[] GetAllExpeditors()
    {
        List<GameObject> result=new List<GameObject>();
        foreach (GameObject obj in _decalType2DecalObject.Values)
        {
            result.Add(obj);
        }
        return result.ToArray();
    }
}
