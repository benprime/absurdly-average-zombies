using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public enum ZombieState
{
    Normal,
    Dead
}

public enum ZombieSize
{
    Small,
    Medium,
    Large,
	Pop,
    Boss
}

public class Zombie : MonoBehaviour
{
    public ZombieSize zSize;
    public float moveSpeed = 2f;
    public float moveModifier = 1.0f;
    public float turnSpeed = 4f;
    public float walkSwayModifier = 20;
    public float maxHitPoints = 10f;
    public float hitPoints = 10f;
    public int worthCurrency = 5;
    public float attackDamage = 5f;

    [HideInInspector]
    public float spawnTime;

    protected UI_FloatingHealthBar hb;

    // These may get moved to a more appropriate location
    public float flameDamageInterval = 1f;
    public float fireDamage = 0;
    protected float nextFlameDamageTime = 0f;

    public ParticleSystem hitParticleSystem;
    public ParticleSystem deathParticleSystem;

    // fire particle systems
    protected ParticleSystem psLarge;
    protected ParticleSystem psSmallLeft;
    protected ParticleSystem psSmallRight;


    public bool onFire = false;

    public ZombieState zombieState;

    public Vector3 direction;
    protected int randSwayStart;

    protected GameManager gm;

    public ZombiePath path = null;
    protected int currentNodeIndex = 0;
    public float targetCloseness = .5f;

    public GameObject popNums;

	public AudioClip[] deathSounds;

    void Awake()
    {
        this.spawnTime = Time.time;
    }

    // Use this for initialization
    protected virtual void Start()
    {
        this.hb = GetComponent<UI_FloatingHealthBar>();

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
        this.walkSwayModifier = Random.Range(20, 35);

        // save reference to particle systems for later use
        this.psLarge = this.transform.FindChild("ParticleSystemFireLarge").GetComponent<ParticleSystem>();
        this.psSmallLeft = this.transform.FindChild("ParticleSystemFireSmallLeft").GetComponent<ParticleSystem>();
        this.psSmallRight = this.transform.FindChild("ParticleSystemFireSmallRight").GetComponent<ParticleSystem>();

		hitPoints = maxHitPoints;

		AudioSource aud = GetComponent<AudioSource>();
		int randSound = Random.Range(0, deathSounds.Count());
		aud.clip = deathSounds[randSound];
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (this.zombieState == ZombieState.Dead)
        {
            return;
        }

        if (this.onFire)
        {
            if (Time.time > nextFlameDamageTime)
            {
                nextFlameDamageTime += this.flameDamageInterval;
                this.TakeDamage(this.fireDamage, DamageType.light);
                this.fireDamage--;
                if (this.fireDamage <= 0)
                {
                    psLarge.Stop();
                    psSmallLeft.Stop();
                    psSmallRight.Stop();

                    this.onFire = false;
                }
            }
        }

        if (this.hitPoints <= 0)
        {
            this.Die();
            return;
        }

        // update zombie position, moving the direction
        GameObject currentNode = path.nodes[currentNodeIndex];
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        direction = currentNode.transform.position - transform.position;
        var newDir = Vector3.RotateTowards(transform.up, direction, turnSpeed * Time.deltaTime * moveModifier, 0.0f);

        if (transform.up == direction)
            Sway();
        else
            transform.up = newDir;

        if (path.nodes.Count <= 0)
            return;

        transform.position = Vector2.MoveTowards(currentPosition, currentNode.transform.position, moveSpeed * Time.deltaTime * moveModifier);

        if (Vector2.Distance(currentPosition, currentNode.transform.position) < targetCloseness && currentNodeIndex < path.nodes.Count - 1)
            currentNodeIndex++;
    }

    protected void Sway()
    {
        // z is -10 to 10 sway (in degrees)
        float z = Mathf.PingPong(Time.time * this.walkSwayModifier + randSwayStart, 20) - 10;

        // apply the "sway" rotate
        //transform.position.z = z;
        transform.Rotate(0.0f, 0.0f, z);
    }

