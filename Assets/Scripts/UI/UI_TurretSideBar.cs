using UnityEngine;
using System.Collections;

public class UI_TurretSideBar : MonoBehaviour {
	
	private GameManager gm;

	// Use this for initialization
	void Start () {
		gm = FindObjectOfType<GameManager>();	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void SelectObjectToPlace(GameObject choice) {
		gm.GetComponent<Object_Placement>().obj = choice;
	}
}
