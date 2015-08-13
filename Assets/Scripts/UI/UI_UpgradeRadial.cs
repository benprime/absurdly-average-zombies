using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_UpgradeRadial : MonoBehaviour {
	public BuildZone connectedZone;
	private Turret currentWeaponStats;
	public int baseUpgradeCost = 5;
	public int maxUpgradeLevels = 3;

	// Use this for initialization
	void Start () {
		transform.FindChild("E").GetComponentInChildren<Text>().text = "$" + baseUpgradeCost;
	}
	
	// Update is called once per frame
	void Update () {
		if(connectedZone) {
			currentWeaponStats = connectedZone.currentWeapon.GetComponent<Turret> ();

			//much hackery going on here due to lack of sleep TODO: optimize!!!! (shouldn't be in the update loop)
			Text sellCostText = transform.FindChild("W").GetComponentInChildren<Text>();
			if(sellCostText.text == "") sellCostText.text = "$" +((currentWeaponStats.costCurrency) / 2);
		}
	}
	
	public void SellWeapon() {		
		if(connectedZone) {
			int worth = (currentWeaponStats.costCurrency) / 2;  //sell for half of purchase cost
			GameManager.instance.PlayerCurrencyTransaction (worth);
			Destroy (connectedZone.currentWeapon);
			connectedZone.currentState = BuildZone.ZONE_STATE.EMPTY;
			Destroy (gameObject);
		}
	}

	public void UpgradeRange() {
		if(connectedZone && currentWeaponStats.rangeLevel <= maxUpgradeLevels) {
			//charge player cost of upgrade
			int cost = baseUpgradeCost * (currentWeaponStats.rangeLevel + 1);  //upgrade cost = baseUpgradeCost * next upgrade level
			if(GameManager.instance.GetPlayerTotalCurrency() < cost) return;
			GameManager.instance.PlayerCurrencyTransaction (-cost);
			//upgrade weapon
			currentWeaponStats.rangeLevel++;
			float temp = currentWeaponStats.transform.FindChild ("DetectionZone").GetComponent<CircleCollider2D>().radius;
			currentWeaponStats.transform.FindChild ("DetectionZone").GetComponent<CircleCollider2D>().radius = temp * (1 + (currentWeaponStats.rangeLevel * currentWeaponStats.rangeIncrease));
			currentWeaponStats.costCurrency += cost;
			transform.FindChild("E").GetComponentInChildren<Text>().text = "$" + (baseUpgradeCost * (currentWeaponStats.rangeLevel + 1));
		}
	}
	
	public void UpgradeDamage() {
		if(connectedZone && currentWeaponStats.damageLevel <= maxUpgradeLevels) {
			//charge player cost of upgrade
			int cost = baseUpgradeCost * (currentWeaponStats.damageLevel + 1);  //upgrade cost = baseUpgradeCost * next upgrade level
			if(GameManager.instance.GetPlayerTotalCurrency() < cost) return;
			GameManager.instance.PlayerCurrencyTransaction (-cost);
			//upgrade weapon
			currentWeaponStats.damageLevel++;
			//float temp = currentWeaponStats.transform.FindChild ("DetectionZone").GetComponent<CircleCollider2D>().radius;
			//currentWeaponStats.transform.FindChild ("DetectionZone").GetComponent<CircleCollider2D>().radius = temp * (1 + (currentWeaponStats.rangeLevel * currentWeaponStats.rangeIncrease));
			currentWeaponStats.costCurrency += cost;
			transform.FindChild("N").GetComponentInChildren<Text>().text = "$" + (baseUpgradeCost * (currentWeaponStats.damageLevel + 1));
		}
	}
	
	public void UpgradeSpeed() {
		if(connectedZone && currentWeaponStats.speedLevel <= maxUpgradeLevels) {
			//charge player cost of upgrade
			int cost = baseUpgradeCost * (currentWeaponStats.speedLevel + 1);  //upgrade cost = baseUpgradeCost * next upgrade level
			if(GameManager.instance.GetPlayerTotalCurrency() < cost) return;
			GameManager.instance.PlayerCurrencyTransaction (-cost);
			//upgrade weapon
			currentWeaponStats.speedLevel++;
			//float tempSD = currentWeaponStats.transform.GetComponent<Turret_Fire>().shotDelay;
			//float tempRS = currentWeaponStats.transform.GetComponent<Turret_Aim>().RotationSpeed;
			//currentWeaponStats.transform.FindChild ("DetectionZone").GetComponent<CircleCollider2D>().radius = temp * (1 + (currentWeaponStats.rangeLevel * currentWeaponStats.rangeIncrease));
			currentWeaponStats.costCurrency += cost;
			transform.FindChild("S").GetComponentInChildren<Text>().text = "$" + (baseUpgradeCost * (currentWeaponStats.speedLevel + 1));
		}
	}
}
