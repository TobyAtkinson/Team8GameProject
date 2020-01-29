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

    [SerializeField]
    private bool isPlayerInViewCollison = false;

	private bool isPlayerInRayCastSight = false;

	private bool enemyIsLookingAtPlayer = false;
	
	private Coroutine reaction;

    private bool isRaycastingForPlayer;

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
        

    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player = other.gameObject.GetComponent<PlayerMovement>();
            if (player == null)
            {
                Debug.LogError("player doesnt have player script");
            }

            isPlayerInViewCollison = true;

            if (!isRaycastingForPlayer)
            {
                isRaycastingForPlayer = true;
                StartCoroutine(RayCastForPlayer());
            }

        }
        if (other.gameObject.tag == "Enemy")
        {
            // Saw another guard
            if (enemy.currentAlarmState != Enemy.enemyState.AlarmedbyPlayer)
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
        if (other.gameObject.tag == "Gadget")
        {
            if (enemy.currentAlarmState != Enemy.enemyState.AlarmedbyPlayer)
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
                //print("BIG GADGET SEEN");
                enemy.AlertedToPlayer(player);
            }


        }
    }
    IEnumerator RayCastForPlayer()
    {
		while (isPlayerInViewCollison == true || enemy.currentAlarmState == Enemy.enemyState.AlarmedbyPlayer)
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
					
					if(enemy.currentAlarmState == Enemy.enemyState.AlarmedbyPlayer)
					{
						Debug.Log("Enemy lost player will search");
                        StartCoroutine(GiveUpDelay());
						//enemy.EnemyLostPlayer(player);
                        //isPlayerInViewCollison = false;
					}
                    else if(enemy.currentAlarmState == Enemy.enemyState.NoticedPlayer)
                    {
                       
                        //isPlayerInViewCollison = false;
                    }
                    isPlayerInRayCastSight = false;
				}
			}

			yield return rcastDelay;
		}
        isRaycastingForPlayer = false;
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
            Debug.Log("Enemy did not notice player will return to patrol");
            enemy.EnemyDidntSeePlayer(player);
        }
		else
		{
			Debug.Log("enemy HAS seen player");
			enemy.AlertedToPlayer(player);
		}

		reaction = null;
	}

    IEnumerator GiveUpDelay()
    {
        yield return new WaitForSeconds(3.0f);
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, player.transform.position - transform.transform.position, out hitInfo))
        {
            if (hitInfo.collider.tag == "Player")
            {
                //keep up search
            }
            else
            {
                enemy.EnemyLostPlayer(player);
            }

        }
    }

    void OnTriggerExit(Collider other) 
	{
		if(other.gameObject.tag == "Player")
		{
			isPlayerInViewCollison = false;
		}
    }
}
