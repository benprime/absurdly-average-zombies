using UnityEngine;
using System.Collections;

public class Rocket_Behavior : Bullet_Behavior
{
    public GameObject bombBlast;

    [HideInInspector]
    public GameObject target;

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (this.target)
        {
            this.transform.up = target.transform.position - this.transform.position;
        }
        base.Update();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "enemy")
        {
            this.Explode(other);
        }
    }

    public override void Explode(Collider2D other)
    {
        Vector3 pos = other == null ? transform.position : other.transform.position;
        GameObject blast = Instantiate(this.bombBlast, pos, Quaternion.identity) as GameObject;

        // this script is used for tarblasts also, which don't have damage... little hacky, but whatevs
        BombBlast bb = blast.GetComponent<BombBlast>();
        if (bb)
        {
            bb.damage = this.damage;
        }

        Destroy(this.gameObject);
    }

}
