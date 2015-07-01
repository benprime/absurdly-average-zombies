using UnityEngine;
using System.Collections;

public class Bullet_Bomb_Behavior : Bullet_Behavior {
	public GameObject bombBlast;

	// Use this for initialization
	protected override void Awake () {
		base.Awake();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}

	protected override void OnTriggerEnter2D (Collider2D other) {
		if(other.tag == "enemy") {
			Instantiate(this.bombBlast, this.transform.position, Quaternion.identity);
			//other.SendMessage("TakeDamage", this.damage);
			Destroy (this.gameObject);
		}
	}

}
