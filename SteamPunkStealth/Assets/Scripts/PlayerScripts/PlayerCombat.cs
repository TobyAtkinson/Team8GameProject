using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{

    public LayerMask executeIgnoreLayers;

    [SerializeField]
    private GameObject sword;

    private Animator swordAnim;

    
    public float maxiumunHealth;


    public float currentHealth;

    [SerializeField]
    private bool isDead;

    private BoxCollider swordKillCollider;

    public bool isBlocking;

    [SerializeField]
    private ParticleSystem spark1;

    [SerializeField]
    private ParticleSystem spark2;

    [SerializeField]
    private GameObject skullUi;

    int timerStab = 0;

    float viewThreshhold = 1;

    [SerializeField]
    private Camera camera;

    [SerializeField]
    private Text healthText;

    private CameraController cameraScript;

    private bool ableToExecute;

    private Enemy enemyToExecute;

    private PlayerMovement playerMovementScript;

    [SerializeField]
    private MeshRenderer swordMeshRender;

    [SerializeField]
    private GameObject behindExecuteSword;

    [SerializeField]
    private GameObject deadguard;

    [SerializeField]
    private ParticleSystem blood;

    [SerializeField]
    private Animator BackStabAnim;

    [SerializeField]
    private GameObject deadguardprefab;

    [SerializeField]
    private bool blockOnCoolDown;

    public UiFader uiFadingScript;

    public GameObject[] tutorialWaypoints;

    public Objective objectiveScript;

    public int checkpointStage = 0;

    public Text instructions;

    public GameObject instructionsgroup;

    AudioManager manager;

    public GameObject key1;

    public GameObject key2;

    public GameObject gateGroupOpen;
    public GameObject gateGroupClosed;

    public GameObject waypoint;
    public GameObject objective1;
    public enum swordState
    {
        Idle,
        FirstSwing,
        SecondSwing,
        Blocking,
        Executing
    }

    public bool windowForSecondSwing = false;

    public swordState currentSwordState = swordState.Idle;

    public Image blackFadeImage;

    public CheckpointManager checkpointManager;

    void Awake()
    {
        swordKillCollider = sword.GetComponent<BoxCollider>();
        playerMovementScript = this.GetComponent<PlayerMovement>();
        cameraScript = camera.gameObject.GetComponent<CameraController>();
    }

    void Start()
    {
        //blackFadeImage.CrossFadeAlpha(0, 1f, false);
        instructionsgroup.SetActive(false);
        objectiveScript.target = tutorialWaypoints[0].transform;
        objectiveScript.offset = new Vector3(0, 1);
        currentHealth = maxiumunHealth;
        swordAnim = sword.GetComponent<Animator>();
        GameObject AudioManager = GameObject.Find("Audio Manager");
        manager = AudioManager.GetComponent<AudioManager>();

        checkpointManager = GameObject.FindGameObjectWithTag("CheckpointManager").GetComponent<CheckpointManager>();
        if(checkpointManager != null)
        {
            if(checkpointManager.Checkpointat == 0)
            {
                DojoStart();
                blackFadeImage.CrossFadeAlpha(0, 4f, false);
            }
            else if(checkpointManager.Checkpointat == 1)
            {
                this.gameObject.transform.position = new Vector3(-81.02f, 7.9f, -192.079f);
                objective1.SetActive(true);
                instructionsgroup.SetActive(false);
                blackFadeImage.CrossFadeAlpha(0, 1f, false);
            }
            else
            {
                this.gameObject.transform.position = new Vector3(-203.51f, 14.29f, -157.7f);
                gateGroupClosed.SetActive(false);
                waypoint.SetActive(false);
                blackFadeImage.CrossFadeAlpha(0, 1f, false);
            }
        }
        else
        {
            blackFadeImage.CrossFadeAlpha(0, 1f, false);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        manager.Play("Stab");
        currentHealth -= damageAmount;
        healthText.text = ("Health: " + currentHealth);
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            healthText.text = (" ----------------------       Dead");
            blackFadeImage.CrossFadeAlpha(1, 1.0f, false);
            StartCoroutine(Death());
            //Destroy(this.gameObject);
        }

    }

    IEnumerator Death()
    {
      
        yield return new WaitForSeconds(1.0f);
        Application.LoadLevel(Application.loadedLevel);

    }

    public void Finished()
    {
        blackFadeImage.CrossFadeAlpha(1, 10.0f, false);
    }

   



    void DojoStart()
    {
        this.gameObject.transform.position = new Vector3(-191.24f, 11.8f, -458.15f);
        instructionsgroup.SetActive(true);
        instructions.text = "To train your defensive skills as a monk, you must perfect the basics. Use Shift while moving with WASD to sprint. Sprint to the marker.";
        objective1.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject == key1)
        {
            if(key2 == null)
            {
                // BOTH KEYS DONE
                gateGroupClosed.SetActive(false);
                gateGroupOpen.SetActive(true);
                waypoint.SetActive(true);
            }
            Destroy(key1);
        }
        else if (other.gameObject == key2)
        {
            if (key1 == null)
            {
                // BOTH KEYS DONE
                gateGroupClosed.SetActive(false);
                gateGroupOpen.SetActive(true);
                waypoint.SetActive(true);
            }
            Destroy(key2);
        }


        if (other.gameObject == tutorialWaypoints[0])
        {
            if(checkpointStage == 0)
            {
                objectiveScript.target = tutorialWaypoints[1].transform;
                Destroy(other.gameObject);
                
                checkpointStage = 1;

                instructions.text = "Move stealthily by crouching with the CTRL key. Crouch under the obstacle.";
            }
            
        }
        else if (other.gameObject == tutorialWaypoints[1])
        {
            if (checkpointStage == 1)
            {
                objectiveScript.target = tutorialWaypoints[2].transform;
                Destroy(other.gameObject);
                
                checkpointStage = 2;

                instructions.text = "Use your agility to navigate areas. Walk up to a vine wall and hold W to climb up.";
            }
            
        }
        else if (other.gameObject == tutorialWaypoints[2])
        {
            if (checkpointStage == 2)
            {
                objectiveScript.target = tutorialWaypoints[3].transform;
                Destroy(other.gameObject);
                
                checkpointStage = 3;

                instructions.text = "Wall run to explore further. Jump to the wall while sprinting using SHIFT and SPACE, then hold W while keeping the wall on your left.";
            }
            
        }
        else if (other.gameObject == tutorialWaypoints[3])
        {
            if (checkpointStage == 3)
            {
                objectiveScript.target = tutorialWaypoints[4].transform;
                objectiveScript.offset = new Vector3(0, 8);
                //instructionsgroup.SetActive(false);

                StartCoroutine(DojoEnd());

                waypoint.SetActive(false);
                Destroy(other.gameObject);
                
                checkpointStage = 4;
            }
            
        }

        if (other.gameObject.tag == "Explosive")
        {
            TakeDamage(10);
        }


            if (other.gameObject.tag == "GuardSpear")
        {
            other.enabled = false;
            if (isBlocking)
            {

                Enemy enemyScript = other.transform.root.GetComponent<Enemy>();
                enemyScript.Parried();
                spark1.Play();
                spark2.Play();
                manager.Play("SwordClash");

            }
            else
            {
                TakeDamage(25);
            }


        }
    }

    IEnumerator DojoEnd()
    {
        blackFadeImage.CrossFadeAlpha(1, 9.0f, false);
        instructions.text = "Good work Fa Shen, you have trained well. Go to the emporers docks and find one of the warlords responsible for attacking the temples.";
        yield return new WaitForSeconds(9.0f);
        this.gameObject.transform.position = new Vector3(-81.02f, 7.9f, -192.079f);
        objective1.SetActive(true);
        instructionsgroup.SetActive(false);
        blackFadeImage.CrossFadeAlpha(0, 3f, false);

    }



    void Update()
    {
        if(currentSwordState == swordState.Executing)
        {
            
            manager.Play("GuardDeath");
        }

        
     
        if (isDead == true)
        {
            //Debug.LogError("Player dead");
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            DojoStart();
        }

            /*
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 toOther = guard.transform.position - transform.position;
            float angle = Vector3.Dot(forward, toOther);

            print(Vector3.Dot(forward, toOther) > 0);

            if (angle > viewThreshhold)
            {
                print("guard infront");
            }
            */


            RaycastHit hitInfo;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);


        if (Physics.Raycast(ray, out hitInfo, 2f, executeIgnoreLayers))
        {
            //Debug.Log("hit" + hitInfo.transform.tag);
            if (hitInfo.transform.CompareTag("Enemy"))
            {
                Enemy enemyscript = hitInfo.transform.root.GetComponent<Enemy>();
                if (enemyscript.currentCombatState == Enemy.enemyState.Stunned || enemyscript.currentAlarmState == Enemy.enemyState.NotAlarmed)
                {
                    skullUi.SetActive(true);
                    ableToExecute = true;
                    enemyToExecute = enemyscript;
                }

                //GuardHitBox
            }
            else
            {
                skullUi.SetActive(false);
                ableToExecute = false;
            }
        }
        else
        {
            skullUi.SetActive(false);
            ableToExecute = false;
        }







        if (Input.GetMouseButton(0))
        {

            if (currentSwordState == swordState.Idle)
            {
                
                if (ableToExecute == true)
                {
                    
                    if (playerMovementScript.isCrouched == false)
                    {
                       
                        bool isplayerahead = enemyToExecute.isPlayerAhead(this.gameObject);
                        if (isplayerahead == true)
                        {
                            timerStab++;
                           
                            enemyToExecute.isMovementLocked = true;
                            playerMovementScript.movementLocked = true;
                            cameraScript.turningLocked = true;
                            currentSwordState = swordState.Executing;
                            ableToExecute = false;
                            
                            StartCoroutine(Execute());
                            
                                
                                
                            

                        }
                        else
                        {
                            manager.Play("Stab");
                            enemyToExecute.isMovementLocked = true;
                            playerMovementScript.movementLocked = true;
                            cameraScript.turningLocked = true;
                            currentSwordState = swordState.Executing;
                            ableToExecute = false;
                            StartCoroutine(BehindExecute());
                        }

                        // true = player infront of guard
                        // false = player guard
                        Debug.Log("executing guard");

                    }

                }
                else
                {
                    //swordAnim.enabled = false;
                    //swordAnim.enabled = true;
                    // start first swing
                    swordAnim.SetBool("Attack1", false);
                    windowForSecondSwing = false;
                    currentSwordState = swordState.FirstSwing;
                    StartCoroutine(FirstSwing());
                }


            }
            else if (currentSwordState == swordState.FirstSwing && windowForSecondSwing == true)
            {
                currentSwordState = swordState.SecondSwing;
                windowForSecondSwing = false;
                swordKillCollider.enabled = false;
                StartCoroutine(SecondSwing());
            }
            else
            {
                // player is already doing something
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (currentSwordState != swordState.Blocking && blockOnCoolDown == false)
            {
                swordKillCollider.enabled = false;
                windowForSecondSwing = false;
                currentSwordState = swordState.Blocking;
                StartCoroutine(Block());
            }

        }
    }

    IEnumerator FirstSwing()
    {

        swordAnim.SetBool("Attack1", true);
        swordAnim.SetBool("Block", false);
        blockOnCoolDown = false;

        manager.Play("SwordSwing");
        yield return new WaitForSeconds(0.25f);

        windowForSecondSwing = true;


        yield return new WaitForSeconds(0.25f);
        if(currentSwordState == swordState.FirstSwing)
        {
            windowForSecondSwing = false;
            
            yield return new WaitForSeconds(0.50f);
            currentSwordState = swordState.Idle;
            swordAnim.SetBool("Attack1", false);
        }


     
    }
    IEnumerator SecondSwing()
    {
        swordAnim.SetBool("Attack2", true);
        swordAnim.SetBool("Attack1", false);
        manager.Play("SwordSwing2");
        yield return new WaitForSeconds(0.5f);
        currentSwordState = swordState.Idle;

        swordAnim.SetBool("Attack2", false);
 
    }
    IEnumerator Block()
    {
        blockOnCoolDown = true;
        swordAnim.SetBool("Block", true);
        swordAnim.SetBool("Attack1", false);
        swordAnim.SetBool("Attack2", false);
        isBlocking = true;
        yield return new WaitForSeconds(0.60f);
        isBlocking = false;
        currentSwordState = swordState.Idle;
        yield return new WaitForSeconds(0.40f);
        swordAnim.SetBool("Block", false);
        blockOnCoolDown = false;



        


        // 1.25 overall

    }
    IEnumerator Execute()
    {
        swordAnim.Play("SwordExecute");
        
        yield return new WaitForSeconds(0.75f);
        // execute
        enemyToExecute.TakeDamage(500f);
        manager.Play("Stab");
        yield return new WaitForSeconds(0.75f);
        currentSwordState = swordState.Idle;
        playerMovementScript.movementLocked = false;
        cameraScript.turningLocked = false;


        // 1.5 overall

    }
   
    IEnumerator BehindExecute()
    {
        behindExecuteSword.SetActive(true);
        BackStabAnim.Play("BackstabSword");
        swordMeshRender.enabled = false;
       
        deadguard.SetActive(true);
        blood.Play();
        enemyToExecute.Backstab();
        yield return new WaitForSeconds(1.3f);
        Instantiate(deadguardprefab, deadguard.transform.position, deadguard.transform.rotation);
        deadguard.SetActive(false);
        behindExecuteSword.SetActive(false);
        swordMeshRender.enabled = true;
        currentSwordState = swordState.Idle;
        playerMovementScript.movementLocked = false;
        cameraScript.turningLocked = false;
    }



    // first swing old
    /*

     swordAnim.Play("SwordAttack1");
     yield return new WaitForSeconds(0.15f);
     // Activate kill barrier
     swordKillCollider.enabled = true;
     yield return new WaitForSeconds(0.25f);
     windowForSecondSwing = true;
     if (currentSwordState == swordState.FirstSwing)
     {
         yield return new WaitForSeconds(0.15f);
         // Deactive kill barrier
         swordKillCollider.enabled = false;
         yield return new WaitForSeconds(0.15f);
         windowForSecondSwing = false;
         yield return new WaitForSeconds(0.15f);
         yield return new WaitForSeconds(0.75f);
         windowForSecondSwing = false;
         currentSwordState = swordState.Idle;
     }


    second swing old

     swordAnim.Play("SwordAttack2");
        
        yield return new WaitForSeconds(0.25f);
        // Activate kill barrier
        swordKillCollider.enabled = true;
        windowForSecondSwing = false;
        yield return new WaitForSeconds(0.30f);
        // Deactive kill barrier
        swordKillCollider.enabled = false;
        windowForSecondSwing = false;
        yield return new WaitForSeconds(0.3f);
        windowForSecondSwing = false;
        yield return new WaitForSeconds(0.75f);
        windowForSecondSwing = false;
        currentSwordState = swordState.Idle;

        //0.70 left


    block old

    swordAnim.Play("SwordBlock");

        //yield return new WaitForSeconds(0.10f);
        // Activate block
        isBlocking = true;
        yield return new WaitForSeconds(0.60f);
        // Deactive block
        isBlocking = false;
        yield return new WaitForSeconds(0.65f);
        currentSwordState = swordState.Idle;
     */

}
