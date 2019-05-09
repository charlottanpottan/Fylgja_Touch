using UnityEngine;
using System.Collections;
using UnityEngine.Video;

public class BGPlayer : MonoBehaviour
{
   // public MovieTexture movTexture;
    public VideoPlayer videoPlayer;

    void Awake()
    {
        //GetComponent<Renderer>().material.mainTexture = movTexture;
        // movTexture.loop = true;
        //  movTexture.Play();
        
        videoPlayer.Play();
    }
}