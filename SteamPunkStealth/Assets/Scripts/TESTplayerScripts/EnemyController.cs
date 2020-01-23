using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float lookRadius = 30f;

    Transform target;
    public NavMeshAgent agent;
    
    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        playerCrouching();

        if (distance <= lookRadius)
            agent.SetDestination(target.position);
       
    }

    void playerCrouching()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            agent.isStopped = true;
            
        }
        if(Input.GetKeyUp(KeyCode.C))
        {
            agent.isStopped = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
