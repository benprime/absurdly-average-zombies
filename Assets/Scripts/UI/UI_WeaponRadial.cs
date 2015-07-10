using UnityEngine;
using System.Collections;

public class UI_WeaponRadial : MonoBehaviour {
	public BuildZone connectedZone;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void BuildSelectedObject(GameObject obj) {		
		//GameObject objToPlace = GameManager.instance.selectedObjToBuild;
		//int cost = objToPlace.GetComponent<Turret_Stats> ().costCurrency;
		//if (GameManager.instance.GetPlayerTotalCurrency () >= cost) {
		//	GameManager.instance.PlayerCurrencyTransaction (-cost);
		//	Instantiate (objToPlace, transform.position, Quaternion.identity);
		//	//Destroy (hitZone);
		//}

		int cost = obj.GetComponent<Turret_Stats> ().costCurrency;
		if (GameManager.instance.GetPlayerTotalCurrency () >= cost) {
			GameManager.instance.PlayerCurrencyTransaction (-cost);
			Instantiate (obj, transform.position, Quaternion.identity);
			if(connectedZone) connectedZone.currentState = BuildZone.ZONE_STATE.BUILT_ON;
			Destroy (gameObject);
		}
	}
}
