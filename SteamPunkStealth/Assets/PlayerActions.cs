using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public bool isThrown;
    public GameObject PlayerCam;
    public GameObject TeleportDiskInstance;
    public GameObject TeleportDisk;
    public Vector3 TeleportPosition;
    public GameObject Player;
    void Start()
    {
        
    }

    void Update()
    {
        ThrowTeleport();

        TeleportToDisk();

        TeleportPosition = TeleportDisk.transform.position;

        Player.transform.position = TeleportPosition;
    }

    void ThrowTeleport()
    {
        if (Input.GetMouseButtonDown(0) && !isThrown)
        {
            Debug.Log("Throw Teleport Disk");
            TeleportDisk = Instantiate(TeleportDiskInstance, PlayerCam.transform.position, PlayerCam.transform.rotation);
            TeleportDisk.GetComponent<Rigidbody>().AddForce(Vector3.forward * 1000);
            isThrown = true;
        }
    }

    void TeleportToDisk()
    {
        if (Input.GetMouseButtonDown(1) && isThrown)
        {
            Debug.Log("Teleport");
            Player.transform.SetPositionAndRotation(TeleportDisk.transform.position, Player.transform.rotation);
        }
    }
}
