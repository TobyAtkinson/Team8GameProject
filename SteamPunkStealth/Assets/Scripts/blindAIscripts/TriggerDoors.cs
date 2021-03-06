﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoors : MonoBehaviour
{
    public GameObject gateGroupOpen;
    public GameObject gateGroupClosed;
    public GameObject blindGuard;
    public GameObject waypoint;
    public GameObject objective1;
    public GameObject objective2;
    public GameObject healthBargroup;
    public CheckpointManager checkpointManager;




    private void Start()
    {
        checkpointManager = GameObject.FindGameObjectWithTag("CheckpointManager").GetComponent<CheckpointManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            blindGuard.GetComponent<blindEnemyController>().speed = 7;
            gateGroupOpen.SetActive(false);
            gateGroupClosed.SetActive(true);
            waypoint.SetActive(false);
            objective1.SetActive(false);
            objective2.SetActive(true);
            healthBargroup.SetActive(true);
            checkpointManager.Checkpointat = 2;
            Destroy(this);
            
        }
    }
}
