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

    public override void Explode(Collider2D other)
    {
        Vector3 pos = other == null ? transform.position : other.transform.position;
        GameObject blast = Instantiate(this.bombBlast, pos, Quaternion.identity) as GameObject;

        BombBlast bb = blast.GetComponent<BombBlast>();
        if (bb)
        {
            bb.damage = this.damage;
        }

        Destroy(this.gameObject);
    }

}
