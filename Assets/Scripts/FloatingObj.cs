using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObj : MonoBehaviour {

    public float fHeight;

    private Vector3 curPos;
    private float rngSpeed;
    void Start() {
        curPos = transform.position;
        rngSpeed = Random.Range(0.3f, 0.7f);
    }

    
    void Update() {
        transform.position = new Vector3(curPos.x, curPos.y + Mathf.Sin(Time.time * rngSpeed) * fHeight * (transform.localScale.x / 3), curPos.z);
    }
}
