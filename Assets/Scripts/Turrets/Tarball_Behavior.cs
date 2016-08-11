using UnityEngine;
using System.Collections;

public class Tarball_Behavior : Bullet_Behavior
{
    public GameObject tarBlast;

    [HideInInspector]
    public GameObject target;

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
        GameObject blast = Instantiate(this.tarBlast, pos, Quaternion.identity) as GameObject;
		Vector3 newScale = new Vector3 (damage, damage, 1f);
		//tar launcher 'damage' is actually tar splat size
		blast.transform.localScale = newScale;

        Destroy(this.gameObject);
    }

}
