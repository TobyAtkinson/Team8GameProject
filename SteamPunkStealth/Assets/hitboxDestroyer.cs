using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitboxDestroyer : MonoBehaviour
{
    float lifetime = 0.1f;
    blindEnemyController blindGuardCon;
    public bool playerHit = false;
    // Start is called before the first frame update
    void Start()
    {
        GameObject blindGuard = GameObject.Find("blindGuard");
        blindGuardCon = blindGuard.GetComponent<blindEnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lifetime > 0)
        {
            lifetime = lifetime - Time.deltaTime;
        }
        else
        {
            if(!blindGuardCon.spawnHitbox)
            {
                Destroy(this.gameObject);
            }
            
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("Player hit by boss!");
            playerHit = true;
        }
        playerHit = false;
    }
}
