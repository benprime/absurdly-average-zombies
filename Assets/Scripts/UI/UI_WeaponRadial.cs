using UnityEngine;
using System.Collections.Generic;
using System.Net;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_WeaponRadial : MonoBehaviour
{
    public BuildZone connectedZone;
    public static Dictionary<string, bool> buttonDisabled = new Dictionary<string, bool>()
    {
        {"N", false },
        {"S", false },
        {"E", false },
        {"W", false },
    };
    Dictionary<string, Transform> buttons = new Dictionary<string, Transform>();

    void Awake()
    {
        int currLevel = SceneManager.GetActiveScene().buildIndex;

        this.buttons["N"] = transform.FindChild("N");
        this.buttons["S"] = transform.FindChild("S");
        this.buttons["E"] = transform.FindChild("E");
        this.buttons["W"] = transform.FindChild("W");

        //Machine gun is always accessible
        this.buttons["N"].GetComponentInChildren<Button>().interactable = !buttonDisabled["N"];

        //Tar launcher is always accessible
        this.buttons["W"].GetComponentInChildren<Button>().interactable = !buttonDisabled["W"];

        //Flamethrower only accessible after level 5 of actual gameplay
        if (currLevel >= 5 /*level 1*/ && currLevel <= 9 /*level 5*/)
            this.buttons["S"].GetComponentInChildren<Button>().interactable = false;
        else
            this.buttons["S"].GetComponentInChildren<Button>().interactable = !buttonDisabled["S"];

        //Rocket launcher only accessible after level 10 of actual gameplay
        if (currLevel >= 5 /*level 1*/ && currLevel <= 14 /*level 10*/)
            this.buttons["E"].GetComponentInChildren<Button>().interactable = false;
        else
            this.buttons["E"].GetComponentInChildren<Button>().interactable = !buttonDisabled["E"];
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BuildSelectedObject(GameObject obj)
    {
        if (connectedZone == null)
        {
            Destroy(gameObject);
            return;
        }

        Input.ResetInputAxes();

		int cost = (int)TurretUpgradeInfo.GetData (obj.GetComponent<Turret>(), TurretField.Cost);
        if (GameManager.Instance.GetPlayerTotalCurrency() >= cost)
        {
            GameManager.Instance.PlayerCurrencyTransaction(-cost);
            GameObject weap = Instantiate(obj, connectedZone.transform.position, Quaternion.identity) as GameObject;

            Turret t = weap.GetComponent<Turret>();
            switch (t.type)
            {
                case TurretTypes.FlameThrower:
                    GameManager.Instance.FireMoney += cost;
                    break;
                case TurretTypes.MachineGun:
                    GameManager.Instance.BulletMoney += cost;
                    break;
                case TurretTypes.RocketLauncher:
                    GameManager.Instance.RocketMoney += cost;
                    break;
            }

            connectedZone.currentState = BuildZone.ZONE_STATE.BUILT_ON;
            connectedZone.currentWeapon = weap;
            connectedZone.CloseOut();
        }
	}

	public void InitRadial()
	{
		if (connectedZone)
		{
			this.buttons["N"].GetComponentInChildren<Text>().text = "$" + (int)TurretUpgradeInfo.GetData(TurretTypes.MachineGun, TurretField.Cost, 0);
			this.buttons["E"].GetComponentInChildren<Text>().text = "$" + (int)TurretUpgradeInfo.GetData(TurretTypes.RocketLauncher, TurretField.Cost, 0);
			this.buttons["S"].GetComponentInChildren<Text>().text = "$" + (int)TurretUpgradeInfo.GetData(TurretTypes.FlameThrower, TurretField.Cost, 0);
			this.buttons["W"].GetComponentInChildren<Text>().text = "$" + (int)TurretUpgradeInfo.GetData(TurretTypes.TarSlinger, TurretField.Cost, 0);
		}
	}
}
