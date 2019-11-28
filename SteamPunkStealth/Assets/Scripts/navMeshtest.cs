using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class navMeshtest : MonoBehaviour
{
    public NavMeshAgent enemy;

    public GameObject destination;
    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();   
    }

   
    void Update()
    {
        enemy.SetDestination(destination.transform.position);
    }
}
