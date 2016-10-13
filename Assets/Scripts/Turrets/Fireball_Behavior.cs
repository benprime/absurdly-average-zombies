using UnityEngine;

public class Fireball_Behavior : Bullet_Behavior
{
    public float impactDmgMod = 0.1f;

    // Use this for initialization
    protected override void Awake()
    {
        startPos = new Vector3();
        startPos = transform.position;

        float z = (Mathf.PingPong(Time.time * 100, 30) - 15);
        transform.Rotate(0.0f, 0.0f, z);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "enemy")
        {
            Zombie z = other.gameObject.GetComponent<Zombie>();

            //fireballs do impact damage at a fraction of their dps damage
            z.TakeDamage(this.damage * this.impactDmgMod, Zombie.DamageType.Fire);

            // Fireballs set zombies on fire
            z.CatchFire(this.damage);
            Destroy(this.gameObject, 0.12f);
        }
    }

}
