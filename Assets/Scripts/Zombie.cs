using UnityEngine;
using System.Collections;

public class Zombie : MonoBehaviour {
	public float moveSpeed = 2f;
	public float turnSpeed = 4f;
	public float walkSwayModifier = 30;

	public Vector3 direction;


	// Use this for initialization
	protected virtual void Start () {
		// set travel direction
		//this.direction = new Vector3(Random.Range (-1f, 1f), Random.Range (-1f, 1f), 0);
		this.direction = Vector3.down;

		// without this new zombies kind of just stand around
		this.direction = this.direction.normalized * 1;

		// set initial rotation of zombie
		this.transform.up = this.direction;
	}
	
	// Update is called once per frame
	protected virtual void Update () {

		// reset the transform to the direction, so that when we apply the
		// sway code, it doesn't become cumulative and do some wonky stuff.
		// Probably not the best way to handle this, but it's simple for now.
		// We can re-address this later.
		this.transform.up = this.direction;

		// update zombie position, moving the direction
		this.transform.position += this.direction * this.moveSpeed * Time.deltaTime;

		// z is -10 to 10 sway (in degrees)
		float z = Mathf.PingPong(Time.time * this.walkSwayModifier, 20) - 10;

		// apply the "sway" rotate
		transform.Rotate (0.0f, 0.0f, z);
	}

	void OnCollisionEnter2D(Collision2D other) {
		// set to move opposite direction
		this.direction = -this.direction;
	}
}
