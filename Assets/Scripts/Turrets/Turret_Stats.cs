using UnityEngine;
using System.Collections;

public class Turret_Stats : MonoBehaviour {
	public int costCurrency = 10;
	public float maxHitPoints = 50f;
	public float currentHitPoints = 50f;
	public int rangeLevel = 0, damageLevel = 0, speedLevel = 0;
	public float rangeIncrease = .1f, damageIncrease = 1f, speedIncrease = .1f;

	//TODO: move all turret stats from other scripts to this script


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
