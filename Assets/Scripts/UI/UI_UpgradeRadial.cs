using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_UpgradeRadial : MonoBehaviour {
	public BuildZone connectedZone;
	private Turret currentWeaponStats;
	public int baseUpgradeCost = 5;
	public int maxUpgradeLevels = 3;
	public Sprite[] rankSprites;
	private float baseRotSpd = -1f, baseShotDelay = -1f, baseRange = -1f;

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
			if(currentWeaponStats.damageLevel < maxUpgradeLevels) {
				transform.FindChild("N").GetComponentInChildren<Text>().text = "$" + ((currentWeaponStats.damageLevel + 1) * baseUpgradeCost);// + currentWeaponStats.baseCost;
				transform.FindChild("N").GetComponentInChildren<SpriteRenderer>().sprite = rankSprites[currentWeaponStats.damageLevel];
				}
			if(currentWeaponStats.rangeLevel < maxUpgradeLevels) {
				transform.FindChild("E").GetComponentInChildren<Text>().text = "$" + ((currentWeaponStats.rangeLevel + 1) * baseUpgradeCost);
				transform.FindChild("E").GetComponentInChildren<SpriteRenderer>().sprite = rankSprites[currentWeaponStats.rangeLevel];
			}			
			if(currentWeaponStats.speedLevel < maxUpgradeLevels) {
				transform.FindChild("S").GetComponentInChildren<Text>().text = "$" + ((currentWeaponStats.speedLevel + 1) * baseUpgradeCost);
				transform.FindChild("S").GetComponentInChildren<SpriteRenderer>().sprite = rankSprites[currentWeaponStats.speedLevel];
			}
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
		if(connectedZone && currentWeaponStats.rangeLevel < maxUpgradeLevels) {
			if(baseRange < 0) baseRange = currentWeaponStats.transform.FindChild ("DetectionZone").GetComponent<CircleCollider2D>().radius;
			//charge player cost of upgrade
			int cost = baseUpgradeCost * (currentWeaponStats.rangeLevel + 1);  //upgrade cost = baseUpgradeCost * next upgrade level
			if(GameManager.instance.GetPlayerTotalCurrency() <= cost) return;
			GameManager.instance.PlayerCurrencyTransaction (-cost);
			//upgrade weapon
			currentWeaponStats.rangeLevel++;
			currentWeaponStats.transform.FindChild ("DetectionZone").GetComponent<CircleCollider2D>().radius = baseRange * (1 + ((currentWeaponStats.rangeLevel) * currentWeaponStats.rangeIncrease));
			//increase weapon worth
			currentWeaponStats.costCurrency += cost;
			if(currentWeaponStats.rangeLevel == maxUpgradeLevels) {
				transform.FindChild("E").GetComponentInChildren<Text>().text = "MAX";
				transform.FindChild("E").GetComponentInChildren<Button>().interactable = false;
			}
			else {
				transform.FindChild("E").GetComponentInChildren<Text>().text = "$" + (baseUpgradeCost * (currentWeaponStats.rangeLevel + 1));
				transform.FindChild("E").GetComponentInChildren<SpriteRenderer>().sprite = rankSprites[currentWeaponStats.rangeLevel];
			}
		}
	}
	
	public void UpgradeDamage() {
		if(connectedZone && currentWeaponStats.damageLevel < maxUpgradeLevels) {
			//charge player cost of upgrade
			int cost = baseUpgradeCost * (currentWeaponStats.damageLevel + 1);  //upgrade cost = baseUpgradeCost * next upgrade level
			if(GameManager.instance.GetPlayerTotalCurrency() <= cost) return;
			GameManager.instance.PlayerCurrencyTransaction (-cost);
			//upgrade weapon
			currentWeaponStats.damageLevel++;
			currentWeaponStats.damage = currentWeaponStats.baseDamage * (1 + (int)((currentWeaponStats.damageLevel) * currentWeaponStats.damageIncrease));
			//increase weapon worth
			currentWeaponStats.costCurrency += cost;
			if(currentWeaponStats.damageLevel == maxUpgradeLevels) {
				transform.FindChild("N").GetComponentInChildren<Text>().text = "MAX";
				transform.FindChild("N").GetComponentInChildren<Button>().interactable = false;
			}
			else {
				transform.FindChild("N").GetComponentInChildren<Text>().text = "$" + (baseUpgradeCost * (currentWeaponStats.damageLevel + 1));
				transform.FindChild("N").GetComponentInChildren<SpriteRenderer>().sprite = rankSprites[currentWeaponStats.damageLevel];
			}
		}
	}
	
	public void UpgradeSpeed() {
		if(connectedZone && currentWeaponStats.speedLevel < maxUpgradeLevels) {
			if(baseRotSpd < 0 || baseShotDelay < 0) {
				baseRotSpd = currentWeaponStats.rotationSpeed;
				baseShotDelay = currentWeaponStats.shotDelay;
			}
			//charge player cost of upgrade
			int cost = baseUpgradeCost * (currentWeaponStats.speedLevel + 1);  //upgrade cost = baseUpgradeCost * next upgrade level
			if(GameManager.instance.GetPlayerTotalCurrency() <= cost) return;
			GameManager.instance.PlayerCurrencyTransaction (-cost);
			//upgrade weapon
			currentWeaponStats.speedLevel++;
			currentWeaponStats.rotationSpeed = baseRotSpd * (1 + ((currentWeaponStats.speedLevel) * currentWeaponStats.speedIncrease));
			currentWeaponStats.shotDelay = baseShotDelay * (1 + ((currentWeaponStats.speedLevel) * -currentWeaponStats.speedIncrease));
			//increase weapon worth
			currentWeaponStats.costCurrency += cost;
			if(currentWeaponStats.speedLevel == maxUpgradeLevels) {
				transform.FindChild("S").GetComponentInChildren<Text>().text = "MAX";
				transform.FindChild("S").GetComponentInChildren<Button>().interactable = false;
			}
			else {
				transform.FindChild("S").GetComponentInChildren<Text>().text = "$" + (baseUpgradeCost * (currentWeaponStats.speedLevel + 1));
				transform.FindChild("S").GetComponentInChildren<SpriteRenderer>().sprite = rankSprites[currentWeaponStats.speedLevel];
			}
		}
	}
}
