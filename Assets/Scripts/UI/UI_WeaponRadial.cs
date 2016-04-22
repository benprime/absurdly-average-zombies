using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
		this.buttons["W"].GetComponentInChildren<Button>().interactable = !buttonDisabled["S"];

		//Flamethrower only accessible after level 5 of actual gameplay
		if(currLevel >= 5 /*level 1*/ && currLevel <= 9 /*level 5*/) 
			this.buttons["S"].GetComponentInChildren<Button>().interactable = false;
		else 
			this.buttons["S"].GetComponentInChildren<Button>().interactable = !buttonDisabled["E"];
		
		//Rocket launcher only accessible after level 10 of actual gameplay
		if(currLevel >= 5 /*level 1*/ && currLevel <= 14 /*level 10*/)  
			this.buttons["E"].GetComponentInChildren<Button>().interactable = false;
		else 
			this.buttons["E"].GetComponentInChildren<Button>().interactable = !buttonDisabled["W"];
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
