using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Controll_Checkpoints : MonoBehaviour
{
    private Vector3 checkPoint;
    private float checkPointRotation;

    public Material[] material;
    Renderer rend;

    private BoxCollider checkPointCollider;
    public int checkpointsRequired;
    public int amountofPoints;

    public float outOfBoundsY = -20;

    void Start()
    {
        //rend = GetComponent<Renderer>();
        //rend.enabled = true;
        //rend.sharedMaterial = material[0];

        checkPointRotation = transform.rotation.y;
        checkPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Save Checkpoint"))
        {
            checkPoint = transform.position;
            //checkPointRotation = transform.rotation.y;
        }
        if (Input.GetButtonDown("TP Checkpoint"))
        {
            tpCheckPoint();
            /*transform.position = checkPoint;
            transform.rotation = Quaternion.Euler(0, 180, 0);
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            GetComponent<Rigidbody>().velocity = Vector3.zero;*/
        }

        if (transform.position.y < outOfBoundsY)
        {
            tpCheckPoint();
        }

        if(amountofPoints >= checkpointsRequired)
        {
            //print("You win");
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Checkpoint"))
        {
            saveCheckpoint(other);

            //GetComponent<UI>().countDownS += 10;

            // testing
            checkPointCollider = other.GetComponent<BoxCollider>();
            rend = other.GetComponent<Renderer>();
            rend.sharedMaterial = material[1];
            print("ping");
            checkPointCollider.enabled = false;

            amountofPoints++;
        }
        if (other.gameObject.CompareTag("OoB Box"))
        {
            tpCheckPoint();
            print("Oob");
        }
    }

    void tpCheckPoint()
    {
        transform.position = checkPoint;
        transform.rotation = Quaternion.Euler(0, 180, 0);
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    void saveCheckpoint(Collider other)
    {
        checkPoint = other.transform.position;
        checkPointRotation = other.transform.rotation.y;
    }
}
