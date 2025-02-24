using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    public Transform objectToFollow;
    public Vector3 offset;
    public float followspeed = 10;
    public float lookSpeed = 10;
    public LayerMask mask;

    private RaycastHit ray;
    private Vector3 targetPos;
    
    public void LookAtTarget() {
        Vector3 lookDirection = objectToFollow.position - transform.position + new Vector3(0, 2, 0);
        Quaternion rot = Quaternion.LookRotation(lookDirection, Vector3.up);
        //Quaternion rot = objectToFollow.rotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, lookSpeed * Time.deltaTime);
    }

    public void MoveToTarget() {
        Vector3 vel = objectToFollow.GetComponent<Rigidbody>().velocity;

        if (new Vector3(vel.x, 0, vel.z).magnitude >= 5) {
            if (Physics.Raycast(objectToFollow.position, Quaternion.LookRotation(vel.normalized) * offset.normalized, out ray, offset.magnitude, mask))
                targetPos = Vector3.Lerp(targetPos, objectToFollow.position + Quaternion.LookRotation(vel.normalized) * offset.normalized * ray.distance, 0.4f);
            else
                targetPos = Vector3.Lerp(targetPos, objectToFollow.position + Quaternion.LookRotation(vel.normalized) * offset, 0.4f);

        } else {
            if (Physics.Raycast(objectToFollow.position, objectToFollow.rotation * offset.normalized, out ray, offset.magnitude, mask)) 
                targetPos = Vector3.Lerp(targetPos, objectToFollow.position + objectToFollow.rotation * offset.normalized * (ray.distance - 0.1f), 0.4f);
            else
                targetPos = Vector3.Lerp(targetPos, objectToFollow.position + objectToFollow.rotation * offset, 0.4f);
        }

        transform.position = Vector3.Lerp(transform.position, targetPos, followspeed * Time.deltaTime);
    }

    public void FixedUpdate() {
        LookAtTarget();
        MoveToTarget();
    }
}
