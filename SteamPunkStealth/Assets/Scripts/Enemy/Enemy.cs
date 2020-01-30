using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    public enum enemyState 
    {
        Stationary, // Enemy standing still
        Patrolling, // Enemy moving from point to point
        Chasing, // Enemy moving towards player
        GoingTowardsLastSeen, // Enemy is going towards where the player was seen last
        NotAlarmed, // Sight - Enemy has not seen anything
        NoticedPlayer, // Sight - Enemy is yellow alarmed noticed player but not reacted
        AlarmedbyPlayer, // Sight - Enemy is red alarmed looking at player
        Lostplayer // Sight -Enemy was red alarmed but now nolonger sees player
            }

	private PlayerMovement player;

	private EnemyUI ui;

    [SerializeField]
    private GameObject _player;

	[SerializeField]
	private float LookRoationSpeed = 3.5f;

    NavMeshAgent enemyAgent;

    [SerializeField]
    public enemyState currentMovementState = enemyState.Stationary;

    [SerializeField]
    public enemyState currentAlarmState = enemyState.NotAlarmed;

    public GameObject lookTowardsHere;

    public GameObject gaurdPoint;

    public Vector3 wherePlayerLastSeen;

    void Awake () 
	{
		ui = GetComponent<EnemyUI>();
	}
	

	
	void Start()
	{
		_player = GameObject.FindGameObjectWithTag("Player");
        enemyAgent = GetComponent<NavMeshAgent>();

        currentAlarmState = enemyState.NotAlarmed;
        currentMovementState = enemyState.Stationary;
    }

	void Update()
	{
        if(currentAlarmState == enemyState.NoticedPlayer && currentMovementState == Enemy.enemyState.Stationary)
        {
            // Enemy should be yellow arrow, glancing towards player
            Vector3 direction = (wherePlayerLastSeen - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * (LookRoationSpeed * 0.5f));
        }
        else if(currentAlarmState == enemyState.AlarmedbyPlayer && currentMovementState == Enemy.enemyState.Chasing)
        {
            enemyAgent.SetDestination(wherePlayerLastSeen);
            //Debug.Log("going towards player");
        }
        else if (currentAlarmState == enemyState.NotAlarmed && currentMovementState == Enemy.enemyState.Stationary)
        {
            if (lookTowardsHere != null)
            {
                Vector3 direction = (lookTowardsHere.transform.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * LookRoationSpeed);
            }
        }
        else if (currentAlarmState == enemyState.NotAlarmed && currentMovementState == Enemy.enemyState.Patrolling)
        {
            if (gaurdPoint != null)
            {
                //Debug.Log("going towards patrol point");
                enemyAgent.SetDestination(gaurdPoint.transform.position);
            }
        }

        if(currentAlarmState == enemyState.NotAlarmed && currentMovementState == enemyState.Chasing)
        {
            currentMovementState = enemyState.Patrolling;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Gaurdpoint")
        {
            if (currentAlarmState == enemyState.NotAlarmed)
            {
                gameObject.GetComponent<NavMeshAgent>().enabled = false;
                currentMovementState = enemyState.Stationary;
            }
        }

    }

    public bool AnotherGuardHasSeen()
    {
        if(currentAlarmState == enemyState.AlarmedbyPlayer)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
	  	
	
	public void PlayerNoticed(PlayerMovement player, Vector3 whereToLook)
	{
		this.player = player;
        wherePlayerLastSeen = whereToLook;
        currentAlarmState = enemyState.NoticedPlayer;
		if (ui != null)
			ui.NoticedPlayer();
	}

	public void AlertedToPlayer(PlayerMovement player)
	{
		this.player = player;

        gameObject.GetComponent<NavMeshAgent>().enabled = true;
        currentAlarmState = enemyState.AlarmedbyPlayer;
        currentMovementState = enemyState.Chasing;
		if (ui != null)
			ui.AlarmedByPlayer();


	}

	public void EnemyDidntSeePlayer(PlayerMovement player)
	{
		this.player = player;
        currentAlarmState = enemyState.NotAlarmed;

        if (ui != null)
            StartCoroutine(YellowAlarmOffDelay());


	}

 
	public void EnemyLostPlayer(PlayerMovement player)
	{
        currentAlarmState = enemyState.NotAlarmed;
        currentMovementState = enemyState.Patrolling;
		this.player = player;
		if (ui != null)
        {
            ui.EnemyDidntSeePlayer();
            //ui.NoticedPlayer();
        }
    }

    /*
    public void EnemyGivingUpSearch(PlayerMovement player)
    {
        currentAlarmState = enemyState.NotAlarmed;
        currentMovementState = enemyState.Stationary;
        this.player = player;
        if (ui != null)
        {
            ui.EnemyDidntSeePlayer();
            //ui.NoticedPlayer();
        }
    }

    */


    IEnumerator YellowAlarmOffDelay()
    {
        
        ui.EnemyDidntSeePlayer();
        yield return new WaitForSeconds(0.2f);
    }

    /*

		private GameObject PlayerGO;


	[SerializeField]
	private int RatHealth = 150;

	public float range = 60f;

	public GameObject ShotgunMuzzleFlash;

	public bool canShoot = true;

	public GameObject DeadRat;
	public GameObject RatBody;
	public GameObject ShotgunImpactEffect;

	
	void Start () 
	{
		PlayerGO = GameObject.FindGameObjectWithTag("Player");
		DeadRat = this.gameObject.transform.GetChild(0).gameObject;
		RatBody = this.gameObject.transform.GetChild(1).gameObject;
		ShotgunMuzzleFlash = this.gameObject.transform.GetChild(2).gameObject;
		
	}
	
	
	 
	void Update () 
	{
		transform.LookAt(Camera.main.transform.position, Vector3.up);
		float dist = Vector3.Distance(PlayerGO.transform.position, transform.position);
        //print("Distance to other: " + dist);
		if(dist < range && canShoot == true)
		{
			canShoot = false;
			Debug.Log("Shot");

			

			ShotgunMuzzleFlash.SetActive(true);
			StartCoroutine(Shootdelay());
		
			RaycastHit hitInfo;
			if(Physics.Raycast(transform.position, transform.forward, out hitInfo, 50))
			{
				GameObject ImpactGO = Instantiate(ShotgunImpactEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
				Destroy(ImpactGO, 6.5f);
				//Debug.Log("Raycast hitted to: " + hitInfo.collider);

				
				if(hitInfo.collider.tag == "Player")
				{
					Debug.Log("You hit the player");
					hitInfo.collider.gameObject.GetComponent<Player>().TakeDamage(20);
					

					
					//hitInfo.collider.gameObject.GetComponent<Enemy>().smaller();

				}
			}
		}
	}


	public void Shot(int Damage)
	{
		RatHealth -= Damage;
		if(RatHealth < 1)
		{
			Destroy(RatBody);
			DeadRat.SetActive(true);
			transform.DetachChildren();
			Destroy(this.gameObject);

			
		}



	}

	IEnumerator Shootdelay()
	{
		yield return new WaitForSeconds(5f);
		ShotgunMuzzleFlash.SetActive(false);
		canShoot = true;
		
		
		

	}
}
	 */
}

	

	
	 


