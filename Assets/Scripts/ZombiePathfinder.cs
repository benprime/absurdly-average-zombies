using UnityEngine;
using System.Collections;

public class ZombiePathfinder : MonoBehaviour {
	//public Transform lastNode, nextNode;
	public float moveSpeed = 2f;
	public GameObject enemyBase;
	public float turnSpeed = 4f;

	// Use this for initialization
	void Start () {
		//lastNode = transform;
		enemyBase = GameObject.FindGameObjectWithTag("PlayerBase");
	}
	
	// Update is called once per frame
	void Update () {	
		Vector3 targetRotation = Vector3.Normalize (enemyBase.transform.position - transform.position);	
		transform.up = Vector3.RotateTowards (transform.up, targetRotation, turnSpeed * Time.deltaTime, 0);  //TODO: convert to Vector2
		transform.position = Vector2.MoveTowards(transform.position, enemyBase.transform.position, moveSpeed * Time.deltaTime);
	}

}
