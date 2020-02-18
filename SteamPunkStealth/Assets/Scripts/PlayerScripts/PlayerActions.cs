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
    }

    void Update()
    {      
        TeleportCountdown();
        ThrowTeleport();
        TeleportToDisk();

        DrawBow();
        
        ShockEnemy();

        GoggleToggle();
        GoggleCountdown();
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
            Debug.Log("Teleport" + TeleportDiskInstance.transform.position);

            TDIrenderer.enabled = false;
            Player.transform.SetPositionAndRotation(TeleportDiskInstance.transform.position, Player.transform.rotation);
            TDIcollider.enabled = false; 
            isThrown = false;
            Debug.Log(Player.transform.position);
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

    #region Bow Functions
    void DrawBow() 
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Arrow = GameObject.Instantiate(arrowPrefab, arrowSpawnPoint.transform.position, arrowSpawnPoint.transform.rotation);
            Arrow.transform.parent = arrowSpawnPoint.transform;
            arrowRB = Arrow.GetComponent<Rigidbody>();
            arrowRB.useGravity = false;
        }
        
        if (Input.GetMouseButton(0)) 
        {
            if (CurrrentDraw <= maxDraw) 
            {
                CurrrentDraw = CurrrentDraw += (Time.deltaTime * drawSpeed);
                
            }
        }

        if (Input.GetMouseButtonUp(0)) 
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
        if (Input.GetKeyDown(KeyCode.G) && !gogglesActive) 
        {
            Debug.Log("Goggles Activate");
            goggleActiveTimer = 0.4f;
            gogglesActive = true;
            Goggles.GetComponent<Camera>().enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.G) && gogglesActive && goggleActiveTimer <= 0)
        {
            Debug.Log("Goggles Deactivate");
            gogglesActive = false;
            Goggles.GetComponent<Camera>().enabled = false;
        }
    }
    #endregion 
}
