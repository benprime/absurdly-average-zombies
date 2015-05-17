using UnityEngine;
using System.Collections;

public class ZombiePathfinder : MonoBehaviour {
	public Transform lastNode, nextNode;
	public float moveSpeed = 2f;
	// Use this for initialization
	void Start () {
		lastNode = transform;
	}
	
	// Update is called once per frame
	void Update () {

	}

}
