using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class MiniMap : MonoBehaviour {

    public float height = 20;
    public float width = 9;
    public bool loop = false;
    public Transform player;

    private LineRenderer line;
    private Transform tracker;
    private Transform arrow;
    private Transform Cam;

    void Start() {
        line = GetComponent<LineRenderer>();
        tracker = this.transform;
        arrow = tracker.GetChild(0).GetChild(1);
        Cam = tracker.GetChild(0).GetChild(0);

        int trackCount = tracker.transform.childCount;
        line.positionCount = (loop) ? trackCount + 1 : trackCount;

        for(int i = 0; i < trackCount; i++)
            line.SetPosition(i, new Vector3(tracker.GetChild(i).position.x, height, tracker.GetChild(i).position.z));

        if(loop)
            line.SetPosition(trackCount, line.GetPosition(0));

        line.startWidth = width;
        line.endWidth = width;
    }

    // Update is called once per frame
    void Update() {

        arrow.SetPositionAndRotation(new Vector3(player.position.x, height + 300,player.position.z), Quaternion.Euler(90, player.rotation.eulerAngles.y, 90));
        Cam.position = (new Vector3(player.position.x, 500, player.position.z));
    }
}
