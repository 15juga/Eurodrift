using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control_Pickup : MonoBehaviour
{
    public bool animate;

    public AnimationCurve myCurve;
    //public float rotatorSpeed;
    //public float lowestHeight;

    public Vector3 rotationSpeed;

    private float height;
    private Vector3 startYposition;

    private SphereCollider pickupColl;

    float timer;

    public float respawnSec = 3;

    private Vector3 startPos;
    private Vector3 activePos;

    bool IsInActive;

    void Start()
    {
        //startYposition = new Vector3((transform.position.x, (transform.position.y, (transform.position.z);
        height = transform.position.y;

        timer = 0;

        IsInActive = false;
        startPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        activePos = new Vector3(transform.position.x, transform.position.y + -100, transform.position.z);

        pickupColl = GetComponent<SphereCollider>();
    }

    void Update()
    {
        if (animate)
        {
            transform.Rotate(rotationSpeed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, height + myCurve.Evaluate((Time.time % myCurve.length)), transform.position.z);
        }



        if (IsInActive)
        {
            timer += Time.deltaTime;
            if (timer > respawnSec)
            {
                //print("timer off");
                //gameObject.SetActive(true);
                transform.position = startPos;
                pickupColl.enabled = true;
                timer = 0;
                animate = true;
                IsInActive = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //gameObject.SetActive(false);
            pickupColl.enabled = false;
            transform.position = activePos;
            IsInActive = true;
            animate = false;
        }

    }
}
