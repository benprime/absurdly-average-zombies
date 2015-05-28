using UnityEngine;
using System.Collections;

public class Object_Placement : MonoBehaviour {
	public GameObject obj;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonUp (0)) {
			Vector3 target = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			target.z = 0;
			Instantiate (obj, target, Quaternion.identity);
		}
	}

	public void SelectTurretType(GameObject turret) {
		obj = turret;
	}
}
