using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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

    }

    public void BuildSelectedObject(GameObject obj)
    {
        Input.ResetInputAxes();

        int cost = obj.GetComponent<Turret>().costCurrency;
        if (GameManager.instance.GetPlayerTotalCurrency() >= cost)
        {
            GameManager.instance.PlayerCurrencyTransaction(-cost);
            GameObject weap = Instantiate(obj, transform.position, Quaternion.identity) as GameObject;
            if (connectedZone)
            {
                connectedZone.currentState = BuildZone.ZONE_STATE.BUILT_ON;
                connectedZone.currentWeapon = weap;
                connectedZone.CloseOut();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
