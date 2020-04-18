using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blindGuardHitbox : MonoBehaviour
{

     blindEnemyController blindGuardCon;
     public  GameObject hitbox;
     public  Transform hitboxPosition;
     float lifetime = 5f;
    public  bool destroyHitbox = false;
    public bool knockback = false;
    // Start is called before the first frame update
    void Start()
    {
        GameObject blindGuard = GameObject.Find("blindGuard");
        blindGuardCon = blindGuard.GetComponent<blindEnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(blindGuardCon.spawnHitbox)
        {
            destroyHitbox = false;
            Instantiate(hitbox, hitboxPosition.position, hitboxPosition.rotation);
            knockback = true;
        }
        else
        {
            destroyHitbox = true;
            knockback = false;
        }
    }
}
