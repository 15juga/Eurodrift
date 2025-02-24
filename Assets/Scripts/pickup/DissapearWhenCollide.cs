using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissapearWhenCollide : MonoBehaviour
{
    float timer;

    public float respawnSec = 3;

    private Vector3 startPos;
    private Vector3 activePos;

    bool IsInActive;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;

        IsInActive = false;
        startPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        activePos = new Vector3(transform.position.x, transform.position.y + 100, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsInActive)
        {
            timer += Time.deltaTime;
            if (timer > respawnSec)
            {
                print("timer off");
                //gameObject.SetActive(true);
                transform.position = startPos;
                IsInActive = false;
                timer = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            print("boom");
            //gameObject.SetActive(false);
            transform.position = activePos;
            IsInActive = true;
        }
    }
}
