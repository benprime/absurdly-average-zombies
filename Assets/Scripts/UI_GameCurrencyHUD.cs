using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_GameCurrencyHUD : MonoBehaviour {
	private GameManager_Stats gm;

	// Use this for initialization
	void Start () {
		gm = FindObjectOfType<GameManager_Stats>();
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.GetComponent<Text>().text = "$" + gm.GetPlayerTotalCurrency ();
	}
}
