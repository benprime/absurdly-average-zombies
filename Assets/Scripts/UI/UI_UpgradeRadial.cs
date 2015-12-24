using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_UpgradeRadial : MonoBehaviour
{
    public BuildZone connectedZone;
    private Turret currentWeaponStats;
    private int baseUpgradeCost = 5;
    public int maxUpgradeLevels = 3;
    public Sprite[] rankSprites;
    private float baseRotSpd = -1f, baseShotDelay = -1f, baseRange = -1f;
	private float timer = 0f;
	public float buttonHoldDelay = 1f;
	private bool buttonHeld = false;
	
	public Color sellColor = Color.green;
    // Use this for initialization
    void Start()
    {
		baseUpgradeCost = currentWeaponStats.costCurrency;
        //NEEDS WORK BAD!!!!!!!!!!!!!!!!!!!!


        //float moveX = 0, moveY = 0;
        //Vector3 posVS = Camera.main.WorldToScreenPoint (transform.position);
        //float leftX =  posVS.x - 118;  //TODO: half of the radial menu size (probably not static...)
        //float rightX =  posVS.x + 118;
        //float bottomY =  posVS.y - 118;
        //float topY =  posVS.y + 118;
        //if(leftX < 0) moveX = 0 - leftX;
        //else if(rightX > 1) moveX = 1 - rightX;
        //if(bottomY < 0) moveY = 0 - bottomY;
        //else if(topY > 1) moveY = 1 - topY;
        //posVS = new Vector3(posVS.x + moveX, posVS.y + moveY, posVS.z);
        //transform.position = Camera.main.ViewportToWorldPoint(posVS);
    }

    // Update is called once per frame
    void Update()
    {
		if (buttonHeld && ((Time.time - timer) < buttonHoldDelay)) {
			Button b = transform.FindChild ("W").GetComponent<Button> ();
			ColorBlock cb = b.colors;
			sellColor.g = ((Time.time - timer)/buttonHoldDelay) * .5f;
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
		if ((Time.time - timer) >= buttonHoldDelay) {
			if (connectedZone) {
				int worth = (currentWeaponStats.costCurrency) / 2;  //sell for half of purchase cost
				GameManager.instance.PlayerCurrencyTransaction (worth);
				Destroy (connectedZone.currentWeapon);
				connectedZone.currentState = BuildZone.ZONE_STATE.EMPTY;
				connectedZone.CloseOut ();
				//Destroy (gameObject);
			}
		}
		timer = 0;
		buttonHeld = false;
    }

	private int CalculateUpgradeCost(int upgradeLevel)
	{
		return baseUpgradeCost + (int)((currentWeaponStats.costCurrency * .5f) * (upgradeLevel + .25));
	}

    public void UpgradeDamage()
    {
        if (connectedZone && currentWeaponStats.damageLevel < maxUpgradeLevels)
        {
            //charge player cost of upgrade
			int cost = CalculateUpgradeCost(currentWeaponStats.damageLevel);
            if (GameManager.instance.GetPlayerTotalCurrency() < cost) return;
            GameManager.instance.PlayerCurrencyTransaction(-cost);
            //upgrade weapon
            currentWeaponStats.damageLevel++;
            currentWeaponStats.damage = (int)(currentWeaponStats.baseDamage * (1 + (currentWeaponStats.damageLevel * currentWeaponStats.damageIncrease)));
            //increase weapon worth
            currentWeaponStats.costCurrency += cost;
            UpdateAllButtons();
        }
    }

    public void UpgradeRange()
    {
        if (connectedZone && currentWeaponStats.rangeLevel < maxUpgradeLevels)
        {
            if (baseRange == -1f) baseRange = currentWeaponStats.transform.FindChild("DetectionZone").localScale.x;
            //if(baseRange < 0) baseRange = currentWeaponStats.transform.FindChild ("DetectionZone").GetComponent<CircleCollider2D>().radius;
            //charge player cost of upgrade
			int cost = CalculateUpgradeCost(currentWeaponStats.rangeLevel);
            if (GameManager.instance.GetPlayerTotalCurrency() < cost) return;
            GameManager.instance.PlayerCurrencyTransaction(-cost);
            //upgrade weapon
            currentWeaponStats.rangeLevel++;
            float increase = baseRange * (1 + ((currentWeaponStats.rangeLevel) * currentWeaponStats.rangeIncrease));
            currentWeaponStats.transform.FindChild("DetectionZone").localScale = new Vector3(increase, increase, 1f);
            //currentWeaponStats.transform.FindChild ("DetectionZone").GetComponent<CircleCollider2D>().radius = baseRange * (1 + ((currentWeaponStats.rangeLevel) * currentWeaponStats.rangeIncrease));
            //increase weapon worth
            currentWeaponStats.costCurrency += cost;
			UpdateAllButtons();
        }
    }

    public void UpgradeSpeed()
    {
        if (connectedZone && currentWeaponStats.speedLevel < maxUpgradeLevels)
        {
            if (baseRotSpd < 0 || baseShotDelay < 0)
            {
                baseRotSpd = currentWeaponStats.rotationSpeed;
                baseShotDelay = currentWeaponStats.shotDelay;
            }
            //charge player cost of upgrade
			int cost = CalculateUpgradeCost(currentWeaponStats.speedLevel);
            if (GameManager.instance.GetPlayerTotalCurrency() < cost) return;
            GameManager.instance.PlayerCurrencyTransaction(-cost);
            //upgrade weapon
            currentWeaponStats.speedLevel++;
            currentWeaponStats.rotationSpeed = baseRotSpd * (1 + ((currentWeaponStats.speedLevel) * currentWeaponStats.speedIncrease));
            currentWeaponStats.shotDelay = baseShotDelay * (1 + ((currentWeaponStats.speedLevel) * -currentWeaponStats.speedIncrease));
            //increase weapon worth
            currentWeaponStats.costCurrency += cost;
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
		Text sellCostText = transform.FindChild("W").GetComponentInChildren<Text>();
		sellCostText.text = "$" + ((currentWeaponStats.costCurrency) / 2);
	}

    void SetDamageButton()
    {
        if (currentWeaponStats.damageLevel == maxUpgradeLevels)
        {
            transform.FindChild("N").GetComponentInChildren<Text>().text = "MAX";
            transform.FindChild("N").GetComponentInChildren<SpriteRenderer>().sprite = rankSprites[currentWeaponStats.damageLevel - 1];
            transform.FindChild("N").GetComponentInChildren<Button>().interactable = false;
        }
        else
        {
			transform.FindChild("N").GetComponentInChildren<Text>().text = "$" + CalculateUpgradeCost(currentWeaponStats.damageLevel);
            transform.FindChild("N").GetComponentInChildren<SpriteRenderer>().sprite = rankSprites[currentWeaponStats.damageLevel];
        }
    }

    void SetRangeButton()
    {
        if (currentWeaponStats.rangeLevel == maxUpgradeLevels)
        {
            transform.FindChild("E").GetComponentInChildren<Text>().text = "MAX";
            transform.FindChild("E").GetComponentInChildren<SpriteRenderer>().sprite = rankSprites[currentWeaponStats.rangeLevel - 1];
            transform.FindChild("E").GetComponentInChildren<Button>().interactable = false;
        }
        else
        {
			transform.FindChild("E").GetComponentInChildren<Text>().text = "$" + CalculateUpgradeCost(currentWeaponStats.rangeLevel);
            transform.FindChild("E").GetComponentInChildren<SpriteRenderer>().sprite = rankSprites[currentWeaponStats.rangeLevel];
        }
    }

    void SetSpeedButton()
    {
        if (currentWeaponStats.speedLevel == maxUpgradeLevels)
        {
            transform.FindChild("S").GetComponentInChildren<Text>().text = "MAX";
            transform.FindChild("S").GetComponentInChildren<SpriteRenderer>().sprite = rankSprites[currentWeaponStats.speedLevel - 1];
            transform.FindChild("S").GetComponentInChildren<Button>().interactable = false;
        }
        else
        {
			transform.FindChild("S").GetComponentInChildren<Text>().text = "$" + CalculateUpgradeCost(currentWeaponStats.speedLevel);
            transform.FindChild("S").GetComponentInChildren<SpriteRenderer>().sprite = rankSprites[currentWeaponStats.speedLevel];
        }
    }

	void UpdateAllButtons() 
	{
		SetRangeButton ();
		SetDamageButton ();
		SetSpeedButton ();
		SetSellButton ();
	}
}
