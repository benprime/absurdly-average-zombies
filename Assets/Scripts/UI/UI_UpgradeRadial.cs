using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_UpgradeRadial : MonoBehaviour {
	public BuildZone connectedZone;
	private Turret currentWeaponStats;
	public int baseUpgradeCost = 5;
	public int maxUpgradeLevels = 3;
	public Sprite[] rankSprites;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(connectedZone) {
			currentWeaponStats = connectedZone.currentWeapon.GetComponent<Turret> ();

			//much hackery going on here due to lack of sleep TODO: optimize!!!! (shouldn't be in the update loop)
			Text sellCostText = transform.FindChild("W").GetComponentInChildren<Text>();
			sellCostText.text = "$" +((currentWeaponStats.costCurrency) / 2);

			//change upgrade image to show next available upgrade
			transform.FindChild("N").GetComponentInChildren<Text>().text = "$" + ((currentWeaponStats.damageLevel + 1) * baseUpgradeCost);// + currentWeaponStats.baseCost;
			transform.FindChild("E").GetComponentInChildren<Text>().text = "$" + ((currentWeaponStats.rangeLevel + 1) * baseUpgradeCost);
			transform.FindChild("S").GetComponentInChildren<Text>().text = "$" + ((currentWeaponStats.speedLevel + 1) * baseUpgradeCost);
			transform.FindChild("N").GetComponentInChildren<SpriteRenderer>().sprite = rankSprites[currentWeaponStats.damageLevel];
			transform.FindChild("E").GetComponentInChildren<SpriteRenderer>().sprite = rankSprites[currentWeaponStats.rangeLevel];
			transform.FindChild("S").GetComponentInChildren<SpriteRenderer>().sprite = rankSprites[currentWeaponStats.speedLevel];
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
		if(connectedZone && currentWeaponStats.rangeLevel < maxUpgradeLevels - 1) {
			//charge player cost of upgrade
			int cost = baseUpgradeCost * (currentWeaponStats.rangeLevel + 1);  //upgrade cost = baseUpgradeCost * next upgrade level
			if(GameManager.instance.GetPlayerTotalCurrency() < cost) return;
			GameManager.instance.PlayerCurrencyTransaction (-cost);
			//upgrade weapon
			currentWeaponStats.rangeLevel++;
			float temp = currentWeaponStats.transform.FindChild ("DetectionZone").GetComponent<CircleCollider2D>().radius;
			currentWeaponStats.transform.FindChild ("DetectionZone").GetComponent<CircleCollider2D>().radius = temp * (1 + ((currentWeaponStats.rangeLevel + 1) * currentWeaponStats.rangeIncrease));
			currentWeaponStats.costCurrency += cost;
			transform.FindChild("E").GetComponentInChildren<Text>().text = "$" + (baseUpgradeCost * (currentWeaponStats.rangeLevel + 1));
			transform.FindChild("E").GetComponentInChildren<SpriteRenderer>().sprite = rankSprites[currentWeaponStats.rangeLevel];
		}
	}
	
	public void UpgradeDamage() {
		if(connectedZone && currentWeaponStats.damageLevel < maxUpgradeLevels - 1) {
			//charge player cost of upgrade
			int cost = baseUpgradeCost * (currentWeaponStats.damageLevel + 1);  //upgrade cost = baseUpgradeCost * next upgrade level
			if(GameManager.instance.GetPlayerTotalCurrency() < cost) return;
			GameManager.instance.PlayerCurrencyTransaction (-cost);
			//upgrade weapon
			currentWeaponStats.damageLevel++;
			currentWeaponStats.damage = currentWeaponStats.damage * (1 + (int)((currentWeaponStats.damageLevel + 1) * currentWeaponStats.damageIncrease));
			currentWeaponStats.costCurrency += cost;
			transform.FindChild("N").GetComponentInChildren<Text>().text = "$" + (baseUpgradeCost * (currentWeaponStats.damageLevel + 1));
			transform.FindChild("N").GetComponentInChildren<SpriteRenderer>().sprite = rankSprites[currentWeaponStats.damageLevel];
		}
	}
	
	public void UpgradeSpeed() {
		if(connectedZone && currentWeaponStats.speedLevel < maxUpgradeLevels - 1) {
			//charge player cost of upgrade
			int cost = baseUpgradeCost * (currentWeaponStats.speedLevel + 1);  //upgrade cost = baseUpgradeCost * next upgrade level
			if(GameManager.instance.GetPlayerTotalCurrency() < cost) return;
			GameManager.instance.PlayerCurrencyTransaction (-cost);
			//upgrade weapon
			currentWeaponStats.speedLevel++;
			currentWeaponStats.rotationSpeed = currentWeaponStats.rotationSpeed * (1 + (int)((currentWeaponStats.speedLevel + 1) * currentWeaponStats.speedIncrease));
			currentWeaponStats.costCurrency += cost;
			transform.FindChild("S").GetComponentInChildren<Text>().text = "$" + (baseUpgradeCost * (currentWeaponStats.speedLevel + 1));
			transform.FindChild("S").GetComponentInChildren<SpriteRenderer>().sprite = rankSprites[currentWeaponStats.speedLevel];
		}
	}
}
