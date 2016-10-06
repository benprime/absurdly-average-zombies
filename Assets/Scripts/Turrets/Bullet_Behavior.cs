using UnityEngine;
using System.Collections;

public class Bullet_Behavior : MonoBehaviour
{
    public float moveSpeed;
    public Vector3 startPos;
    protected float damage;

    [HideInInspector]
    public Turret turret;

    // Use this for initialization
    protected virtual void Awake()
    {
        startPos = new Vector3();
        startPos = transform.position;
    }

    // The bullet damage is set at the time of firing when this bullet is
    // instantiated.  This keeps the damage/upgrade data on the turret.
    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, startPos) > turret.range)
        {
            this.Explode(null);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "enemy")
        {
            Zombie z = other.GetComponent<Zombie>();
            z.TakeDamage(this.damage, Zombie.DamageType.Bullet);
            this.Explode(null);
        }
    }

    public virtual void Explode(Collider2D other)
    {
        Destroy(this.gameObject);
    }

}
