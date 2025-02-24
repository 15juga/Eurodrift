using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    public int lives;

    public Image[] heartsI;
    public Sprite heartsS;

    //GameObject thePlayer;
    //VehicleControler vchCScript;

    Material emmission;

    public float outOfBoundsY = -20;

    private bool blastProof = false;

    // Start is called before the first frame update
    void Start()
    {
        //GameObject thePlayer = GameObject.Find("Player");
        //VehicleControler vchCScript = thePlayer.GetComponent<VehicleControler>();

        //if(vchCScript.number == )

        emmission = transform.GetChild(0).GetComponent<Material>(); 


    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < heartsI.Length; i++)
        {
            if(i < lives)
            {
                heartsI[i].enabled = true;
            }
            else
            {
                heartsI[i].enabled = false;
            }
        }

        if(transform.position.y <= outOfBoundsY)
        {
            looseLife();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!blastProof)
        {
            if (other.CompareTag("Bomb") || other.CompareTag("OoB Box"))
            {
                looseLife();
                blastProof = true;
                StartCoroutine(BlastProof());
            }
        }
    }

    void looseLife()
    {
        lives--;
        emmission.SetColor("_EmissionColor", Color.red);
    }

    IEnumerator BlastProof()
    {
        yield return new WaitForSeconds(3);
        blastProof = false;
    }
}
