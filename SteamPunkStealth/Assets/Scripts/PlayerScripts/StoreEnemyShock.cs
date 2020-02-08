using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreEnemyShock : MonoBehaviour
{

    public bool enemyDectected;
    public GameObject storedEnemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col) 
    {
        //Debug.Log("Shock Collision");
        
        if (col.gameObject.tag == "Enemy") 
        {
            storedEnemy = col.gameObject;
            enemyDectected = true;
        }        
    }

    void OnTriggerExit(Collider col) 
    {
        if (col.gameObject.tag == "Enemy") 
        {
            storedEnemy = null;
            enemyDectected = false;
        }
    }

}
