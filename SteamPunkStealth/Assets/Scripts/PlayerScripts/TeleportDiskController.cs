using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportDiskController : MonoBehaviour
{
    Rigidbody rb;
    public bool isFlying;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isFlying = true;
    }

    void Update()
    {
        if (isFlying == true)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        }

        else 
        {
            rb.constraints = RigidbodyConstraints.None;
        }
    }

    void OnCollisionEnter(Collision col) 
    {
        if (col.collider.gameObject.layer == 9)
        {
            isFlying = false;
        }

        else 
        {
            isFlying = true;
        }
    }
}
