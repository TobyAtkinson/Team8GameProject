﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class blindEnemyController : MonoBehaviour
{

    public float lookRadius = 30f;
    Transform target;
    public NavMeshAgent agent;

   
    PlayerMovement playerMovement;

    public AudioClip clip;
    public AudioClip punchSound;
    AudioSource source;

    int rate;
    int radius;
    int timer = 0;
    int searchTimer = 0;
    Vector3 lastPosition;
    bool clear = true;
    bool done = false;
    bool angry;
    bool clock;
    int finishSearch = 0;
    int radius2;
    bool chargePlayer = false;
    bool stoppedAngry = false;
    Vector3 center;
    Animator anim;
    bool attack = false;
    int animTimer = 0;
    bool animToIdle = false;
   public bool spawnHitbox = false;
    bool punchSoundPlay;
    //tobys  ints for battle
    public int speed;
    int punchTimer = 0;
    bool punchSoundAllow;
    public float health;
    public Image healthProgressBar;

    public PlayerCombat playerCombatScript;


    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.instance.player.transform;

        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        clock = true;

        rate = 1;

        GameObject player = GameObject.Find("NewPlayer");
        playerMovement = player.GetComponent<PlayerMovement>();

        source = GetComponent<AudioSource>();

        speed = 0;

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Explosive")
        {
            health -= 25f;
            
            //toby change
            healthProgressBar.fillAmount = health /100f;
            if (health <= 0f)
            {
                playerCombatScript.Finished();
                Destroy(this.gameObject);
            }
        }
    }


        // Update is called once per frame
        void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (!chargePlayer)
        {
            playerCrouching(distance, lookRadius);
        }

        chargeAttack();
        attackPlayer();

        if (rate == 1)
        {
            roam();
        }


        if (playerMovement.isCrouched)
        {
            done = true;
        }

       

        if(reachedDestination() && !attack)
        {
            if (animToIdle)
            {
                animTimer++;
                anim.SetInteger("condition", 5);
                spawnHitbox = false;
            }
            if (animTimer % 20 == 0)
            {
                anim.SetInteger("condition", 2);
                animTimer = 0;
                animToIdle = false;
                spawnHitbox = false;
            }
            
            
        }
        else
        {
            animToIdle = true;
        }


       

        timer++;
        finishSearch++;
    }

    void playerCrouching(float distance, float lookRadius)
    {

        // If the Player crouches using left control and is in the Blind Enemy's area of detection, the Enemy goes to the position where he last heard the player.
        if (playerMovement.isCrouched && distance <= lookRadius)
        {

            if (clear == true)
            {
                source.PlayOneShot(clip, 0.2f);
                //  Debug.Log("Going for the player.");
                lastPosition = target.position;
                rate = 0;
                clear = false;

                agent.SetDestination(lastPosition);
                center = lastPosition;





            }






        }

        // Look for player around the position where the player was heard last
        if (done && distance <= lookRadius)
        {
            if (clock == true)
            {
                searchTimer++;
            }
            if (searchTimer % 500 == 0)
            {
                angry = true;
                //start angry with timer inside function
                anim.SetInteger("condition", 1);
                angryRoam();
                clock = false;
                searchTimer = 0;
                spawnHitbox = false;
            }


        }

        // If the player stands up while they're still in the area of detection, attack him
        if (distance <= lookRadius && !playerMovement.isCrouched)
        {
            source.PlayOneShot(clip, 0.2f);
            rate = 1;
            agent.isStopped = false;
            agent.SetDestination(target.position);
            clear = true;
            done = false;
            clock = true;
            angry = false;
            stoppedAngry = false;
            //  Debug.Log("Going for the player.");
            anim.SetInteger("condition", 3);
            spawnHitbox = false;

        }

        // If the player got out of the area of detection, continue roaming peacefully
        if (distance > lookRadius)
        {
            rate = 1;
            agent.isStopped = false;
            clear = true;
            done = false;
            clock = true;
            angry = false;
            stoppedAngry = false;
            roam();
            spawnHitbox = false;

        }

        if (distance <= lookRadius && rate == 1 && clear == true && done == false)
        {
            source.PlayOneShot(clip, 0.2f);
            agent.SetDestination(target.position);
            anim.SetInteger("condition", 3);
            spawnHitbox = false;

        }



    }


    //Look at the player
    void facePlayer()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 7f);

    }


    //If player bumps into the Enemy, they'll get attacked
    void attackPlayer()
    {

        if ((Vector3.Distance(transform.position, target.position) <= 2f && done == false) || (Vector3.Distance(transform.position, target.position) <= 1.7f && done))
        {
            punchSoundPlay = true;
            playPunch(punchSoundPlay);
            facePlayer();
            attack = true;
            agent.SetDestination(target.position);
            agent.speed = 0;
            spawnHitbox = true;
            StartCoroutine(HitAnim());
            /*
            anim.SetInteger("condition", 4);
            spawnHitbox = true;
            Debug.Log("Attacks player.");

    */
        }
        else
        {
            agent.speed = speed;
            attack = false;
            spawnHitbox = false;

        }
    }


    void playPunch(bool playSound)
    {
         punchTimer ++;
        if (playSound &&    punchTimer % 30 == 0)
        {
            source.PlayOneShot(punchSound, 0.7f);
            playSound = false;
            punchTimer = 0;

        }
       
    }
    // Placeholder method for when the Blind enemy will hear fighting
    void chargeAttack()
    {
        if (Input.GetKey(KeyCode.K))
        {
            chargePlayer = true;
            rate = 0;
            facePlayer();
            agent.SetDestination(target.position);
        }

        if (Input.GetKeyUp(KeyCode.K))
        {
            chargePlayer = false;
            rate = 1;
        }
    }


    bool insideSphere(Vector3 position, Vector3 location)
    {
        return Vector3.Distance(position, location) < radius;
    }

    bool reachedDestination()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }


    // Idle/peaceful roam
    void idleRoam()
    {


        // Debug.Log("Calm.");

        radius = Random.Range(1, 20);
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        lookRoam(ref randomDirection);
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, radius, 1);
        Vector3 finalPos = hit.position;
        agent.SetDestination(finalPos);


    }


    // Look for random spot to go to
    void lookRoam(ref Vector3 randomDirection)
    {

        Quaternion lookRotationRoam = Quaternion.LookRotation(new Vector3(randomDirection.x, 0, randomDirection.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotationRoam, Time.deltaTime * 7f);
    }

    void angryLook(ref Vector3 randomDirection)
    {
        Quaternion lookRotationRoam = Quaternion.LookRotation(new Vector3(randomDirection.x, 0, randomDirection.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotationRoam, Time.deltaTime * 9f);
    }


    // Look around for player in the last position they were heard
    void angryRoam()
    {
        if (angry && finishSearch % 80 == 0)
        {

            //  Debug.Log("Stressed.");

            radius2 = Random.Range(1, 9);
            Vector3 randomDirection2 = Random.insideUnitSphere * radius2;
            angryLook(ref randomDirection2);
            randomDirection2 += transform.position;
            NavMeshHit hit2;
            NavMesh.SamplePosition(randomDirection2, out hit2, radius2, 1);
            Vector3 finalPos2 = hit2.position;
            agent.SetDestination(finalPos2);





        }



    }



    void roam()
    {
       // anim.SetInteger("condition", 2);
        if (timer % 100 == 0)
        {
            anim.SetInteger("condition", 1);
            idleRoam();
        }
        
    }


    IEnumerator HitAnim()
    {
        yield return new WaitForSeconds(0.1f);
        anim.SetInteger("condition", 4);
        Debug.Log("Attacks player.");
       
    }

    IEnumerator PunchSound()
    {
        yield return new WaitForSeconds(1f);
        source.PlayOneShot(punchSound, 0.7f);

    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}