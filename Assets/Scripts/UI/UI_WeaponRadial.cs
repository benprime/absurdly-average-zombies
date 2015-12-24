using UnityEngine;
using System.Collections;

public class UI_WeaponRadial : MonoBehaviour
{
    public BuildZone connectedZone;

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
        //GameObject objToPlace = GameManager.instance.selectedObjToBuild;
        //int cost = objToPlace.GetComponent<Turret_Stats> ().costCurrency;
        //if (GameManager.instance.GetPlayerTotalCurrency () >= cost) {
        //	GameManager.instance.PlayerCurrencyTransaction (-cost);
        //	Instantiate (objToPlace, transform.position, Quaternion.identity);
        //	//Destroy (hitZone);
        //}

        int cost = obj.GetComponent<Turret>().costCurrency;
        if (GameManager.instance.GetPlayerTotalCurrency() >= cost)
        {
            GameManager.instance.PlayerCurrencyTransaction(-cost);
            GameObject weap = Instantiate(obj, transform.position, Quaternion.identity) as GameObject;
            if (connectedZone)
            {
                connectedZone.currentState = BuildZone.ZONE_STATE.BUILT_ON;
				connectedZone.currentWeapon = weap;
				connectedZone.CloseOut ();
			}
			else Destroy (gameObject);
        }
    }
}
