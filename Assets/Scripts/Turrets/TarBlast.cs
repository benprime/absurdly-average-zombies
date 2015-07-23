using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TarBlast : MonoBehaviour {

	private List<GameObject> zombies;

	// Use this for initialization
	void Start () {
		this.zombies = new List<GameObject> ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnDestroy()
	{
		foreach (GameObject g in this.zombies) {
			Zombie z = g.GetComponent<Zombie>();
			z.moveModifier = 1.0f;
		}
		this.zombies.Clear ();
		Destroy (this.gameObject);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.tag == "enemy") {
			Zombie z = other.gameObject.GetComponent<Zombie>();
			z.moveModifier = 0.5f;
			this.zombies.Add(other.gameObject);
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if(other.tag == "enemy") {
			Zombie z = other.gameObject.GetComponent<Zombie>();
			z.moveModifier = 1f;
			this.zombies.Remove(other.gameObject);
		}
	}
}