    public enum DamageType { light, medium, heavy }
    public void TakeDamage(float amount, DamageType dmgType)
    {
        this.hitPoints -= amount * DamageTypeModifier(dmgType);

        if (this.hb.healthBar.rectTransform)
        {
            this.hb.healthBar.rectTransform.localScale = new Vector3(Mathf.Max(0, (hitPoints / maxHitPoints)), 1, 1); //TODO: Make healthbar scale from left pivot point
        }

        //if(hitPoints <= 0) Destroy (hb.healthBar.gameObject);
        //else {
        //this.GeneratePopUpNumber(amount.ToString(), Color.red, false);

        if (hitPoints <= maxHitPoints / 3)
        {
            hb.healthBar.color = Color.red;
        }
        else if (hitPoints <= 2 * (maxHitPoints / 3))
        {
            hb.healthBar.color = Color.yellow;
        }
        //}

        GameObject.Instantiate(this.hitParticleSystem, this.transform.position, Quaternion.identity);
        //localBloodsObj.transform.rotation = rotation;
		//localBloodsObj.transform.localRotation; 
        //localBloodsObj.Play();
    }

    protected virtual void Die()
	{
		AudioSource aud = GetComponent<AudioSource>();
		if (aud) {
			aud.Stop ();
			aud.Play ();
		}

        this.zombieState = ZombieState.Dead;
        CircleCollider2D c = GetComponent<CircleCollider2D>();
        c.enabled = false;

        // TODO: set animation to death animation (via trigger)
		GameManager.instance.PlayerCurrencyTransaction (worthCurrency);
        this.GeneratePopUpNumber("$" + worthCurrency, Color.yellow, true);

        Destroy(this.hb.healthBar.gameObject);

        Destroy(gameObject);

        GameObject.Instantiate(this.deathParticleSystem, this.transform.position, Quaternion.identity);
    }

    //	void OnDestroy() {
    //		if(this.zombieState != ZombieState.Dead && this.hb.healthBar.gameObject != null) Destroy(this.hb.healthBar.gameObject);
    //	}
    //
    protected void OnCollisionStay2D(Collision2D other)
    {
        //damage the buildings/turrets in path
        if (other.transform.tag == "Turret" || other.transform.tag == "PlayerBase")
        {
            // todo: remove all "SendMessage" calls (they use reflection)
            other.gameObject.SendMessage("TakeDamage", attackDamage * Time.deltaTime);
        }
    }

    public void CatchFire()
    {
        this.CatchFire(4);
    }

    public void CatchFire(float dmg)
    {

        // getting hit by another fireball will always reset the damage to the function parameter
        this.fireDamage = dmg;

        // we only reset the flame damage timer if we weren't
        // already on fire.
        if (!this.onFire)
        {
            this.nextFlameDamageTime = Time.time;

            // start the flame particle effect
            psLarge.Play();
            psSmallLeft.Play();
            psSmallRight.Play();

            this.onFire = true;
        }
    }

    protected void GeneratePopUpNumber(string txt, Color txtCol, bool largeText)
    {
        GameObject pop = Instantiate(popNums) as GameObject;
        pop.transform.position = transform.position;
        pop.GetComponent<Text>().text = txt;
        pop.GetComponent<Text>().color = txtCol;
        if (largeText) pop.GetComponent<Text>().fontSize += 10;
    }

    protected float DamageTypeModifier(DamageType dmgType)
    {
        if (dmgType == DamageType.light)
        {
            if (zSize == ZombieSize.Small)
                return .5f;
            else if (zSize == ZombieSize.Large)
                return 1.5f;
        }
        else if (dmgType == DamageType.medium)
        {
            if (zSize == ZombieSize.Medium)
                return 1.5f;
            else if (zSize == ZombieSize.Boss)
                return .5f;
        }
        else if (dmgType == DamageType.heavy)
        {
            if (zSize == ZombieSize.Small)
                return 1.5f;
            else if (zSize == ZombieSize.Large)
                return .5f;
        }
        return 1f;
    }

	public void SetPath(ZombiePath path, int startNodeIndex) {
		this.path = path;
		this.currentNodeIndex = startNodeIndex;
	}
}
