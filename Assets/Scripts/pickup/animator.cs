using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animator : MonoBehaviour
{
    public AnimationCurve myCurve;
    //public float rotatorSpeed;
    //public float lowestHeight;

    public Vector3 rotationSpeed;

    private float height;
    private Vector3 startYposition;

    void Start()
    {
        //startYposition = new Vector3((transform.position.x, (transform.position.y, (transform.position.z);
        height = transform.position.y;
    }

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, height + myCurve.Evaluate((Time.time % myCurve.length)), transform.position.z);

    }
}
