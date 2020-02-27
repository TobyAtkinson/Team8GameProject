using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class blindEnemyController : MonoBehaviour
{

    public float lookRadius = 30f;
    Transform target;
    public NavMeshAgent agent;
    Vector3 location;
    Vector3 min;
    Vector3 max;
    int rate;
    int radius;
    int timer = 0;
    int searchTimer = 0;
    Vector3 lastPosition;
    bool clear = true;
    

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.instance.player.transform;
       
        agent = GetComponent<NavMeshAgent>();
        min.x = 1;
        min.y = 0;
        min.z = 1;
        max.x = 25;
        max.y = 0;
        max.z = 25;
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

        /*if (distance <= lookRadius)
            agent.SetDestination(target.position);
*/


        timer++;
        
    }

    void playerCrouching(float distance, float lookRadius)
    {
        if (Input.GetKeyDown(KeyCode.C) && distance <= lookRadius)
        {
            
            if (clear == true)
            {
                lastPosition = target.position;
                rate = 0;
                
                agent.SetDestination(lastPosition);
            }

            if (transform.position == lastPosition && clear == true)
            {
                agent.isStopped = true;
                clear = false;
                Debug.Log("enemy stops");
                searchTimer++;
                if (searchTimer % 10 == 0)
                {
                    agent.isStopped = false;
                    rate = 1;
                    roam();
                    
                }

            }

        }
         if (distance <= lookRadius && Input.GetKeyUp(KeyCode.C))
        {

            rate = 1;
            agent.isStopped = false;
            agent.SetDestination(target.position);
            clear = true;




        }
         if (distance > lookRadius)
        {
            rate = 1;
            agent.isStopped = false;
            clear = true;
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

    void idleRoam()
    {
       
              

        
        radius = Random.Range(1, 15);
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

    void searchForPlayer(float distance)
    {
        radius = Random.Range(1, 5);
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        lookRoam(ref randomDirection);
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, radius, 1);
        Vector3 finalPos = hit.position;
        agent.SetDestination(finalPos);
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
