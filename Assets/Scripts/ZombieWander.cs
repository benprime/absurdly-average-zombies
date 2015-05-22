using UnityEngine;
using System.Collections;

public class ZombieWander : MonoBehaviour {
	public float moveSpeed = 2f;
	public float turnSpeed = 4f;

	private Vector3 targetDirection;

	// Use this for initialization
	void Start () {
		Vector3 direction = new Vector3(Random.Range (-1f, 1f), Random.Range (-1f, 1f), 0);
		transform.up = direction.normalized * 1;
		targetDirection = transform.up;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += transform.up*moveSpeed * Time.deltaTime;
		//transform.up = Vector3.RotateTowards (transform.up, targetDirection, turnSpeed * Time.deltaTime, 0);  //TODO: convert to Vector2
	}

	void OnCollisionEnter2D(Collision2D other) {
		targetDirection = -transform.forward;

		float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}
}
