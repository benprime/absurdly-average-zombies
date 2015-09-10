using UnityEngine;
using System.Collections;

public class Rocket_Behavior : Bullet_Behavior {
	public GameObject bombBlast;

	[HideInInspector]
	public GameObject target;

	// Use this for initialization
	protected override void Awake () {
		base.Awake();
	}
	
	// Update is called once per frame
	protected override void Update () {
		if (this.target) {
			this.transform.up = target.transform.position - this.transform.position;
		}
		base.Update();
	}

	protected override void OnTriggerEnter2D (Collider2D other) {
		if(other.tag == "enemy") {
			GameObject blast = Instantiate(this.bombBlast, other.transform.position, Quaternion.identity) as GameObject;
			blast.GetComponent<BombBlast>().damage = this.damage;
			//other.SendMessage("TakeDamage", this.damage);
			Destroy (this.gameObject);
		}
	}

}
