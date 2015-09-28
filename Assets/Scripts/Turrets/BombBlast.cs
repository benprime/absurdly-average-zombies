using UnityEngine;
using System.Collections;

public class BombBlast : MonoBehaviour
{
    public int damage;

    // Use this for initialization
    void Start()
    {
        transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
    }

    // Update is called once per frame
    void Update()
    {
    }

    void onExplosionEnd()
    {
        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "enemy")
        {
            Zombie z = other.GetComponent<Zombie>();
            z.TakeDamage(this.damage);
        }
    }

}
