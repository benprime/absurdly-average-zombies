using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Turret_ZombieDetect : MonoBehaviour
{
    private Turret parentInfo;

    // Use this for initialization
    void Start()
    {
        parentInfo = GetComponentInParent<Turret>();
    }


    void OnTriggerEnter2D(Collider2D thing)
    {
        if (thing.tag == "enemy")
        {
            parentInfo.zombiesInRange.Add(thing.gameObject);
            //parentInfo.zombiesInRange.Sort (   //TODO: implement later to save on distance checks
        }
    }

    void OnTriggerExit2D(Collider2D thing)
    {
        if (thing.tag == "enemy")
        {
            parentInfo.zombiesInRange.Remove(thing.gameObject);
            //parentInfo.zombiesInRange.Add(thing.gameObject);
        }
    }

}
