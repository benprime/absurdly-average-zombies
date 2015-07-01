using UnityEngine;
using System.Collections;

public class BombBlast : MonoBehaviour {
	public float damage = 25f;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerStay2D(Collider2D other) {
		if(other.tag == "enemy") {
			//Debug.Log ("DAMAGE!");
			other.SendMessage ("TakeDamage", damage);
			Destroy (this.gameObject); //TODO: possibly make blast expand over time
		}
	}

}
