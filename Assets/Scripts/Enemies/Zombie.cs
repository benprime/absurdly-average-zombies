﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum ZombieState
{
	Normal,
	Dead
}

public class Zombie : MonoBehaviour {
	public float moveSpeed = 2f;
	public float moveModifier = 1.0f;
	public float turnSpeed = 4f;
	public float walkSwayModifier = 20;
	public float maxHitPoints = 10f;
	public float hitPoints = 10f;
	public int worthCurrency = 5;
	public float attackDamage = 5f;

	private UI_FloatingHealthBar hb;

	// These may get moved to a more appropriate location
	public float flameDamageInterval = 1f;
	public int fireDamage = 1;
	private float nextFlameDamageTime = 0f;

	public ParticleSystem bloodParticleSystem;

	private bool onFire = false;

	public ZombieState zombieState;

	public Vector3 direction;
	private int randSwayStart;
	
	private GameManager gm;

	protected Path_Create path;
	public int currentNodeIndex = 0;
	public float targetCloseness = .5f;

	public GameObject popNums;

	// Use this for initialization
	protected virtual void Start () {
		this.hb = GetComponent<UI_FloatingHealthBar>();
		gm = FindObjectOfType<GameManager>();
		path = FindObjectOfType<Path_Create> ();

		this.zombieState = ZombieState.Normal;
		// set travel direction
		//this.direction = new Vector3(Random.Range (-1f, 1f), Random.Range (-1f, 1f), 0);
		this.direction = Vector3.down;

		// without this new zombies kind of just stand around
		this.direction = this.direction.normalized * 1;

		// set initial rotation of zombie
		this.transform.up = this.direction;

		// randomize initial sway
		// so zombies don't all sway at the same time
		this.randSwayStart = Random.Range(0, 10) * 100;
		this.walkSwayModifier = Random.Range (20, 35);

		hitPoints = maxHitPoints;
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		if(!gm) gm = FindObjectOfType<GameManager>();  //TODO: probably move this to appropriate Awake function or whatever

		if (this.zombieState == ZombieState.Dead) {
			return;
		}

		if (this.onFire) {
			if (Time.time > nextFlameDamageTime) {
				nextFlameDamageTime += this.flameDamageInterval;
				this.TakeDamage(this.fireDamage);
			}
		}
		if (this.hitPoints <= 0) {
			this.Die();
			return;
		}


		// update zombie position, moving the direction
		//this.transform.position += this.direction * this.moveSpeed * Time.deltaTime;

		//pathfinding code
		GameObject currentNode = path.pathNodes[currentNodeIndex];
		Vector2 currentPosition = new Vector2 (transform.position.x, transform.position.y);
		this.direction = currentNode.transform.position - transform.position;
		transform.position = Vector2.MoveTowards(currentPosition, currentNode.transform.position, moveSpeed * Time.deltaTime * this.moveModifier);
		if(Vector2.Distance (currentPosition, currentNode.transform.position) < targetCloseness) {
			if(currentNodeIndex < path.pathNodes.Count - 1) currentNodeIndex++;
		}

		// reset the transform to the direction, so that when we apply the
		// sway code, it doesn't become cumulative and do some wonky stuff.
		// Probably not the best way to handle this, but it's simple for now.
		// We can re-address this later.
		this.transform.up = this.direction;

		Sway ();
	}

	protected virtual void Sway() {
		// z is -10 to 10 sway (in degrees)
		float z = Mathf.PingPong (Time.time * this.walkSwayModifier + randSwayStart, 20) - 10;
		
		// apply the "sway" rotate
		//transform.position.z = z;
		transform.Rotate (0.0f, 0.0f, z);
	}

	public virtual void TakeDamage(int amount)
	{
		this.hitPoints -= amount;

		if (this.hb.healthBar.rectTransform) {
			this.hb.healthBar.rectTransform.localScale = new Vector3 (Mathf.Max (0, (hitPoints / maxHitPoints)), 1, 1); //TODO: Make healthbar scale from left pivot point
		}

		//if(hitPoints <= 0) Destroy (hb.healthBar.gameObject);
		//else {
			this.GeneratePopUpNumber(amount.ToString (), Color.red, false);

			if(hitPoints <= maxHitPoints / 3) {
				hb.healthBar.color = Color.red;
			}
			else if(hitPoints <= 2 * (maxHitPoints / 3)) {
				hb.healthBar.color = Color.yellow;
			}
		//}

		ParticleSystem localBloodsObj = GameObject.Instantiate(this.bloodParticleSystem, this.transform.position, Quaternion.identity) as ParticleSystem;
		localBloodsObj.Play();
	}

	protected virtual void Die()
	{
		this.zombieState = ZombieState.Dead;
		CircleCollider2D c = GetComponent<CircleCollider2D> ();
		c.enabled = false;

		// TODO: set animation to death animation (via trigger)
		gm.SendMessage ("PlayerCurrencyTransaction", worthCurrency);
		this.GeneratePopUpNumber("$" + worthCurrency, Color.yellow, true);

		Destroy (this.hb.healthBar.gameObject);

		Destroy (gameObject);
	}

	void OnCollisionStay2D(Collision2D other) {
		//damage the buildings/turrets in path
		if(other.transform.tag == "Turret" || other.transform.tag == "PlayerBase") {
			other.gameObject.SendMessage ("TakeDamage", attackDamage * Time.deltaTime);
		}
	}

	public void CatchFire()
	{
		this.onFire = true;
		this.nextFlameDamageTime = Time.time;
		ParticleSystem ps = this.GetComponent<ParticleSystem> ();
		ps.Play ();
	}

	private void GeneratePopUpNumber(string txt, Color txtCol, bool largeText) {		
		GameObject pop = Instantiate (popNums) as GameObject;
		pop.transform.position = transform.position;
		pop.GetComponent<Text>().text = txt;
		pop.GetComponent<Text>().color = txtCol;
		if(largeText) pop.GetComponent<Text>().fontSize += 10;
	}
}
