using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public GameObject guard;

    float viewThreshhold = 1.1f;





    public enum swordState
    {
        Idle,
        FirstSwing,
        SecondSwing,
        Blocking
    }

    public bool windowForSecondSwing = false;

    public swordState currentSwordState = swordState.Idle;

    void Awake()
    {
        swordKillCollider = sword.GetComponent<BoxCollider>();
    }

    void Start()
    {
        currentHealth = maxiumunHealth;
        swordAnim = sword.GetComponent<Animator>();
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "GuardSpear")
        {
            other.enabled = false;
            if(isBlocking)
            {

                Enemy enemyScript = other.transform.root.GetComponent<Enemy>();
                enemyScript.Parried();
                spark1.Play();
                spark2.Play();


            }
            else
            {
                TakeDamage(50);
            }

            
        }
    }

    void Update()
    {
        if (isDead == true)
        {
            //Debug.LogError("Player dead");
        }

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 toOther = guard.transform.position - transform.position;
        float angle = Vector3.Dot(forward, toOther);

        print(Vector3.Dot(forward, toOther) > 0);

        if (angle > viewThreshhold)
        {
            print("guard infront");
        }
    

        RaycastHit hitInfo;
        if (Physics.Raycast(Camera.main.transform.position, transform.forward, out hitInfo, 2f, executeIgnoreLayers))
        {
            if (hitInfo.transform.root.name == "Enemy")
            {
                skullUi.SetActive(true);
               
            }
            else
            {
                skullUi.SetActive(false);
            }
        }
        else
        {
            skullUi.SetActive(false);
        }
            
        


        if (Input.GetMouseButton(0))
        {
            if(currentSwordState == swordState.Idle)
            {
                //swordAnim.enabled = false;
                //swordAnim.enabled = true;
                // start first swing
                windowForSecondSwing = false;
                currentSwordState = swordState.FirstSwing;
                StartCoroutine(FirstSwing());

            }
            else if (currentSwordState == swordState.FirstSwing && windowForSecondSwing == true)
            {
                //swordAnim.enabled = false;
                //swordAnim.enabled = true;
                //start second swing
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
            if(currentSwordState == swordState.Idle)
            {
                windowForSecondSwing = false;
                currentSwordState = swordState.Blocking;
                StartCoroutine(Block());
            }
            
        }
    }

    IEnumerator FirstSwing()
    {
        swordAnim.Play("SwordAttack1");
        yield return new WaitForSeconds(0.15f);
        // Activate kill barrier
        swordKillCollider.enabled = true;
        yield return new WaitForSeconds(0.25f);
        windowForSecondSwing = true;
        if(currentSwordState == swordState.FirstSwing)
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
        
        

        //0.70 left
    }
    IEnumerator SecondSwing()
    {
        swordAnim.Play("SwordAttack2");
        windowForSecondSwing = false;
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
}
