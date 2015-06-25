using UnityEngine;
using System.Collections;

public class BombBlast : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("DAMAGE!");
		other.SendMessage ("TakeDamage", 5000);
	}

}
