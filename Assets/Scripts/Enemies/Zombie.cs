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
    protected int fireTimer = 5;
    public int fireDuration = 5;

    public ParticleSystem hitParticleSystem;
    public ParticleSystem deathParticleSystem;

    // fire particle systems
    protected ParticleSystem psLarge;
    protected ParticleSystem psSmallLeft;
    protected ParticleSystem psSmallRight;

    public bool onFire = false;
    public ZombieState zombieState;
    private Vector3 direction;
    protected GameManager gm;

    public ZombiePath path = null;
    public float targetCloseness = .5f;

    public GameObject popNums;

    private int _currentNodeIndex;
    protected int currentNodeIndex
    {
        get
        {
            return _currentNodeIndex;
        }
        set
        {
            _currentNodeIndex = value;
            direction = currentNode.transform.position - transform.position;
        }
    }

    public GameObject currentNode
    {
        get { return path.nodes[currentNodeIndex]; }
    }

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
        if (currentNode)
        {
            this.direction = currentNode.transform.position - this.transform.position;
        }

        // save reference to particle systems for later use
        this.psLarge = this.transform.FindChild("ParticleSystemFireLarge").GetComponent<ParticleSystem>();
        this.psSmallLeft = this.transform.FindChild("ParticleSystemFireSmallLeft").GetComponent<ParticleSystem>();
        this.psSmallRight = this.transform.FindChild("ParticleSystemFireSmallRight").GetComponent<ParticleSystem>();

        hitPoints = maxHitPoints;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (this.zombieState == ZombieState.Dead)
        {
            return;
        }

        CheckAndUpdateFireDamage();

        if (this.hitPoints <= 0)
        {
            this.Die();
            return;
        }

        PathMovement();
    }

    private void PathMovement()
    {
        // check if there are paths to process
        if (path.nodes.Count <= 0)
            return;

        // update physical location of zombie
        transform.position = Vector2.MoveTowards(transform.position, currentNode.transform.position,
            moveSpeed * Time.deltaTime * moveModifier);

        // rotate zombie toward direction of travel
        if (transform.up != direction)
        {
            var newDir = Vector3.RotateTowards(transform.up, direction, turnSpeed * Time.deltaTime * moveModifier, 0.0f);
            transform.up = newDir;
        }

        // check for collision with path collider
        if (Vector2.Distance(transform.position, currentNode.transform.position) < targetCloseness)
        {
            // if we're not on the last path collider, move on
            if (currentNodeIndex < path.nodes.Count - 1)
            {
                currentNodeIndex++;
            }
        }
    }

    private void CheckAndUpdateFireDamage()
    {
        if (this.onFire)
        {
            if (Time.time > nextFlameDamageTime)
            {
                nextFlameDamageTime += this.flameDamageInterval;
                this.TakeDamage(this.fireDamage, DamageType.Fire);
                this.fireTimer--;
                if (this.fireTimer <= 0)
                {
                    psLarge.Stop();
                    psSmallLeft.Stop();
                    psSmallRight.Stop();

                    this.onFire = false;
                }
            }
        }
    }

    public enum DamageType { Fire, Bullet, Rocket }
    public void TakeDamage(float amount, DamageType dmgType)
    {
        float finalDamage = amount * DamageTypeModifier(dmgType);
        this.hitPoints -= finalDamage;

        if (this.hb.healthBar.rectTransform)
        {
            this.hb.healthBar.rectTransform.localScale = new Vector3(Mathf.Max(0, (hitPoints / maxHitPoints)), 1, 1); //TODO: Make healthbar scale from left pivot point
        }

        if (hitPoints <= maxHitPoints / 3)
        {
            hb.healthBar.color = Color.red;
        }
        else if (hitPoints <= 2 * (maxHitPoints / 3))
        {
            hb.healthBar.color = Color.yellow;
        }

        GameObject.Instantiate(this.hitParticleSystem, this.transform.position, Quaternion.identity);
    }

    protected virtual void Die()
    {
        this.zombieState = ZombieState.Dead;
        CircleCollider2D c = GetComponent<CircleCollider2D>();
        c.enabled = false;

        GameManager.Instance.PlayerCurrencyTransaction(worthCurrency);
        this.GeneratePopUpNumber("$" + worthCurrency, Color.yellow, true);

        Destroy(this.hb.healthBar.gameObject);

        Destroy(gameObject);

        GameObject.Instantiate(this.deathParticleSystem, this.transform.position, Quaternion.identity);
    }

    protected void OnCollisionStay2D(Collision2D other)
    {
        // allow zombies to damage the player base
        if (other.transform.tag == "PlayerBase")
        {
            // todo: why are we multiplying it by deltaTime?
            PlayerBase_Stats.Instance.TakeDamage(attackDamage * Time.deltaTime);
        }
    }

    public void CatchFire()
    {
        this.CatchFire(4);
    }

    public void CatchFire(float dmg)
    {
        // fire dps duration timer will be reset everytime zombie is hit by a fireball
        this.fireTimer = fireDuration;

        // getting hit by another fireball will only reset damage if the fireball stats are higher than what is already on the zombie
        if (dmg > this.fireDamage) this.fireDamage = dmg;

        // we only set fire to zombie if not already on fire
        if (!this.onFire)
        {
            this.nextFlameDamageTime = Time.time + this.flameDamageInterval;

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
        if (dmgType == DamageType.Fire)
        {
            if (zSize == ZombieSize.Small)
                return .5f;
            else if (zSize == ZombieSize.Large)
                return 1.5f;
        }
        else if (dmgType == DamageType.Bullet)
        {
            if (zSize == ZombieSize.Medium)
                return 1.5f;
            else if (zSize == ZombieSize.Boss)
                return .5f;
        }
        else if (dmgType == DamageType.Rocket)
        {
            if (zSize == ZombieSize.Small)
                return 1.5f;
            else if (zSize == ZombieSize.Large)
                return .5f;
        }
        return 1f;
    }


    public void SetPath(ZombiePath path, int startNodeIndex)
    {
        this.path = path;
        this.currentNodeIndex = startNodeIndex;
    }
}
