﻿/*
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalDestroyer : MonoBehaviour
{
    private float _timeToDestroy;//time to sestroy
    public float TimeToDestroy
    {
        get { return _timeToDestroy; }
        set { _timeToDestroy = value; }
    }
    private float _fadingTime;
    public float FadingTime
    {
        get { return _fadingTime; }
        set { _fadingTime = value; }
    }
    private float _startFadingTime;
    private bool _fade = true;
    public bool Fade
    {
        get { return _fade; }
        set { _fade = value; }
    }


    private void Start()
    {
        StartCoroutine(TimingDestroy());
    }
    /// Delay
    private IEnumerator TimingDestroy()
    {
        yield return new WaitForSeconds(_timeToDestroy);
        if (_fade)
        {
            _startFadingTime = Time.time;
            StartCoroutine(SmoothFade());
        }
        else
        {
            ClearAndDestroy();
        }
    }
    /// Smooth destroy , alpha
    private IEnumerator SmoothFade()
    {
        float i = 1;
        while (i > 0)
        {
            float delta = Time.time - _startFadingTime;
            i = (_fadingTime - delta) / _fadingTime;

            Color color = GetComponent<Renderer>().material.color;
            color.a = i;
            GetComponent<Renderer>().material.color = color;

            yield return null;
        }
        ClearAndDestroy();
    }
    /// Clearing
    private void ClearAndDestroy()
    {
        Destroy(GetComponent<MeshFilter>().sharedMesh);
        Destroy(GetComponent<Renderer>().material);
        Destroy(this.gameObject);
    }
}
