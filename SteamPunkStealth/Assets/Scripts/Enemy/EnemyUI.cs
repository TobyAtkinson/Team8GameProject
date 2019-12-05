using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Enemy))]
public class EnemyUI : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer alertIcon; 


	void Awake()
	{
		Debug.Assert(alertIcon != null, "Alert Icon not there");
	}
	void Start()
	{
		EnemyDidntSeePlayer();
	}
	
	public void NoticedPlayer()
	{
		alertIcon.color = Color.yellow;
		alertIcon.gameObject.SetActive(true);
	}
	
	public void AlarmedByPlayer()
	{
		alertIcon.color = Color.red;
		alertIcon.gameObject.SetActive(true);
	}

	public void EnemyDidntSeePlayer()
	{
		alertIcon.gameObject.SetActive(false);
	}	
}
