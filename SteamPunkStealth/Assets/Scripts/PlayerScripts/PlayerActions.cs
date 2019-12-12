﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public bool isThrown;
    public GameObject PlayerCam;
    public Transform DiskSpawnPoint;
    public GameObject TeleportDiskPrefab;
    GameObject TeleportDiskInstance;
    public Vector3 TeleportPosition;
    public GameObject Player;
    Rigidbody TDIrigidbody;
    Collider TDIcollider;
    public Transform TDItransform;
    Renderer TDIrenderer;
    void Start()
    {
        TeleportDiskInstance = Instantiate(TeleportDiskPrefab, Player.transform.position, Player.transform.rotation);
       
        TDIrigidbody = TeleportDiskInstance.GetComponent<Rigidbody>();
        TDIcollider = TeleportDiskInstance.GetComponent<MeshCollider>();
        TDIrenderer = TeleportDiskInstance.GetComponent<MeshRenderer>();
        TDItransform = TeleportDiskInstance.GetComponent<Transform>(); 
        
        TDIcollider.enabled = false;
        TDIrenderer.enabled = false;
    }

    void Update()
    {
        ThrowTeleport();

        TeleportToDisk();
    }

    void ThrowTeleport()
    {
        Debug.DrawRay(PlayerCam.transform.position, PlayerCam.transform.forward, Color.black, 1f);
        if (Input.GetMouseButtonDown(0) && !isThrown)
        {
            Debug.Log("Throw Teleport Disk");
            TDIcollider.enabled = true;
            TDIrenderer.enabled = true;
            TDItransform.position = DiskSpawnPoint.position;
            TDItransform.rotation = DiskSpawnPoint.rotation;
            TDIrigidbody.AddForce(TDItransform.forward * 1000);
            isThrown = true;
        }
    }

    void TeleportToDisk()
    {
        if (Input.GetMouseButtonDown(1) && isThrown)
        {
            Debug.Log("Teleport");

            TDIcollider.enabled = false; 
            TDIrenderer.enabled = false;
            Player.transform.SetPositionAndRotation(TeleportDiskInstance.transform.position, Player.transform.rotation);
            isThrown = false;
        }
    }
}
