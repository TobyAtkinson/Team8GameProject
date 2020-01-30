using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{

    #region Teleport Disk Variables
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
    public int throwForce;
    public float thrownTime;
    public float teleportDelay;
    #endregion
    #region Shock Gaunlet Variables
    public GameObject shockGadgetRange;
    public GameObject enemyToShock;
    #endregion


    void Start()
    {
        #region Teleport Disk Component Assignments
        TeleportDiskInstance = Instantiate(TeleportDiskPrefab, Player.transform.position, Player.transform.rotation);
        // TDI stands for Teleport Disk Instance
        TDIrigidbody = TeleportDiskInstance.GetComponent<Rigidbody>();
        TDIcollider = TeleportDiskInstance.GetComponent<MeshCollider>();
        TDIrenderer = TeleportDiskInstance.GetComponent<MeshRenderer>();
        TDItransform = TeleportDiskInstance.GetComponent<Transform>();
        TDIcollider.enabled = false;
        TDIrenderer.enabled = false;
        #endregion
    }

    void Update()
    {
        TeleportCountdown();
        ThrowTeleport();
        TeleportToDisk();
        ShockEnemy();
    }

    #region Teleport Disk Functions
    void TeleportCountdown() 
    {
        if (isThrown) 
        {
            thrownTime = thrownTime -= Time.deltaTime;
        }

        if (!isThrown) 
        {
            thrownTime = 0.4f;
        }
    }
    
    void ThrowTeleport()
    {
        Debug.DrawRay(PlayerCam.transform.position, PlayerCam.transform.forward, Color.black, 1f);
        if (Input.GetKeyDown(KeyCode.E) && !isThrown)
        {
            Debug.Log("Throw Teleport Disk");
            TDIcollider.enabled = true;
            TDIrenderer.enabled = true;
            TDItransform.position = DiskSpawnPoint.position;
            TDItransform.rotation = DiskSpawnPoint.rotation;
            TDIrigidbody.AddForce(TDItransform.forward * throwForce);
            isThrown = true;
            TeleportDiskInstance.GetComponent<TeleportDiskController>().isFlying = true;
        }
    }

    void TeleportToDisk()
    {
        if (Input.GetKeyDown(KeyCode.E) && isThrown && thrownTime <= 0)
        {
            Debug.Log("Teleport");

            TDIrenderer.enabled = false;
            Player.transform.SetPositionAndRotation(TeleportDiskInstance.transform.position, Player.transform.rotation);
            TDIcollider.enabled = false; 
            isThrown = false;
        }
    }
    #endregion

    #region Shock Gauntlet Functions
    void ShockEnemy() 
    {
        enemyToShock = shockGadgetRange.GetComponent<StoreEnemyShock>().storedEnemy;
        if (Input.GetKeyDown(KeyCode.Q) == true && shockGadgetRange.GetComponent<StoreEnemyShock>().enemyDectected == true) 
        {
            
            enemyToShock.GetComponent<ShockGadgetReceiver>().Shock();
        }
    }
    #endregion


}
