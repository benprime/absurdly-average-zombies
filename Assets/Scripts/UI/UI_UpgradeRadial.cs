using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class UI_UpgradeRadial : MonoBehaviour
{
    public BuildZone connectedZone;
    private Turret currentWeaponStats;
    public int maxUpgradeLevels = 3;
    public Sprite[] rankSprites;
    private float baseRotSpd = -1f, baseShotDelay = -1f, baseRange = -1f;
    private float timer = 0f;
    public float buttonHoldDelay = 1f;
    private bool buttonHeld = false;

    // global override for enabling buttons (used by tutorial)
    public static Dictionary<string, bool> buttonDisabled = new Dictionary<string, bool>()
    {
        {"N", false },
        {"S", false },
        {"E", false },
        {"W", false },
    };
    Dictionary<string, Transform> buttons = new Dictionary<string, Transform>();

    public Color sellColor = Color.green;

    void Awake()
    {
        this.buttons["N"] = transform.FindChild("N");
        this.buttons["S"] = transform.FindChild("S");
        this.buttons["E"] = transform.FindChild("E");
        this.buttons["W"] = transform.FindChild("W");

        this.buttons["N"].GetComponentInChildren<Button>().interactable = !buttonDisabled["N"];
        this.buttons["S"].GetComponentInChildren<Button>().interactable = !buttonDisabled["S"];
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
        if (buttonHeld && ((Time.time - timer) < buttonHoldDelay))
        {
            Button b = transform.FindChild("W").GetComponent<Button>();
            ColorBlock cb = b.colors;
            sellColor.g = ((Time.time - timer) / buttonHoldDelay) * .5f;
            cb.pressedColor = sellColor;
            b.colors = cb;
        }
    }

    public void SellDelay()
    {
        timer = Time.time;
        buttonHeld = true;
    }

    public void SellWeapon()
    {
        Input.ResetInputAxes();
        if ((Time.time - timer) >= buttonHoldDelay)
        {
            if (connectedZone)
            {
                int worth = (currentWeaponStats.costCurrency) / 2;  //sell for half of purchase cost
                GameManager.instance.PlayerCurrencyTransaction(worth);
                Destroy(connectedZone.currentWeapon);
                connectedZone.currentState = BuildZone.ZONE_STATE.EMPTY;
                connectedZone.CloseOut();
            }
        }
        timer = 0;
        buttonHeld = false;
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

            UpdateAllButtons();
        }
    }

    public void InitRadial()
    {
        if (connectedZone)
        {
            currentWeaponStats = connectedZone.currentWeapon.GetComponent<Turret>();

            //much hackery going on here due to lack of sleep TODO: optimize!!!! (shouldn't be in the update loop)
            SetSellButton();

            //change upgrade image to show next available upgrade
            SetDamageButton();
            SetRangeButton();
            SetSpeedButton();
        }
    }

    public void SetSellButton()
    {
        if (buttonDisabled["W"])
        {
            this.buttons["E"].GetComponentInChildren<Button>().interactable = false;
            return;
        }

        Text sellCostText = this.buttons["E"].GetComponentInChildren<Text>();
        sellCostText.text = "$" + ((currentWeaponStats.costCurrency) / 2);
    }

    void SetDamageButton()
    {
        if (buttonDisabled["N"])
        {
            this.buttons["N"].GetComponentInChildren<Button>().interactable = false;
            return;
        }

        if (currentWeaponStats.damageLevel == maxUpgradeLevels)
        {
            this.buttons["N"].GetComponentInChildren<Text>().text = "MAX";
            this.buttons["N"].GetComponentInChildren<SpriteRenderer>().sprite = rankSprites[currentWeaponStats.damageLevel - 1];
            this.buttons["N"].GetComponentInChildren<Button>().interactable = false;
        }
        else
        {
            this.buttons["N"].GetComponentInChildren<Text>().text = "$" + CalculateUpgradeCost(currentWeaponStats.damageLevel);
            this.buttons["N"].GetComponentInChildren<SpriteRenderer>().sprite = rankSprites[currentWeaponStats.damageLevel];
        }

    }

    void SetRangeButton()
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
            this.buttons["E"].GetComponentInChildren<Button>().interactable = false;
        }
        else
        {
            this.buttons["E"].GetComponentInChildren<Text>().text = "$" + CalculateUpgradeCost(currentWeaponStats.rangeLevel);
            this.buttons["E"].GetComponentInChildren<SpriteRenderer>().sprite = rankSprites[currentWeaponStats.rangeLevel];
        }
    }

    void SetSpeedButton()
    {
        if (buttonDisabled["S"])
        {
            this.buttons["S"].GetComponentInChildren<Button>().interactable = false;
            return;
        }

        if (currentWeaponStats.speedLevel == maxUpgradeLevels)
        {
            this.buttons["S"].GetComponentInChildren<Text>().text = "MAX";
            this.buttons["S"].GetComponentInChildren<SpriteRenderer>().sprite = rankSprites[currentWeaponStats.speedLevel - 1];
            this.buttons["S"].GetComponentInChildren<Button>().interactable = false;
        }
        else
        {
            this.buttons["S"].GetComponentInChildren<Text>().text = "$" + CalculateUpgradeCost(currentWeaponStats.speedLevel);
            this.buttons["S"].GetComponentInChildren<SpriteRenderer>().sprite = rankSprites[currentWeaponStats.speedLevel];
        }
    }

    void UpdateAllButtons()
    {
        SetRangeButton();
        SetDamageButton();
        SetSpeedButton();
        SetSellButton();
    }
}
