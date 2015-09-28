using UnityEngine;
using System.Collections;

public class BombBlast : MonoBehaviour
{
    public int damage;

    // Use this for initialization
    void Start()
    {
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
            //Debug.Log ("DAMAGE!");
            Zombie z = other.GetComponent<Zombie>();
            z.TakeDamage(this.damage);
            //other.SendMessage ("TakeDamage", damage);
            //Destroy (this.gameObject); //TODO: possibly make blast expand over time
        }
    }

}
