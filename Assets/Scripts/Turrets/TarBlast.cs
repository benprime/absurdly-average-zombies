using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TarBlast : MonoBehaviour
{

    private List<GameObject> zombies;

    // Use this for initialization
    void Start()
    {
        this.zombies = new List<GameObject>();
        transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnDestroy()
    {
        foreach (GameObject g in this.zombies)
        {
            // g will be null if the zombie has been destroyed
            // before the tar field has been destroyed
            if (g != null)
            {
                Zombie z = g.GetComponent<Zombie>();
                z.moveModifier = 1.0f;
            }
        }
        this.zombies.Clear();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "enemy")
        {
            Zombie z = other.gameObject.GetComponent<Zombie>();
            z.moveModifier = 0.5f;
            if (!this.zombies.Contains(other.gameObject))
            {
                this.zombies.Add(other.gameObject);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "enemy")
        {
            Zombie z = other.gameObject.GetComponent<Zombie>();
            z.moveModifier = 1f;
            this.zombies.Remove(other.gameObject);
        }
    }

    // This is required for situation where the zombies have exited one tar field
    // and walked directly into another one that was created before they exited the first.
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "enemy")
        {
            Zombie z = other.gameObject.GetComponent<Zombie>();
            z.moveModifier = 0.5f;
            if (!this.zombies.Contains(other.gameObject))
            {
                this.zombies.Add(other.gameObject);
            }
        }
    }


}
