using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class UI_UpgradeRadial : MonoBehaviour
{
	public BuildZone connectedZone;
	private SpriteRenderer currentWeaponUpgradeSprite;
    private Turret currentWeaponStats;
    public int maxUpgradeLevels = 3;
    public Sprite[] rankSprites;
    private float baseRotSpd = -1f, baseShotDelay = -1f, baseRange = -1f;

    // global override for enabling buttons (used by tutorial)
    public static Dictionary<string, bool> buttonDisabled = new Dictionary<string, bool>()
    {
        {"E", false },
        {"W", false },
    };
    Dictionary<string, Transform> buttons = new Dictionary<string, Transform>();


	public Sprite sellConfirmSprite;
	public bool saleInitiated = false;

    void Awake()
    {
        this.buttons["E"] = transform.FindChild("E");
        this.buttons["W"] = transform.FindChild("W");

        this.buttons["E"].GetComponentInChildren<Button>().interactable = !buttonDisabled["E"];
        this.buttons["W"].GetComponentInChildren<Button>().interactable = !buttonDisabled["W"];
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SellWeapon()
    {
		if (!saleInitiated) {
			saleInitiated = true;
			this.buttons ["W"].GetComponent<Image> ().sprite = sellConfirmSprite;
		} else {
			if (connectedZone) {
				int worth = (currentWeaponStats.costCurrency) / 2;  //sell for half of purchase cost
				GameManager.instance.PlayerCurrencyTransaction (worth);
				connectedZone.Clear ();
				connectedZone.CloseOut ();
			}
		}
    }

    private int CalculateUpgradeCost(int upgradeLevel)
    {
        return currentWeaponStats.baseCost + (5 * (upgradeLevel + 1));
    }

    public void UpgradeDamage()
    {
        currentWeaponStats.damageLevel++;
        currentWeaponStats.damage = (int)(currentWeaponStats.baseDamage * (1 + (currentWeaponStats.damageLevel * currentWeaponStats.damageIncrease)));
    }

    public void UpgradeRange()
    {
        if (baseRange == -1f) baseRange = currentWeaponStats.transform.FindChild("DetectionZone").localScale.x;
        currentWeaponStats.rangeLevel++;
        float increase = baseRange * (1 + ((currentWeaponStats.rangeLevel) * currentWeaponStats.rangeIncrease));
        currentWeaponStats.transform.FindChild("DetectionZone").localScale = new Vector3(increase, increase, 1f);
    }

    public void UpgradeSpeed()
    {
        if (baseRotSpd < 0 || baseShotDelay < 0)
        {
            baseRotSpd = currentWeaponStats.rotationSpeed;
            baseShotDelay = currentWeaponStats.shotDelay;
        }

        //upgrade weapon
        currentWeaponStats.speedLevel++;
        currentWeaponStats.rotationSpeed = baseRotSpd * (1 + ((currentWeaponStats.speedLevel) * currentWeaponStats.speedIncrease));
        currentWeaponStats.shotDelay = baseShotDelay * (1 + ((currentWeaponStats.speedLevel) * -currentWeaponStats.speedIncrease));
    }

    public void UpgradeAll()
    {
        Input.ResetInputAxes();
        if (connectedZone && currentWeaponStats.damageLevel < maxUpgradeLevels)
        {
            //charge player cost of upgrade
            int cost = CalculateUpgradeCost(currentWeaponStats.damageLevel);
            if (GameManager.instance.GetPlayerTotalCurrency() < cost) return;
            GameManager.instance.PlayerCurrencyTransaction(-cost);
            //increase weapon worth
            currentWeaponStats.costCurrency += cost;

            UpgradeRange();
            UpgradeDamage();
            UpgradeSpeed();

			SetMEGAEPICAWESOMEEVERTHINGISGONNASPLODEButton();
        }
    }

    public void InitRadial()
    {
        if (connectedZone)
        {
			currentWeaponUpgradeSprite = connectedZone.transform.FindChild("Stars").GetComponent<SpriteRenderer>();
            currentWeaponStats = connectedZone.currentWeapon.GetComponent<Turret>();

            //much hackery going on here due to lack of sleep TODO: optimize!!!! (shouldn't be in the update loop)
            SetSellButton();
            SetMEGAEPICAWESOMEEVERTHINGISGONNASPLODEButton();
        }
    }

    public void SetSellButton()
    {
        if (buttonDisabled["W"])
        {
            this.buttons["W"].GetComponentInChildren<Button>().interactable = false;
            return;
        }

        Text sellCostText = this.buttons["W"].GetComponentInChildren<Text>();
        sellCostText.text = "$" + ((currentWeaponStats.costCurrency) / 2);
    }

	void SetMEGAEPICAWESOMEEVERTHINGISGONNASPLODEButton()
	{
		if (buttonDisabled["E"])
		{
			this.buttons["E"].GetComponentInChildren<Button>().interactable = false;
			return;
		}

		if (currentWeaponStats.rangeLevel == maxUpgradeLevels)
		{
			this.buttons["E"].GetComponentInChildren<Text>().text = "MAX";
			this.buttons["E"].GetComponentInChildren<SpriteRenderer>().sprite = rankSprites[currentWeaponStats.rangeLevel - 1];
			currentWeaponUpgradeSprite.sprite = rankSprites [currentWeaponStats.rangeLevel - 1];
			this.buttons["E"].GetComponentInChildren<Button>().interactable = false;
		}
		else
		{
			this.buttons["E"].GetComponentInChildren<Text>().text = "$" + CalculateUpgradeCost(currentWeaponStats.rangeLevel);
			this.buttons["E"].GetComponentInChildren<SpriteRenderer>().sprite = rankSprites[currentWeaponStats.rangeLevel];
			if(currentWeaponStats.rangeLevel > 0) currentWeaponUpgradeSprite.sprite = rankSprites[currentWeaponStats.rangeLevel - 1];
		}
	}
}
