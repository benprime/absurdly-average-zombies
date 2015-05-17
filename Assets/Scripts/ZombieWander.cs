using UnityEngine;
using System.Collections;

public class ZombieWander : MonoBehaviour {
	public Vector3 direction;
	public float moveSpeed = 2f;
	// Use this for initialization
	void Start () {
		direction = new Vector3(Random.Range (.5f, 1f) * moveSpeed * Time.deltaTime, Random.Range (.5f, 1f) * moveSpeed * Time.deltaTime, 0);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(direction);
	}

	void OnCollisionEnter2D(Collision2D other) {
		//transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
		transform.right = transform.right * -1;
		transform.up = new Vector3 (Random.Range (-1.5f, 1.5f) * moveSpeed, Random.Range (-1.5f, 1.5f) * moveSpeed);
		//direction = transform.right * -1;
	}
}
