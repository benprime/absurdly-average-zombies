using UnityEngine;
using System.Collections;

public class Bullet_Fireball_Behavior : MonoBehaviour {
	public float moveSpeed = 20f;
	public Vector3 startPos;
	public float range = 10f;
	public float damage = 5f;

	// Use this for initialization
	protected virtual void Awake () {
		startPos = new Vector3();
		startPos = transform.position;

		float z = (Mathf.PingPong (Time.time * 100, 30) - 15);
		transform.Rotate (0.0f, 0.0f, z);
		//transform.up += new Vector3 (0, 0, z);
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		transform.Translate (Vector2.up * moveSpeed * Time.deltaTime);
		if(Vector3.Distance (transform.position, startPos) > range) {
			Destroy (this.gameObject);
		}
	}

	protected virtual void OnTriggerEnter2D (Collider2D other) {
		if(other.tag == "enemy") {
			other.SendMessage("TakeDamage", this.damage);
			Destroy (this.gameObject);
		}
	}

}
