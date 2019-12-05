using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class forcetest : MonoBehaviour {
    public float thrust;

	 public float backwardsthrust;
    public Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
		rb.AddForce(transform.up * thrust);
		//rb.AddForce(transform.forward * backwardsthrust);
    }

    void FixedUpdate()
    {
       // rb.AddForce(transform.up * thrust);
    }
}