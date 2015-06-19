using UnityEngine;
using System.Collections;

public class PlayerBase_Stats : MonoBehaviour {
	public float maxHitPoints = 100f;
	public float currentHitPoints = 100f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {		
		if (currentHitPoints <= 0) {
			Destroy(gameObject);
		}
	}
	
	void TakeDamage(float damage) {
		currentHitPoints -= damage;
		if(currentHitPoints < (maxHitPoints / 3)) {
			gameObject.GetComponent<SpriteRenderer>().color = Color.red;
		}
	}
}
