using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class VehicleControler : MonoBehaviour {
    
    public float accel = 1000;
    public float boostSpeed = 1000;
    public float maxSpeed = 300;
    public float turnSpeed = 200;
    public float maxTurn = 300;
    public float hovorHeight = 2.5f;
    public float airControlStrength = 500;
    public float jumpStrength =  1000;
    public Vector2 placement = new Vector2(4.1f, 7.2f);
    public LayerMask mask;
    public float boostDuration;
    public float boostMtr = 0;
    public GameObject Fafnir;
    public GameObject Ae86;


    private bool GroundBoosting = false;
    private bool boosting = false;
    private float boostTimer;
    private float boostTimerG;

    private bool antiBoosting;
    private float antiBoostTimer;
    private float currentMaxSpeed;
    private float maxSpeedLerp;
    private int number = 0;

    private Rigidbody entity;
    private Vector3 axis;
    private Vector3 relativeVel;
    private float angularDrag = 1;
    private float drift;
    private Vector3[] thrustPos = new Vector3[4];
    private RaycastHit ray;
    private bool space = false;
    private bool shift = false;
    private bool mouse1 = false;
    private bool mouse2 = false;
    private float rotationSpeed = 0;
    private GameObject Faf;
    private GameObject Ae;

    public Material checkPMaterial;
    public Material[] mat;
    public Material[] objMat;
    private Color originColor;
    private Color currentCol;
    private float time;

    void Start() {
        
        maxSpeedLerp = maxSpeed;
        currentMaxSpeed = maxSpeed;
        number = PlayerPrefs.GetInt("carModel");

        if (number == 0) {
            Instantiate(Fafnir);
            Faf = GameObject.Find(Fafnir.name + "(Clone)");
            Faf.transform.position = transform.position + new Vector3(0, -0.5f, 0);
            Faf.transform.SetParent(transform);

            mat = GameObject.Find("Car").GetComponent<Renderer>().materials;
            originColor = mat[1].GetColor("_EmissionColor");
        }
        else if (number == 1)
        {
            Instantiate(Ae86);
            Ae = GameObject.Find(Ae86.name + "(Clone)");
            Ae.transform.position = transform.position + new Vector3(0, 0.5f, 0);
            Ae.transform.SetParent(transform);

            mat = Ae.GetComponent<Renderer>().materials;
            originColor = mat[0].GetColor("_EmissionColor");
        }

        entity = this.GetComponent<Rigidbody>();

        checkPMaterial.SetColor("_EmissionColor", originColor * 2);

        for(int i = 0; i < mat.Length; i++)
            mat[i].EnableKeyword("_EMISSION");

        for (int i = 0; i < objMat.Length; i++) {
            objMat[i].EnableKeyword("_EMISSION");
            objMat[i].SetColor("_EmissionColor", originColor);
        }
    }

    void Update() {
        // WASD Axis
        axis = new Vector2(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));

        // Other buttons
        space = Input.GetButton("space");
        shift = Input.GetButton("shift");
        if (Input.GetButtonDown("Fire1"))
            mouse1 = true;
        if (Input.GetButtonDown("Fire2"))
            mouse2 = true;

        // Thrust positions
        thrustPos[0] = transform.position + transform.forward * placement.y / 2 + transform.right * placement.x / 2;
        thrustPos[1] = transform.position + transform.forward * placement.y / 2 - transform.right * placement.x / 2;
        thrustPos[2] = transform.position - transform.forward * placement.y / 2 + transform.right * placement.x / 2;
        thrustPos[3] = transform.position - transform.forward * placement.y / 2 - transform.right * placement.x / 2;

        // Relative Velocity
        relativeVel = transform.InverseTransformDirection(entity.velocity);

        // Drift
        drift = (space) ? Mathf.Lerp(drift, 5, 0.01f) : Mathf.Lerp(drift, 50, 0.003f);

        // Stearing calculations
        rotationSpeed = (axis.y * turnSpeed * Mathf.Clamp(Mathf.Abs((relativeVel.z + relativeVel.y) * 0.005f), 0, 1) - entity.angularVelocity.y * 30);

        // Lerp maxSpeed
        currentMaxSpeed = Mathf.Lerp(currentMaxSpeed ,maxSpeedLerp, 0.01f);

        // Clamp boostMtr
        boostMtr = Mathf.Clamp(boostMtr, 0, 4);

        // Change Map color
        if(number == 0)
            for (int i = 0; i < objMat.Length; i++) {
                objMat[i].SetColor("_EmissionColor", currentCol * 0.7f);
                if(i == 1)
                    objMat[i].SetColor("_EmissionColor", currentCol * 1.5f);
            }

        if(number == 1)
            for (int i = 0; i < objMat.Length; i++) {
                objMat[i].SetColor("_EmissionColor", currentCol);
                if (i == 1)
                    objMat[i].SetColor("_EmissionColor", currentCol * 1.5f);
            }

        GameObject canvas = GameObject.Find("Canvas");
        time = canvas.GetComponent<UI>().countDownS;
    }

    void FixedUpdate() {
        // Anti wobble
        entity.angularVelocity = new Vector3(entity.angularVelocity.x * angularDrag, entity.angularVelocity.y, entity.angularVelocity.z * angularDrag);

        for (int i = 0; i < 4; i++) {
        // Set MaxSpeed
        entity.velocity = new Vector3(Mathf.Clamp(entity.velocity.x, -currentMaxSpeed, currentMaxSpeed), entity.velocity.y, Mathf.Clamp(entity.velocity.z, -currentMaxSpeed, currentMaxSpeed));

            if (Physics.Raycast(thrustPos[i], -transform.up, out ray, hovorHeight + 2, mask)) {
                Debug.DrawRay(thrustPos[i], -transform.up * ray.distance, Color.red);

                if (ray.distance < hovorHeight) {
                    // Hovor
                    entity.AddForceAtPosition(transform.up * (13 * Mathf.Pow(entity.mass, 2) * Mathf.Pow(hovorHeight - ray.distance, 2) / hovorHeight - relativeVel.y * entity.mass * 150) * Time.deltaTime, thrustPos[i], ForceMode.Force);

                    // Set higher drag
                    angularDrag = Mathf.Clamp(ray.distance / (hovorHeight + 2) + 0.1f, 0, 0.7f);
                    
                    // Check for Ground Booster
                    if (ray.transform.CompareTag("Boost"))
                        GroundBoosting = true;

                    // Check for Anti Ground Booster
                    if (ray.transform.CompareTag("AntiBoost"))
                        antiBoosting = true;
                }

                // Add booster when drifting
                boostMtr += Mathf.Abs(relativeVel.x) * 0.000002f;

                // Jump
                if (mouse2)
                    entity.AddForce(transform.up * jumpStrength * Mathf.Clamp(Mathf.Abs(Vector3.Magnitude(entity.velocity)) * 0.4f, 70, 120) * Time.deltaTime, ForceMode.Acceleration);

                // stearing
                float angCon = (Mathf.Abs(Mathf.Rad2Deg * transform.rotation.z) < 22.5f) ? Mathf.Abs(Mathf.Rad2Deg * transform.rotation.z) : 45 - Mathf.Abs(Mathf.Rad2Deg * transform.rotation.z);
                entity.AddForceAtPosition(transform.right * Mathf.Clamp(rotationSpeed, -maxTurn, maxTurn) * entity.mass * Time.deltaTime, transform.position + transform.forward * (placement.y + 5f + 0.6f * angCon));

                // Drift and Accelerate
                entity.AddRelativeForce(new Vector3(-relativeVel.x * drift + relativeVel.x * 3, -Mathf.Abs(Vector3.Magnitude(entity.velocity)) * 10, (axis.x * (accel + Mathf.Abs(relativeVel.x) * 7) - relativeVel.z * accel * 0.001f)) * Time.deltaTime, ForceMode.Acceleration);

            } else {
                // Set lower drag
                angularDrag = 1f;

                // Air controll
                entity.AddTorque(transform.right * axis.x * entity.mass * 1.5f * airControlStrength * Time.deltaTime);
                
                if (shift) {
                    if(Vector3.Magnitude(entity.velocity) > 10)
                        entity.AddTorque(transform.forward * -axis.y * entity.mass * airControlStrength * Time.deltaTime);
                    else
                        entity.AddTorque(transform.forward * -axis.y * entity.mass * 1200 * Time.deltaTime);
                } else
                    entity.AddTorque(transform.up * axis.y * entity.mass * 1.5f * airControlStrength * Time.deltaTime);


                // Add boost when doing tricks
                boostMtr += Mathf.Abs(Vector3.Magnitude(entity.angularVelocity)) * 0.00007f;
            }
        }

        //  Boost = true when M1 is pressed and if 1 mtr is full 
        if (mouse1 && boostMtr >= 1) {
            boosting = true;
            boostMtr -= 1;
        }

        // Ground Boost
        if (GroundBoosting)
            Groundboost();


        // Boost
        if (boosting)
            boost();

        // antiboost
        if (antiBoosting)
            antiBoost();
        
        mouse1 = false;
        mouse2 = false;

        if(!boosting && !GroundBoosting)
            changeBack();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Pickup"))
        {
            //GroundBoosting = true;
            int boostOrAnti = Random.Range(0, 2);
            if (boostOrAnti == 0)
            {
                GroundBoosting = true;
                //antiBoosting = true;
            }
            if (boostOrAnti == 1)
            {
                antiBoosting = true;
            }
        }
        if(other.CompareTag("Bomb"))
        {
            //print("boomer");
            //currentMaxSpeed = 0;
            //entity.AddExplosionForce(100, transform.position, 10);
            explosion();
        }
    }

    private void Groundboost() {
        boostTimerG += Time.deltaTime;
        entity.AddForce(new Vector3(entity.velocity.x, 0, entity.velocity.z).normalized * boostSpeed * entity.mass * Time.deltaTime, ForceMode.Acceleration);
        maxSpeedLerp = maxSpeed * 3;

        

        if (boostTimerG >= boostDuration) {
            boostTimerG = 0;
            maxSpeedLerp = maxSpeed;
            GroundBoosting = false;
            
        }
    }

    private void boost() {
        boostTimer += Time.deltaTime;
        entity.AddForce(transform.forward * boostSpeed * entity.mass * Time.deltaTime, ForceMode.Acceleration);
        maxSpeedLerp = maxSpeed * 3;

        changeRed();

        if (boostTimer >= boostDuration) {
            boostTimer = 0;
            maxSpeedLerp = maxSpeed;
            boosting = false;
        }
    }

    private void antiBoost() {
        antiBoostTimer += Time.deltaTime;
        maxSpeedLerp = maxSpeed / 6;

        if (antiBoostTimer > 3) {
            antiBoostTimer = 0;
            maxSpeedLerp = maxSpeed;
            antiBoosting = false;
        }
    }

    private void explosion()
    {
        //if(!blastResist)
        //{
            currentMaxSpeed = 0;
            entity.AddForce(Vector3.up * 40 * entity.mass * Time.deltaTime, ForceMode.Acceleration);
            entity.AddForce(Vector3.forward * -1000 * entity.mass * Time.deltaTime, ForceMode.Acceleration);
            entity.AddTorque(new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)).normalized * 90 * entity.mass * Time.deltaTime, ForceMode.Acceleration);

            /*//print("boom");
            blastProofSec = 4;
            if (blastProofSec >= 0)
            {
                blastProofSec -= Time.deltaTime;
                //print(blastProofSec);
            }
            if(blastProofSec <= 0)
            {
                blastResist = false;
                //print("blasting false");
            }
        }*/
    }

    public void changeRed() {
        if (number == 0)
            for (int i = 1; i < mat.Length; i++) {
                mat[i].SetColor("_EmissionColor", Color.Lerp(mat[i].GetColor("_EmissionColor"), Color.red, 0.2f));
                currentCol = Color.Lerp(mat[i].GetColor("_EmissionColor"), Color.red, 0.2f);
            }

        if (number == 1)
            for (int i = 0; i < mat.Length; i++) {
                mat[i].SetColor("_EmissionColor", Color.Lerp(mat[i].GetColor("_EmissionColor"), Color.red, 0.2f));
                currentCol = Color.Lerp(mat[i].GetColor("_EmissionColor"), Color.red, 0.2f);
            }
    }

    public void OutOfTime()
    {
        
    }


    public void changeBack() {
        if (number == 0)
            for (int i = 1; i < mat.Length; i++) {
                mat[i].SetColor("_EmissionColor", Color.Lerp(mat[i].GetColor("_EmissionColor"), originColor, 0.03f));
                currentCol = Color.Lerp(mat[i].GetColor("_EmissionColor"), Color.red, 0.03f);
            }

        if (number == 1)
            for (int i = 0; i < mat.Length; i++) {
                mat[i].SetColor("_EmissionColor", Color.Lerp(mat[i].GetColor("_EmissionColor"), originColor, 0.2f));
                currentCol = Color.Lerp(mat[i].GetColor("_EmissionColor"), Color.red, 0.2f);
            }
    }
}
