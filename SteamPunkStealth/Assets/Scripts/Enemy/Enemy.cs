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
        Lostplayer, // Sight -Enemy was red alarmed but now no longer sees player
        Atacking, // Combat - Enemy is swinging at player
        Kicking, // Combat - Enemy is kicking player as he is too close
        Stunned, // Combat - Enemy was parried by player
        Ready // Combat - Currently still

    }
    [Header("Variables to not edit")]

   
	private PlayerMovement player;




    [SerializeField]
    private GameObject body;

    [SerializeField]
    private GameObject attackKickSpear;

    [SerializeField]
    private GameObject idleStunnedSpear;

    [SerializeField]
    private GameObject walkSpear;

    [SerializeField]
    private GameObject ReadySpear;

    private Animator anim;

    private float dist;

	private EnemyUI ui;

    [SerializeField]
    private GameObject _player;

	[SerializeField]
	private float LookRoationSpeed = 3.5f;

    NavMeshAgent enemyAgent;


    public enemyState currentMovementState;

  
    public enemyState currentAlarmState = enemyState.NotAlarmed;

    

    private Transform lookTowardsHere;

    private Transform lookTowardsHere2;

    public Vector3 wherePlayerLastSeen;

    [SerializeField]
    private ParticleSystem bloodSplat;


    [SerializeField]
    private SpriteRenderer exclemationMarkUI;

    [SerializeField]
    private GameObject stunnedUI;

    [Header("Combat Variables")]

  
    public enemyState currentCombatState = enemyState.Ready;

    [SerializeField]
    private float maxiumunHealth;

    [SerializeField]
    private float currentHealth;

    [SerializeField]
    private bool isDead;

    private BoxCollider spearKillCollider;

    [SerializeField]
    private GameObject aliveGuard;

    [SerializeField]
    private GameObject deadGuard;

    public bool isMovementLocked;

    private PlayerCombat playerCombatScript;

    [Header("Variables you can customize")]
    public GameObject gaurdPoint;



    public GameObject gaurdPoint2;

    private GameObject currentPoint;

    [SerializeField]
    private bool PatrollingEnemy;


    private int currentGuardPointToGo = 1;

    [SerializeField]
    private int patrolDelay = 3;

    [SerializeField]
    private float walkSpeed;

    float viewAngle = 170;





    void Awake () 
	{
		ui = GetComponent<EnemyUI>();
        anim = body.GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player");
        playerCombatScript = _player.GetComponent<PlayerCombat>();
        enemyAgent = GetComponent<NavMeshAgent>();
        enemyAgent.speed = walkSpeed;
        spearKillCollider = attackKickSpear.GetComponent<BoxCollider>();
       

        if (gaurdPoint != null)
        {
            lookTowardsHere = gaurdPoint.transform.GetChild(0);
            currentPoint = gaurdPoint;
            enemyAgent.SetDestination(currentPoint.transform.position);
        }
        else
        {
            Debug.LogError("Guard does not have 1st patrol point set!");
        }

        if (PatrollingEnemy == true && gaurdPoint2 != null)
        {
            currentGuardPointToGo = 2;
            lookTowardsHere2 = gaurdPoint2.transform.GetChild(0);
            StartCoroutine(NextPatrolPointDelay());
        }
        else if (PatrollingEnemy == true && gaurdPoint2 == null)
        {
            Debug.LogError("Guard is supposed to patrol but 2nd guard point is not set!");
        }
    }
	

	
	void Start()
	{
        currentAlarmState = enemyState.NotAlarmed;
        currentMovementState = enemyState.Patrolling;
        currentCombatState = enemyState.Ready;
        currentHealth = maxiumunHealth;
    }

    public bool isPlayerAhead(GameObject player)
    {
        /*
        float angle = Vector3.Dot(forward, toOther);

        print(Vector3.Dot(forward, toOther) > 0);

        if (angle > 1)
        {
            print("infront");
           return true;
        }
        else
        {
            print("behind");
           return false;
        }
        */

        Vector3 forward = transform.forward;
        Vector3 toOther = transform.position - player.transform.position;
        float checkAngle = Vector3.Angle(forward, toOther);
        bool returnState = (checkAngle >= viewAngle * .5f);
        print(returnState);
        return returnState;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            this.gameObject.transform.DetachChildren();
            Destroy(aliveGuard);
            deadGuard.SetActive(true);
            Destroy(this.gameObject);
        }
        else
        {
            bloodSplat.Play();
        }
    }

    public void Backstab()
    {
            isDead = true;
            Destroy(this.gameObject);
    }

    void Update()
	{
        if(isDead == true)
        {
            //Debug.LogWarning("Guard dead");
        }

        if(isMovementLocked)
        {
            enemyAgent.speed = 0;
        }
        
        if (currentAlarmState == enemyState.NoticedPlayer && currentMovementState == Enemy.enemyState.Stationary && currentCombatState != enemyState.Stunned)
        {
            // Enemy should be yellow arrow, glancing towards player
            Vector3 direction = (wherePlayerLastSeen - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * (LookRoationSpeed * 0.5f));
            anim.SetBool("isWalking", false);
            walkSpear.SetActive(false);
            idleStunnedSpear.SetActive(true); 
        }
        else if(currentAlarmState == enemyState.AlarmedbyPlayer && currentMovementState == Enemy.enemyState.Chasing && currentCombatState != enemyState.Stunned)
        {
            enemyAgent.SetDestination(wherePlayerLastSeen);
            anim.SetBool("isWalking", true);
            
            idleStunnedSpear.SetActive(false);
            
            dist = Vector3.Distance(player.transform.position, transform.position);

            if (dist <= 3.4f)
            {
                enemyAgent.speed = 0;
                Vector3 direction = (wherePlayerLastSeen - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * (LookRoationSpeed * 6f));
                anim.SetBool("inRange", true);
                walkSpear.SetActive(false);

               
                if(currentCombatState == enemyState.Ready)
                {
                    ReadySpear.SetActive(true);
                    if (dist >= 2.0f)
                    {
                        // attack
                        currentCombatState = enemyState.Atacking;
                        StartCoroutine(Attack());
                    }
                    else
                    {
                        // kick
                        currentCombatState = enemyState.Kicking;
                        StartCoroutine(Kick());
                    }
                }
                
            }
            else if(currentCombatState == enemyState.Ready)
            {
                enemyAgent.speed = walkSpeed;
                anim.SetBool("inRange", false);
                ReadySpear.SetActive(false);
                walkSpear.SetActive(true);
            }
            
        }
        else if (currentAlarmState == enemyState.NotAlarmed && currentMovementState == Enemy.enemyState.Stationary && currentCombatState != enemyState.Stunned)
        {
            if (currentGuardPointToGo == 1)
            {
                Vector3 direction = (lookTowardsHere.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * LookRoationSpeed);
                anim.SetBool("isWalking", false);
                walkSpear.SetActive(false);
                idleStunnedSpear.SetActive(true);
           
            }
            else
            {
                Vector3 direction = (lookTowardsHere2.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * LookRoationSpeed);
                anim.SetBool("isWalking", false);
                walkSpear.SetActive(false);
                idleStunnedSpear.SetActive(true);
               
            }
        }
        else if (currentAlarmState == enemyState.NotAlarmed && currentMovementState == Enemy.enemyState.Patrolling)
        {
            if (gaurdPoint != null)
            {
                gameObject.GetComponent<NavMeshAgent>().enabled = true;
                //Debug.Log("going towards patrol point");
                enemyAgent.SetDestination(currentPoint.transform.position);
                anim.SetBool("isWalking", true);
                walkSpear.SetActive(true);
                idleStunnedSpear.SetActive(false);
                
                
            }
        }

        if(currentAlarmState == enemyState.NotAlarmed && currentMovementState == enemyState.Chasing && currentCombatState != enemyState.Stunned)
        {
            currentMovementState = enemyState.Patrolling;
        }
    }

    IEnumerator Attack()
    {
        ReadySpear.SetActive(false);
        attackKickSpear.SetActive(true);
        Debug.Log("Attack");
        anim.SetBool("isAttacking", true);
        anim.SetTrigger("attack");
        yield return new WaitForSeconds(0.60f);
        spearKillCollider.enabled = true;
        if(currentCombatState != enemyState.Stunned)
        {
            yield return new WaitForSeconds(0.50f);
            spearKillCollider.enabled = false;
            yield return new WaitForSeconds(0.65f);

            attackKickSpear.SetActive(false);
            anim.SetBool("isAttacking", false);
            if (currentCombatState != enemyState.Stunned)
            {
                currentCombatState = enemyState.Ready;
            }
        }

        

        // 0.75 is when box collider should be on
        // 1.75 overall
       
    }

    public void Parried()
    {
        currentCombatState = enemyState.Stunned;
        spearKillCollider.enabled = false;
        attackKickSpear.SetActive(false);
        anim.SetBool("isStunned", true);
        anim.SetBool("isAttacking", false);
        //anim.SetTrigger("stun");
        idleStunnedSpear.SetActive(true);
        exclemationMarkUI.enabled = false;
        stunnedUI.SetActive(true);
        StartCoroutine(Stunned());
        
    }

    IEnumerator Stunned()
    {
        Debug.Log("Stunned");

        yield return new WaitForSeconds(4.0f);
        exclemationMarkUI.enabled = true;
        idleStunnedSpear.SetActive(false);
        stunnedUI.SetActive(false);
        currentCombatState = enemyState.Ready;
        anim.SetBool("isStunned", false);
    }
        
    IEnumerator Kick()
    {
        attackKickSpear.SetActive(true);
        ReadySpear.SetActive(false);
        Debug.Log("Kick");
        anim.SetBool("isKicking", true);
        anim.SetTrigger("kick");
        yield return new WaitForSeconds(0.4f);

        playerCombatScript.TakeDamage(10);
      
        yield return new WaitForSeconds(0.8f);

        attackKickSpear.SetActive(false);
        anim.SetBool("isKicking", false);
        currentCombatState = enemyState.Ready;

        //1.2 overall
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerSword")
        {
            other.enabled = false;
            TakeDamage(50);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject == currentPoint)
        {
            if (currentAlarmState == enemyState.NotAlarmed)
            {
                gameObject.GetComponent<NavMeshAgent>().enabled = false;
                Debug.Log("connected with point");
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

    IEnumerator YellowAlarmOffDelay()
    {
        ui.EnemyDidntSeePlayer();
        yield return new WaitForSeconds(0.2f);
    }

    IEnumerator NextPatrolPointDelay()
    {
        while(true)
        {
            if(currentAlarmState == enemyState.NotAlarmed)
            {
                if (currentGuardPointToGo == 1)
                {
                    currentGuardPointToGo = 2;
                    currentPoint = gaurdPoint2;
                    gameObject.GetComponent<NavMeshAgent>().enabled = true;
                    currentMovementState = enemyState.Patrolling;
                }
                else
                {
                    currentGuardPointToGo = 1;
                    currentPoint = gaurdPoint;
                    gameObject.GetComponent<NavMeshAgent>().enabled = true;
                    currentMovementState = enemyState.Patrolling;
                }
                
            }
            
            yield return new WaitForSeconds(patrolDelay);
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







