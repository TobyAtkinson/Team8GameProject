using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemySight : MonoBehaviour
{
	[SerializeField]	
	private Enemy enemy;

	[SerializeField, Tooltip("How often the enemy will check if they can see the player.")]
	private float raycastDelay = 0.05f;

	[Space(20)]
	[Header("Reaction Time")]

	[SerializeField, Tooltip("How long it takes the enemy the enemy to react based on distance.")]
	private float noticeTimePerMetre = 10f;

	[SerializeField]
	private float noticeTimeScaler = 13f;

	[SerializeField]
	private float MinimunReactionTime = 0.5f;
	[SerializeField]
	private float MaximunReactionTime = 5.0f;

	private WaitForSeconds rcastDelay;

	private PlayerMovement player;

	private bool isPlayerInViewCollison = false;

	private bool isPlayerInRayCastSight = false;

	private bool enemyIsLookingAtPlayer = false;
	
	private Coroutine reaction;

	void Awake()
	{
		Debug.Assert(enemy != null, "Enemy class is null.");
		rcastDelay = new WaitForSeconds(raycastDelay);
	}

	void OnDrawGizmos()
	{
		if(isPlayerInViewCollison == true)
		{
			if (isPlayerInRayCastSight == true)
				Gizmos.color = Color.green;
			else
				Gizmos.color = Color.red;

			Gizmos.DrawRay (transform.position, player.transform.position - transform.position);
		}
	}
	
	void OnTriggerEnter(Collider other) 
	{
		if(other.gameObject.tag == "Player")
		{
			player = other.gameObject.GetComponent<PlayerMovement>();
			 if (player == null)
			 {
				 Debug.LogError("Player doesn't have a player script!");
			 }

			isPlayerInViewCollison = true;
			StartCoroutine(RayCastForPlayer());
		}     
        
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            // Saw another guard
            if (enemy.alarmedByPlayer == false)
            {
                // If guard is chasing player
                Enemy otherGuardScript = other.GetComponent<Enemy>();
                bool isOtherGaurdAlerted = otherGuardScript.AnotherGuardHasSeen();
                if (isOtherGaurdAlerted == true)
                {
                    enemy.AlertedToPlayer(player);
                }
            }

            //Enemy.
        }
        else if (other.gameObject.tag == "Gadget")
        {
            if (enemy.alarmedByPlayer == false)
                StartCoroutine(RayCastForSuspicosObject(other.gameObject));
        }
    }

        IEnumerator RayCastForSuspicosObject(GameObject target)
    {
        yield return new WaitForSeconds(0.05f);
        RaycastHit targetInfo;
        if (Physics.Raycast(transform.position, target.transform.position - transform.transform.position, out targetInfo))
        {
            if (targetInfo.collider.tag == "Gadget")
            {
                print("BIG GADGET SEEN");
                enemy.AlertedToPlayer(player);
            }
        }
    }
    IEnumerator RayCastForPlayer()
    {
		    while (isPlayerInViewCollison == true)
		    {
			RaycastHit hitInfo;
			if(Physics.Raycast(transform.position, player.transform.position - transform.transform.position, out hitInfo))
			{
				if(hitInfo.collider.tag == "Player")
				{
					if (isPlayerInRayCastSight == false)
					{
						// First time the enemy spots the player.
						if (reaction == null)
						{
							//Debug.Log("Player has caught an enemies eye.");
							reaction = StartCoroutine(ReactionToPlayer());
						}
					}

					isPlayerInRayCastSight = true;
					//Debug.Log(Vector3.Distance(transform.position, player.transform.position));
				}
				else
				{
					
					if(enemyIsLookingAtPlayer)
					{
						Debug.Log("Enemy lost player will search");
						enemy.EnemyLostPlayer(player);
						enemyIsLookingAtPlayer = false;
					}
					isPlayerInRayCastSight = false;
				}
			}

			yield return rcastDelay;
		}

		isPlayerInRayCastSight = false;
	}

	IEnumerator ReactionToPlayer()
	{
		float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
		float reactionTime = (distanceToPlayer/noticeTimePerMetre) * (distanceToPlayer/noticeTimeScaler);
		reactionTime = Mathf.Clamp(reactionTime, MinimunReactionTime, MaximunReactionTime);
		print("Enemy will  notice player in " + reactionTime + " seconds");
		enemy.PlayerNoticed(player);

		yield return new WaitForSeconds(reactionTime);

		if(isPlayerInRayCastSight == false)
		{
			Debug.Log("enemy saw nothing");
			enemy.EnemyDidntSeePlayer(player);
		}
		else
		{
			enemyIsLookingAtPlayer = true;
			Debug.Log("enemy HAS seen player");
			enemy.AlertedToPlayer(player);
		}

		reaction = null;
	}
	

	void OnTriggerExit(Collider other) 
	{
		if(other.gameObject.tag == "Player")
		{
			isPlayerInViewCollison = false;
		}
    }
}
