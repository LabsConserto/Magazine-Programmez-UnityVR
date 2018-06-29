using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedTextureMain : MonoBehaviour {
 public float scrollSpeed = 0.5F;
    public Renderer rend;
    void Start() {
        rend = GetComponent<Renderer>();
    }
    void Update() {
        float offset = Time.time * scrollSpeed;
        float offset2 = Time.time * scrollSpeed + 1;
        rend.material.SetTextureOffset("_MainTex", new Vector2(0, -offset));
        rend.material.SetTextureOffset("_DetailAlbedoMap", new Vector2(offset2, 0));
    }
}



