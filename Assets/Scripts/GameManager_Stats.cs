using UnityEngine;
using System.Collections;

public class GameManager_Stats : MonoBehaviour {
	private int player_totalCurrency = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlayerCurrencyTransaction(int amount) {
		player_totalCurrency += amount;
	}

	public int GetPlayerTotalCurrency() {
		return player_totalCurrency;
	}
}
