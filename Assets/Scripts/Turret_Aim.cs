using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Turret_Aim : MonoBehaviour {
	public List<GameObject> zombiesInRange;
	public Transform target;
	public Turret_Fire shootBehavior;
	public float RotationSpeed;

	// Use this for initialization
	void Start () {
		shootBehavior = gameObject.GetComponent<Turret_Fire>();
	}
	
	// Update is called once per frame
	void Update () {
		if(zombiesInRange.Count > 0) {
			LookAtNearestEnemy();
			while(shootBehavior.firedShots.Remove (null));
			//shootBehavior.Fire();
		}
	}

	void LookAtNearestEnemy() {
		while(zombiesInRange.Remove (null));
		if(zombiesInRange.Count <= 0) return;
		if(!target) target = zombiesInRange[0].transform;
		Transform nearest = target;
		float distanceToNearest = Vector3.Distance (nearest.position, this.transform.position);
		foreach (GameObject enemy in zombiesInRange) {
			float tempDist = Vector3.Distance (enemy.transform.position, this.transform.position);
			if(tempDist < distanceToNearest) {
				nearest = enemy.transform;
				distanceToNearest = tempDist;
			}
		}

		float step = RotationSpeed * Time.deltaTime;
		Vector3 targetRotation = Vector3.Normalize (nearest.position - transform.position);
		transform.up = Vector3.RotateTowards (transform.up, targetRotation, step, 0.0f);

		if (transform.up == targetRotation) {
			shootBehavior.Fire();
		}
	}


}
