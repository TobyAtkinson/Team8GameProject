using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour {

	private Camera playerCamera;

	// Use this for initialization
	void Start () 
	{
		playerCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(playerCamera != null) 
		{
			transform.LookAt(playerCamera.transform.position, Vector3.up);
		}
		else
		{
			playerCamera = Camera.main;
			
		}
	}
}
