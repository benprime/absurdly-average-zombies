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
        else if (thing.tag == "Projectile")
        {
            // project tile has left the range of the turret so explode/remove object
            Bullet_Behavior bb = thing.gameObject.GetComponent<Bullet_Behavior>();
            Turret t = this.gameObject.transform.parent.gameObject.GetComponent<Turret>();
            if (bb.turret == t)
            {
                bb.Explode(null);
            }
        }
    }

}
