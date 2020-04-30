using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    GameObject spawner;
    blindGuardHitbox hitbox;
    PlayerCombat playerCombat;
    public float knockBackPower = 5f;
    Rigidbody rb;
    GameObject blindGuardEnemy;
   
    // Start is called before the first frame update
    void Start()
    {
        GameObject blindGuard = GameObject.Find("blindGuard");
        hitbox = blindGuard.GetComponent<blindGuardHitbox>();
        rb = this.GetComponent<Rigidbody>();
        spawner = GameObject.Find("hitboxSpawner");
        GameObject Player = GameObject.Find("NewPlayer");
        playerCombat = Player.GetComponent <PlayerCombat >();
        blindGuardEnemy = GameObject.Find("blindGuard");
     
        
    }

    // Update is called once per frame
    void Update()
    {

        if (hitbox.knockback && rb != null)
        {
            StartCoroutine(HitPlayer());

        }


            /*
             * KNOCKBACK
            Vector3 direction = blindGuardEnemy.transform.position - transform.position;
            direction.y = 0;
            rb.AddForce(direction.normalized * knockBackPower, ForceMode.Impulse);


    */
        }


    IEnumerator HitPlayer()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("PLAYERHIT!!!");
        playerCombat.TakeDamage(1);
        yield return new WaitForSeconds(51f);
    }

    }

   

