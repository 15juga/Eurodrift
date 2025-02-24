using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Anim : MonoBehaviour {

    public Vector2 offset;

    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        Vector2 pos = new Vector2(Time.time * offset.x, Time.time * offset.y);
        GetComponent<Renderer>().material.mainTextureOffset = pos;
        GetComponent<Renderer>().material.SetTextureOffset("_Emission", pos);
    }
}
