using UnityEngine;
using System.Collections;

public class ZombiePathfinder : Zombie {
	public GameObject enemyBase;

	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();
		enemyBase = GameObject.FindGameObjectWithTag("PlayerBase");

		this.direction = enemyBase.transform.position - this.transform.position;
		this.direction = this.direction.normalized * 1;
	}
	
	// Update is called once per frame
	protected override void Update()
	{
		base.Update ();
		//Vector3 targetRotation = Vector3.Normalize (enemyBase.transform.position - transform.position);	
		//transform.up = Vector3.RotateTowards (transform.up, targetRotation, turnSpeed * Time.deltaTime, 0);  //TODO: convert to Vector2
		//transform.position = Vector2.MoveTowards(transform.position, enemyBase.transform.position, moveSpeed * Time.deltaTime);
	}

}
