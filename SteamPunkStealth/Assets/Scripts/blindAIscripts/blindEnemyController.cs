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
    Transform wall;
    int radius;
    int timer = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.instance.player.transform;
        wall = PlayerManager.instance.wall.transform;
        agent = GetComponent<NavMeshAgent>();
        min.x = 1;
        min.y = 0;
        min.z = 1;
        max.x = 25;
        max.y = 0;
        max.z = 25;

    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        playerCrouching();
        if (timer % 100 == 0)
        {
            idleRoam();
        }

        if (distance <= lookRadius)
            agent.SetDestination(target.position);

        if(distance <= agent.stoppingDistance)
        {
            facePlayer();
        }
        timer++;
    }

    void playerCrouching()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            agent.isStopped = true;

        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            agent.isStopped = false;
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
        /*
                if (wall.position != location)
                {
                    MathUtilities.Random(ref location, min, max);
                    lookRoam(location);

                    agent.SetDestination(location);

                }
                */

        
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
     //   MathUtilities.Random(ref location, min, max);
     //  Vector3  roamDirection = (randomDirection - transform.position).normalized;
        Quaternion lookRotationRoam = Quaternion.LookRotation(new Vector3(randomDirection.x, 0, randomDirection.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotationRoam, Time.deltaTime * 7f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
