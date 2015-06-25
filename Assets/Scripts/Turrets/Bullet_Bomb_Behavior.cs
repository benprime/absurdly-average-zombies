using UnityEngine;
using System.Collections;

public class Bullet_Bomb_Behavior : MonoBehaviour {
	public float moveSpeed = 20f;
	public Vector3 startPos;
	public float range = 10f;
	public float damage = 5f;
	public GameObject bombBlast;

	// Use this for initialization
	void Awake () {
		startPos = new Vector3();
		startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (Vector2.up * moveSpeed * Time.deltaTime);
		if(Vector3.Distance (transform.position, startPos) > range) {
			Destroy (this.gameObject);
		}
	}

	void OnTriggerEnter2D (Collider2D other) {
		if(other.tag == "enemy") {
			Instantiate(this.bombBlast, this.transform.position, Quaternion.identity);
			//other.SendMessage("TakeDamage", this.damage);
			//Destroy (other.gameObject);
			Destroy (this.gameObject);
		}
	}

}
