using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedTexture : MonoBehaviour {
 public float scrollSpeed = 0.5F;
    public Renderer rend;
    void Start() {
        rend = GetComponent<Renderer>();
    }
    void Update() {
        float offset = Time.time * scrollSpeed;
        rend.material.SetTextureOffset("_DetailAlbedoMap", new Vector2(offset, 0));
        rend.material.SetTextureOffset("_BumpMap", new Vector2(offset, 0));
    }
}



