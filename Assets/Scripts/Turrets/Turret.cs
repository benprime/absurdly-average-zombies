using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Turret : MonoBehaviour
{
    // aiming stuff
    public List<GameObject> zombiesInRange;
    private Transform target;
    public float rotationSpeed;
    public float shootWithinDegrees = 10f;

    // firing stuff
    public GameObject bulletPrefab;
    public float shotDelay;
    public Transform barrelTip;
    Animator animator;
    private float lastShotTime;

    // turret stats
    public int costCurrency;
	public int baseCost;
    public float maxHitPoints;
    public float currentHitPoints;
    public int rangeLevel, damageLevel, speedLevel;
    public float rangeIncrease, damageIncrease, speedIncrease;
    public float pauseAfterFiring;
    public int baseDamage;

	public AudioClip[] shotSounds;

    [HideInInspector]
    public int damage;

    // Use this for initialization
    void Start()
    {
        this.animator = GetComponent<Animator>();
        this.damage = this.baseDamage;

		AudioSource aud = GetComponent<AudioSource>();
		int randSound = Random.Range (0,shotSounds.Count());			
		aud.clip = shotSounds[randSound];
    }

    // Update is called once per frame
    void Update()
    {
        // stats
        if (currentHitPoints <= 0)
        {
            Destroy(gameObject);
        }
        // aiming
        if (zombiesInRange.Count > 0)
        {
            LookAtNearestEnemy();

        }
    }

    public void Fire()
    {

        if (Time.time > this.lastShotTime + shotDelay)
        {
            // create projectile object
            GameObject clone = Instantiate(bulletPrefab, barrelTip.transform.position, gameObject.transform.rotation) as GameObject;

            // Hack! Make the rockets home in on targets.
            Rocket_Behavior rb = clone.GetComponent<Rocket_Behavior>();
            if (rb)
            {
                rb.target = this.target.gameObject;
            }

            // set the damage on the bullet
            // TODO: perhaps set a reference to the turret instead of
            // passing around the damage value.
            Bullet_Behavior b = clone.GetComponent<Bullet_Behavior>();
            b.turret = this;
            b.SetDamage(this.damage);

            animator.SetTrigger("Fire");

            this.lastShotTime = Time.time;

			AudioSource aud = GetComponent<AudioSource>();
			aud.Stop();
			aud.Play();
        }
    }

    void TakeDamage(float damage)
    {
        currentHitPoints -= damage;
        if (currentHitPoints < (maxHitPoints / 3))
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    void LookAtNearestEnemy()
    {

        // pause after firing (if set)
        // This exists mostly to let the fire animations complete before the turret
        // starts rotating again.  The catapult should be still while projectile leaves the bucket.
        if (this.lastShotTime > 0 && Time.time < this.lastShotTime + this.pauseAfterFiring)
            return;

        // remove all the zombies that have died
        while (zombiesInRange.Remove(null)) ;

        // if no zombies in detection zone, return
        if (zombiesInRange.Count <= 0)
        {
            return;
        }

        // This prevents a turret from getting "stuck" targetting a
        // zombie that has left the detection area without dying.
        if (target && !zombiesInRange.Contains(target.gameObject))
        {
            target = null;
        }

        // once we pick a target, we stick to it until it is dead or leaves the detection area
        // except for the flamethrower, which tries to get all nearby units on fire
        // TODO: Find a cleaner way to determine if this turret is a flamethrower
        Fireball_Behavior fb = this.bulletPrefab.GetComponent<Fireball_Behavior>();
        if (fb)
        {
            target = zombiesInRange.OrderBy(x => x.GetComponent<Zombie>().fireDamage).First().transform;
        }
        // everything else locks onto one guy and tries to kill him
        else if (!target)
        {
            // grab the lead zombie
            target = zombiesInRange.OrderBy(x => x.GetComponent<Zombie>().spawnTime).First().transform;
        }



        // rotate toward the target
        float step = rotationSpeed * Time.deltaTime;
        Vector3 targetRotation = Vector3.Normalize(target.position - transform.position);
        transform.up = Vector3.RotateTowards(transform.up, targetRotation, step, 0.0f);

        // if the target is within the "shootWithinDegrees" property, we fire
        float angleDiff = Mathf.Abs(Vector3.Angle(transform.up, targetRotation));
        if (angleDiff <= this.shootWithinDegrees)
        {
            this.Fire();
        }
    }
}
