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

    [SerializeField]
    private float maxiumunHealth;

    [SerializeField]
    private float currentHealth;

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

    void Awake()
    {
        swordKillCollider = sword.GetComponent<BoxCollider>();
        playerMovementScript = this.GetComponent<PlayerMovement>();
        cameraScript = camera.gameObject.GetComponent<CameraController>();
    }

    void Start()
    {
        currentHealth = maxiumunHealth;
        swordAnim = sword.GetComponent<Animator>();
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        healthText.text = ("Health: " + currentHealth);
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            healthText.text = (" ----------------------       Dead");
            Destroy(this.gameObject);
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "GuardSpear")
        {
            other.enabled = false;
            if (isBlocking)
            {

                Enemy enemyScript = other.transform.root.GetComponent<Enemy>();
                enemyScript.Parried();
                spark1.Play();
                spark2.Play();


            }
            else
            {
                TakeDamage(25);
            }


        }
    }

    void Update()
    {
        if (isDead == true)
        {
            //Debug.LogError("Player dead");
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
                            enemyToExecute.isMovementLocked = true;
                            playerMovementScript.movementLocked = true;
                            cameraScript.turningLocked = true;
                            currentSwordState = swordState.Executing;
                            ableToExecute = false;
                            StartCoroutine(Execute());
                        }
                        else
                        {
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
        if (Input.GetMouseButton(1))
        {
            if (currentSwordState == swordState.Idle)
            {
                windowForSecondSwing = false;
                currentSwordState = swordState.Blocking;
                StartCoroutine(Block());
            }

        }
    }

    IEnumerator FirstSwing()
    {
        swordAnim.SetBool("Attack1", true);

        yield return new WaitForSeconds(0.25f);

        windowForSecondSwing = true;


        yield return new WaitForSeconds(0.25f);
        if(currentSwordState == swordState.FirstSwing)
        {
            windowForSecondSwing = false;
            currentSwordState = swordState.Idle;
            yield return new WaitForSeconds(0.50f);
            swordAnim.SetBool("Attack1", false);
        }


     
    }
    IEnumerator SecondSwing()
    {
        swordAnim.SetBool("Attack2", true);
        swordAnim.SetBool("Attack1", false);

        yield return new WaitForSeconds(0.5f);
        currentSwordState = swordState.Idle;
        swordAnim.SetBool("Attack2", false);
 
    }
    IEnumerator Block()
    {
        swordAnim.Play("SwordBlock");

        //yield return new WaitForSeconds(0.10f);
        // Activate block
        isBlocking = true;
        yield return new WaitForSeconds(0.60f);
        // Deactive block
        isBlocking = false;
        yield return new WaitForSeconds(0.65f);
        currentSwordState = swordState.Idle;


        // 1.25 overall

    }
    IEnumerator Execute()
    {
        swordAnim.Play("SwordExecute");
        yield return new WaitForSeconds(0.75f);
        // execute
        enemyToExecute.TakeDamage(500f);

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
     */






    //0.70 left
}
