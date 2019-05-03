using UnityEngine;
using System.Collections;

public class BGPlayer : MonoBehaviour {
    public MovieTexture movTexture;
    void Awake() {
        GetComponent<Renderer>().material.mainTexture = movTexture;
		movTexture.loop = true;
        movTexture.Play();
    }
}