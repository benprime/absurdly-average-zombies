using UnityEngine;

public class Fireball_Behavior : Bullet_Behavior
{

    // Use this for initialization
    protected override void Awake()
    {
        startPos = new Vector3();
        startPos = transform.position;

        float z = (Mathf.PingPong(Time.time * 100, 30) - 15);
        transform.Rotate(0.0f, 0.0f, z);
    }

    // Update is called once per frame
    protected override void Update()
    {
        transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, startPos) > range)
        {
            Destroy(this.gameObject, 1);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "enemy")
        {
            Zombie z = other.gameObject.GetComponent<Zombie>();

            // Fireballs now do no base damage... only damage over time
            z.CatchFire(this.damage);
            Destroy(this.gameObject, 0.12f);
        }
    }

}
