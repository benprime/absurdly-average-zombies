using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_UpgradeRadial : MonoBehaviour {
	public BuildZone connectedZone;
	public Text sellCostText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(connectedZone && sellCostText.text == "") sellCostText.text = "$" +((connectedZone.currentWeapon.GetComponent<Turret_Stats> ().costCurrency) / 2);
	}
	
	public void SellWeapon() {		
		if(connectedZone) {
			int worth = (connectedZone.currentWeapon.GetComponent<Turret_Stats> ().costCurrency) / 2;  //sell for half of purchase cost
			GameManager.instance.PlayerCurrencyTransaction (worth);
			Destroy (connectedZone.currentWeapon);
			connectedZone.currentState = BuildZone.ZONE_STATE.EMPTY;
			Destroy (gameObject);
		}
	}
}
