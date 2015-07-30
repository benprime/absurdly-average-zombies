using UnityEngine;
using System.Collections;

public class EnemyKillZone : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D (Collider2D other) {
		if(other.CompareTag("enemy")){
			Destroy	(other.gameObject.GetComponent<UI_FloatingHealthBar>().healthBar.gameObject);
			Destroy (other.gameObject);
		}
	}
}
