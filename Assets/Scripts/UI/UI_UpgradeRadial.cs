using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class UI_UpgradeRadial : MonoBehaviour
{
	public BuildZone connectedZone;
	private SpriteRenderer currentWeaponUpgradeSprite;
    private Turret turret;
    public int maxUpgradeLevels = 3;
    public Sprite[] rankSprites;

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
				int worth = (turret.costCurrency) / 2;  //sell for half of purchase cost
				GameManager.instance.PlayerCurrencyTransaction (worth);
				connectedZone.Clear ();
				connectedZone.CloseOut ();
			}
		}
    }

    private int CalculateUpgradeCost(int upgradeLevel)
    {
        return turret.baseCost + (5 * (upgradeLevel + 1));
    }


    public void UpgradeAll()
    {
        Input.ResetInputAxes();

        if (connectedZone)
        {
            //charge player cost of upgrade
            int cost = CalculateUpgradeCost(turret.level);
            if (GameManager.instance.GetPlayerTotalCurrency() < cost) return;
            GameManager.instance.PlayerCurrencyTransaction(-cost);
            //increase weapon worth
            turret.costCurrency += cost;

            // turret upgrades
            turret.damage = TurretUpgradeInfo.GetData(this.turret, TurretField.Damage);
            turret.damage = TurretUpgradeInfo.GetData(this.turret, TurretField.Damage);
            float detectionZoneDiameter = TurretUpgradeInfo.GetData(this.turret, TurretField.Range);
            // todo: this is probably wrong.  How do we just set the diameter so we can keep our upgrade info readable and simple?
            turret.transform.FindChild("DetectionZone").localScale = new Vector3(detectionZoneDiameter, detectionZoneDiameter, 1f);
            turret.rotationSpeed = TurretUpgradeInfo.GetData(this.turret, TurretField.RotationSpeed);
            turret.shotDelay = TurretUpgradeInfo.GetData(this.turret, TurretField.ShotDelay);

            SetMEGAEPICAWESOMEEVERTHINGISGONNASPLODEButton();
        }
    }

    public void InitRadial()
    {
        if (connectedZone)
        {
			currentWeaponUpgradeSprite = connectedZone.transform.FindChild("Stars").GetComponent<SpriteRenderer>();
            turret = connectedZone.currentWeapon.GetComponent<Turret>();

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
        sellCostText.text = "$" + ((turret.costCurrency) / 2);
    }

	void SetMEGAEPICAWESOMEEVERTHINGISGONNASPLODEButton()
	{
		if (buttonDisabled["E"])
		{
			this.buttons["E"].GetComponentInChildren<Button>().interactable = false;
			return;
		}

		if (turret.level == maxUpgradeLevels)
		{
			this.buttons["E"].GetComponentInChildren<Text>().text = "MAX";
			this.buttons["E"].GetComponentInChildren<SpriteRenderer>().sprite = rankSprites[turret.level - 1];
			currentWeaponUpgradeSprite.sprite = rankSprites [turret.level - 1];
			this.buttons["E"].GetComponentInChildren<Button>().interactable = false;
		}
		else
		{
			this.buttons["E"].GetComponentInChildren<Text>().text = "$" + CalculateUpgradeCost(turret.level);
			this.buttons["E"].GetComponentInChildren<SpriteRenderer>().sprite = rankSprites[turret.level];
			if(turret.level > 0)
            { 
                currentWeaponUpgradeSprite.sprite = rankSprites[turret.level - 1];
            }
        }
	}
}
