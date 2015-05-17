using UnityEngine;
using System.Collections;

public class Spawn_Enemy : MonoBehaviour {
	public GameObject prefab;
	public bool spawnStopped = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(!spawnStopped) {
			Instantiate(prefab, this.transform.position, Quaternion.identity);
			spawnStopped = true;
		}
	}
}
