using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    public string playerName;
    public Transform player;
    public Transform destination;
    public int lapsAmount;
    public int currentPoints;


    public Text lapsText;

    private int pointsNeeded;
    private bool teleported;

    void Start()
    {
        lapsAmount = 0;

        setLapsText();
    }

    void Update()
    {
        GameObject checkpoints = GameObject.FindGameObjectWithTag("Player");
        Controll_Checkpoints checkPointScript = checkpoints.GetComponent<Controll_Checkpoints>();

        currentPoints = (int)checkPointScript.amountofPoints;
        pointsNeeded = (int)checkPointScript.checkpointsRequired;

        if (teleported) {
            // Relative Pos
            Vector3 relativePos = player.position - transform.position;
            player.position = destination.position + destination.rotation * relativePos;

            // Rotation
            player.rotation = Quaternion.Euler(destination.eulerAngles) * player.rotation;

            // Velaocity
            Vector3 vel = player.GetComponent<Rigidbody>().velocity;
            player.GetComponent<Rigidbody>().velocity = Quaternion.FromToRotation(Vector3.Normalize(vel), destination.forward) * Quaternion.Inverse(Quaternion.FromToRotation(Vector3.Normalize(vel), transform.forward)) * vel;

            // adding +1 lap
            lapsAmount++;
            setLapsText();
            
            teleported = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag(playerName) && currentPoints >= pointsNeeded)
        {
            teleported = true;
        }
    }

    void setLapsText()
    {
        lapsText.text = "laps: " + lapsAmount.ToString();
    }
}
