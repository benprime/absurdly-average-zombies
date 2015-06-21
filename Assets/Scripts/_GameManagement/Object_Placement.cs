﻿using UnityEngine;
using System.Collections;

public class Object_Placement : MonoBehaviour {
	public GameObject obj, placementIcon;
	private GameObject square;
	public Color openPlace, blockPlace;
	private bool isPlaceable = true;
	public float snapSize = 1;

	// there's some reason for this that has to do with computations
	// TODO: lookup why this is better
	//float snapInverse;

	//private Rect sqSize;

	// Use this for initialization
	void Start () {
		//this.snapInverse = 1/snapSize;
	}
	
	// Update is called once per frame
	void Update () {

		// check if this is game level or a menu
		// TODO: should this script be re-factored into the game manager?
		if (!GameManager.instance.menu) {

			if (Input.GetMouseButtonUp (0)) {
				if (isPlaceable) {
					int cost = obj.GetComponent<Turret_Stats> ().costCurrency;
					if (GameManager.instance.GetPlayerTotalCurrency () >= cost) {
						Vector3 target = Camera.main.ScreenToWorldPoint (Input.mousePosition);
						target.z = 0;
						GameManager.instance.PlayerCurrencyTransaction (-cost);
						Instantiate (obj, square.transform.position, Quaternion.identity);
					}
				}
				//Debug.Log ("Destroying square");
				Destroy (square);
			} else if (Input.GetMouseButtonDown (0)) {
				if (!square) {
					Vector3 temp = Camera.main.ScreenToWorldPoint (Input.mousePosition);
					temp.z = 0;
					square = Instantiate (placementIcon, temp, Quaternion.identity) as GameObject;
					//	square.GetComponent<SpriteRenderer>().color = openPlace;
					//	isPlaceable = true;
					//sqSize = square.GetComponent<SpriteRenderer>().sprite.rect;
				}
			} else {
				if (square) {
					Vector3 currPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);

					currPos.x = Mathf.Round (currPos.x);// + (sqSize.width / 2));  //TODO: figure out sprite offset
					currPos.y = Mathf.Round (currPos.y);//+ (sqSize.height / 2));

					//currPos.x = Mathf.Round(currPos.x * snapInverse)/snapInverse;
					//currPos.y = Mathf.Round(currPos.y * snapInverse)/snapInverse;   

					currPos.z = 0;
					square.transform.position = currPos;
					if (square.GetComponent<BoxCollider2D> ().IsTouchingLayers ()) {
						square.GetComponent<SpriteRenderer> ().color = blockPlace;
						isPlaceable = false;
					} else {
						square.GetComponent<SpriteRenderer> ().color = openPlace;
						isPlaceable = true;
					}
				}
			}
		}
	}

	public void SelectTurretType(GameObject turret) {
		obj = turret;
	}
}
