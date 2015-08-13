using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Turret : MonoBehaviour {
	// aiming stuff
	public List<GameObject> zombiesInRange;
	private Transform target;
	public float RotationSpeed;
	public float shootWithinDegrees = 10f;

	// firing stuff
	public GameObject bulletPrefab;
	public float shotDelay = .1f;
	public Transform barrelTip;
	Animator animator;
	private float lastShotTime;

	// turret stats
	public int costCurrency;
	public float maxHitPoints;
	public float currentHitPoints;
	public int rangeLevel, damageLevel, speedLevel;
	public float rangeIncrease, damageIncrease, speedIncrease;
	public float pauseAfterFiring;
	public int baseDamage;
	[HideInInspector]
	public int damage;

	// Use this for initialization
	void Start () {
		this.animator = GetComponent<Animator> ();
		this.damage = this.baseDamage;
	}
	
	// Update is called once per frame
	void Update () {
		// stats
		if (currentHitPoints <= 0) {
			Destroy(gameObject);
		}
		// aiming
		if(zombiesInRange.Count > 0) {
			LookAtNearestEnemy();

		}
	}

	public void Fire() {
		
		if(Time.time > this.lastShotTime + shotDelay)
		{
			// create projectile object
			GameObject clone = Instantiate(bulletPrefab, barrelTip.transform.position, gameObject.transform.rotation) as GameObject;

			// set the damage on the bullet
			// TODO: perhaps set a reference to the turret instead of
			// passing around the damage value.
			Bullet_Behavior b = clone.GetComponent<Bullet_Behavior>();
			b.SetDamage(this.damage);

			animator.SetTrigger("Fire");
			
			this.lastShotTime = Time.time;
		}
	}

	void TakeDamage(float damage) {
		currentHitPoints -= damage;
		if(currentHitPoints < (maxHitPoints / 3)) {
			gameObject.GetComponent<SpriteRenderer>().color = Color.red;
		}
	}
	
	void Upgrade() {
		float temp = transform.FindChild ("DetectionZone").GetComponent<CircleCollider2D>().radius;
		transform.FindChild ("DetectionZone").GetComponent<CircleCollider2D>().radius = temp * (rangeLevel * (1 + rangeIncrease));
	}

	void LookAtNearestEnemy() {

		// pause after firing (if set)
		if (this.lastShotTime > 0 && Time.time < this.lastShotTime + this.pauseAfterFiring)
			return;


		// remove all the zombies that have died
		while(zombiesInRange.Remove (null));

		if(zombiesInRange.Count <= 0) return;

		if(!target) target = zombiesInRange[0].transform;
		Transform nearest = target;
		float distanceToNearest = Vector3.Distance (nearest.position, this.transform.position);
		foreach (GameObject enemy in zombiesInRange) {
			float tempDist = Vector3.Distance (enemy.transform.position, this.transform.position);
			if(tempDist < distanceToNearest) {
				nearest = enemy.transform;
				distanceToNearest = tempDist;
			}
		}

		float step = RotationSpeed * Time.deltaTime;
		Vector3 targetRotation = Vector3.Normalize (nearest.position - transform.position);
		transform.up = Vector3.RotateTowards (transform.up, targetRotation, step, 0.0f);

		float angleDiff = Mathf.Abs (Vector3.Angle (transform.up, targetRotation));
		if (angleDiff <= this.shootWithinDegrees) {
			this.Fire();
		}
	}


}
