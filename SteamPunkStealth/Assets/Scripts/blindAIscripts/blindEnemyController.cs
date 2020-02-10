using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class blindEnemyController : MonoBehaviour
{

    public float lookRadius = 30f;
    Transform target;
    public NavMeshAgent agent;
    
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
    bool stoppedAngry = false;
    Vector3 center;



    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.instance.player.transform;
       
        agent = GetComponent<NavMeshAgent>();
        clock = true;
        
        rate = 1;

    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        
        playerCrouching(distance, lookRadius);


        if (rate == 1)
        {
            roam();
        }

   
        if(Input.GetKey(KeyCode.C))
        {
            done = true;
        }


        
    
        timer++;
        finishSearch++;
    }

    void playerCrouching(float distance, float lookRadius)
    {
        if (Input.GetKeyDown(KeyCode.C) && distance <= lookRadius)
        {
            
            if (clear == true)
            {
              //  Debug.Log("Going for the player.");
                lastPosition = target.position;
                rate = 0;
                clear = false;
                
                agent.SetDestination(lastPosition);
                center = lastPosition;

              



            }

           
       
            


        }


        if(done && distance <= lookRadius)
        {
            if (clock == true)
            {
                searchTimer++;
            }
            if(searchTimer % 500 == 0)
            {
                angry = true;
                //start angry with timer inside function
               
                angryRoam();
                clock = false;
                searchTimer = 0;
            }
            

        }

        
        if (distance <= lookRadius && Input.GetKeyUp(KeyCode.C))
        {

            rate = 1;
            agent.isStopped = false;
            agent.SetDestination(target.position);
            clear = true;
            done = false;
            clock = true;
            angry = false;
            stoppedAngry = false;
          //  Debug.Log("Going for the player.");



        }
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
           

        }

         if (distance <= lookRadius && rate == 1 && clear == true)
        {
            agent.SetDestination(target.position);

        }



    }

    void facePlayer()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 7f);

    }


   bool insideSphere(Vector3 position,  Vector3 location)
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
        if (timer % 100 == 0)
        {
            idleRoam();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
