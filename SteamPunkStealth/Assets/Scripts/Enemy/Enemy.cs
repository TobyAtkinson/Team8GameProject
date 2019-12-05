using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	private PlayerMovement player;

	private EnemyUI ui;

    [SerializeField]
    private GameObject _player;

	[SerializeField]
	private float LookRoationSpeed = 3.5f;


    public bool noticedPlayer;

    public bool alarmedByPlayer;
	

	void Awake () 
	{
		ui = GetComponent<EnemyUI>();
	}
	

	
	void Start()
	{
		_player = GameObject.FindGameObjectWithTag("Player");
	}

	void Update()
	{
        if(alarmedByPlayer)
        {
            Vector3 direction = (_player.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * LookRoationSpeed);
        }
    }
	  	
	
	public void PlayerNoticed(PlayerMovement player)
	{
		this.player = player;

		if (ui != null)
			ui.NoticedPlayer();
	}

	public void AlertedToPlayer(PlayerMovement player)
	{
		this.player = player;

        alarmedByPlayer = true;
		if (ui != null)
			ui.AlarmedByPlayer();


	}

	public void EnemyDidntSeePlayer(PlayerMovement player)
	{
		this.player = player;

		if (ui != null)
			ui.EnemyDidntSeePlayer();
	}

	public void EnemyLostPlayer(PlayerMovement player)
	{
        alarmedByPlayer = false;
		this.player = player;
		if (ui != null)
			ui.NoticedPlayer();
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

	

	
	 


