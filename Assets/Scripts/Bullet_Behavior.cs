﻿using UnityEngine;
using System.Collections;

public class Bullet_Behavior : MonoBehaviour {
	public float moveSpeed = 20f;
	public Vector3 startPos;
	public float range = 10f;

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
			Destroy (other.gameObject);
			Destroy (this.gameObject);
		}
	}

}
