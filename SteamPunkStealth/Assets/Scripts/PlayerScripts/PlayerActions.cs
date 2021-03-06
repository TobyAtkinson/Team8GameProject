﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public GameObject AbilityWheel;
    public int AWC;
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
    #region Bow Variables
    public GameObject arrowPrefab;
    public GameObject arrowSpawnPoint;
    GameObject Arrow;
    Rigidbody arrowRB;
    public float drawSpeed;
    public float maxDraw;
    public float arrowVelocity;
    public float CurrrentDraw;
    public float extraArrowForce;
    #endregion
    #region Goggles Variables 
    public bool gogglesActive;
    public GameObject Goggles;
    public float goggleActiveTimer;
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
        AWC = 1;
    }

    void Update()
    {
        ActionScroll();
        TeleportCountdown();
        ThrowTeleport();
        TeleportToDisk();
        TeleportReset();
        DrawBow();
        
        ShockEnemy();

        GoggleToggle();
        GoggleCountdown();
        AbilityWheelToggle();

    }

    #region Teleport Disk Functions

    void TeleportReset()
    {
        if (AWC != 3 && isThrown)
        {
            TDIrenderer.enabled = false;
            TDIcollider.enabled = false;
            isThrown = false;
        }
    }

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
        if (AWC == 3)
        {
            Debug.DrawRay(PlayerCam.transform.position, PlayerCam.transform.forward, Color.black, 1f);
            if (Input.GetKeyDown(KeyCode.E)  && !isThrown)
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
    }

    void TeleportToDisk()
    {
        if (AWC == 3)
        {
            if (Input.GetKeyDown(KeyCode.E) && isThrown && thrownTime <= 0)
            {
                Debug.Log("Teleport" + TeleportDiskInstance.transform.position);

                TDIrenderer.enabled = false;
                Player.transform.position = TeleportDiskInstance.transform.position;
                TDIcollider.enabled = false; 
                isThrown = false;
                Debug.Log(Player.transform.position);
            }
        }
    }
    #endregion

    #region Shock Gauntlet Functions
    void ShockEnemy() 
    {
        if (AWC == 2)
        {
            enemyToShock = shockGadgetRange.GetComponent<StoreEnemyShock>().storedEnemy;
            if (Input.GetKeyDown(KeyCode.E) == true && shockGadgetRange.GetComponent<StoreEnemyShock>().enemyDectected == true) 
            {
            
                enemyToShock.GetComponent<ShockGadgetReceiver>().Shock();
            }
        }
       
    }
    #endregion

    #region Bow Functions
    void DrawBow() 
    {
        if (Input.GetKeyDown("up"))
        {
            Arrow = GameObject.Instantiate(arrowPrefab, arrowSpawnPoint.transform.position, arrowSpawnPoint.transform.rotation);
            Arrow.transform.parent = arrowSpawnPoint.transform;
            arrowRB = Arrow.GetComponent<Rigidbody>();
            arrowRB.useGravity = false;
        }
        
        if (Input.GetKey("up"))
        {
            if (CurrrentDraw <= maxDraw) 
            {
                CurrrentDraw = CurrrentDraw += (Time.deltaTime * drawSpeed);
                
            }
        }

        if (Input.GetKeyUp("up")) 
        {
            arrowVelocity = CurrrentDraw;
            Arrow.transform.parent = null;
            FireArrow();
            CurrrentDraw = 0;
        }
    }

    void FireArrow() 
    {
        arrowRB.AddForce(arrowSpawnPoint.transform.up * (CurrrentDraw * extraArrowForce));
        arrowRB.useGravity = true;
    }
    #endregion

    #region Goggle Functions

    void GoggleCountdown() 
    {
        if (gogglesActive)
        {
            goggleActiveTimer = goggleActiveTimer -= Time.deltaTime;
        }
    }
    
    
    void GoggleToggle() 
    {
        if (AWC == 1)
        {
            if (Input.GetKeyDown(KeyCode.E) && !gogglesActive) 
            {
                Debug.Log("Goggles Activate");
                goggleActiveTimer = 0.4f;
                gogglesActive = true;
                Goggles.GetComponent<Camera>().enabled = true;
            }

            if (Input.GetKeyDown(KeyCode.E) && gogglesActive && goggleActiveTimer <= 0)
            {
                Debug.Log("Goggles Deactivate");
                gogglesActive = false;
                Goggles.GetComponent<Camera>().enabled = false;
            }
        }      
    }
    #endregion 

    void ActionScroll()
    {
        Debug.Log("Action Scroll Active");
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("AWC + 1");
            AWC = AWC + 1;
            if (AWC >= 4)
            {
                AWC = 1;
            }
        }
    }
    void AbilityWheelToggle()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            AbilityWheel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            AbilityWheel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

    }
}
