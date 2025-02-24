using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;
using UnityEngine;
//using UnityEditor.Experimental.GraphView;
using System.Drawing;
using System.ComponentModel;

public class Controll_Checkpoints : MonoBehaviour
{
    public Material[] material;
    Renderer rend;

    public int checkpointsRequired;
    public int amountofPoints;

    public float outOfBoundsY = -20;
    public int addTime = 10;

    private Vector3 checkPoint;
    private float checkPointRotation;
    private BoxCollider checkPointCollider;
    private GameObject[] pointReset;

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
        if (Input.GetButtonDown("TP Checkpoint"))
        {
            tpCheckPoint();
            transform.GetComponent<HP>().looseLife();
            /*transform.position = checkPoint;
            transform.rotation = Quaternion.Euler(0, 180, 0);
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            GetComponent<Rigidbody>().velocity = Vector3.zero;*/
        }

        if (transform.position.y < outOfBoundsY)
        {
            tpCheckPoint();
            transform.GetComponent<HP>().looseLife();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Checkpoint"))
        {
            saveCheckpoint(other);

            //Adding time
            GameObject canvas = GameObject.Find("Canvas");
            canvas.GetComponent<UI>().countDownS += addTime;

            // testing
            checkPointCollider = other.GetComponent<BoxCollider>();
            rend = other.GetComponent<Renderer>();
            rend.sharedMaterial = material[1];
            //print("ping");
            checkPointCollider.enabled = false;

            amountofPoints++;
        }
        if (other.gameObject.CompareTag("OoB Box"))
        {
            tpCheckPoint();
            //print("Oob");
        }

        if (other.gameObject.CompareTag("EndPortal"))
        {
            pointReset = GameObject.FindGameObjectsWithTag("Checkpoint");

            foreach (GameObject i in pointReset)
            {
                rend = i.GetComponent<Renderer>();
                rend.sharedMaterial = material[0];
                amountofPoints = 0;
                checkPointCollider = i.GetComponent<BoxCollider>();
                checkPointCollider.enabled = true;
            }
        }
    }

    void tpCheckPoint()
    {
        transform.position = checkPoint;
        transform.rotation = Quaternion.Euler(0, checkPointRotation - 180, 0);
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public void saveCheckpoint(Collider other)
    {
        checkPoint = other.transform.position;
        checkPointRotation = other.transform.eulerAngles.y;
    }
}
