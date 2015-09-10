using UnityEngine;
using System.Collections;

public class Fireball_Behavior : Bullet_Behavior {

	// Use this for initialization
	protected override void Awake () {

		startPos = new Vector3();
		startPos = transform.position;

		float z = (Mathf.PingPong (Time.time * 100, 30) - 15);
		transform.Rotate (0.0f, 0.0f, z);
		//transform.up += new Vector3 (0, 0, z);
	}
	
	// Update is called once per frame
	protected override void Update () {
		transform.Translate (Vector2.up * moveSpeed * Time.deltaTime);
		if(Vector3.Distance (transform.position, startPos) > range) {
			Destroy (this.gameObject);
		}
	}

	protected override void OnTriggerEnter2D (Collider2D other) {
		if(other.tag == "enemy") {
			Zombie z = other.gameObject.GetComponent<Zombie>();

			// Fireballs now do no base damage... only damage over time
			//z.TakeDamage(this.damage);
			z.CatchFire(this.damage);

			//other.SendMessage("TakeDamage", this.damage);
			//other.SendMessage("CatchFire");
			Destroy (this.gameObject);
		}
	}

}
