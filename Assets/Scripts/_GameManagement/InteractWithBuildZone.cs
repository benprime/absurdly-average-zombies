using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractWithBuildZone : MonoBehaviour {

	private static GameObject lastHit = null;

	// Use this for initialization
	void Start () {

	}

	public void HideRadialMenu() {
		if(lastHit) lastHit.GetComponent<BuildZone>().CloseOut();
	}
	
	
	// Update is called once per frame
	void Update () {
		int pointerId = (Input.touchCount == 1) ? Input.GetTouch(0).fingerId : -1;  //Only accounts for single touches TODO: make it work nicely for accidental multi touches

		RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.forward, 20, LayerMask.GetMask("BuildZone"));
		if(hitInfo && !EventSystem.current.IsPointerOverGameObject(pointerId)) { //mouse is over a BuildZone && do not place object when mouse is over button
			GameObject hitZone = hitInfo.transform.gameObject;
			//SpriteRenderer hitSprite = hitZone.GetComponent<SpriteRenderer> ();

			if(Input.GetMouseButtonDown (0)) {
				//hitSprite.color = Color.green;  //TODO: make flag in BuildZone class that sets color when mouse is over it
			}
			bool isSingleTouchEnding = (Input.touchCount == 1) ? (Input.GetTouch (0).phase == TouchPhase.Ended) : false; //check to see if there is touch input, and if so, if the touch just ended
			if (Input.GetMouseButtonUp (0) || isSingleTouchEnding) {
				//if(!EventSystem.current.IsPointerOverGameObject(pointerId)){ //
				if(lastHit && lastHit != hitZone) lastHit.GetComponent<BuildZone>().CloseOut();
				lastHit = hitZone;

                    hitZone.GetComponent<BuildZone>().PopRadialMenu (hitZone.transform.position);

					//GameObject objToPlace = GameManager.instance.selectedObjToBuild;
					//int cost = objToPlace.GetComponent<Turret_Stats> ().costCurrency;
					//if (GameManager.instance.GetPlayerTotalCurrency () >= cost) {
					//	GameManager.instance.PlayerCurrencyTransaction (-cost);
					//	Instantiate (objToPlace, hitZone.transform.position, Quaternion.identity);
					//	//Destroy (hitZone);
					//}
				//}				
			}
		}
		else { //mouse is not over a BuildZone			
			if(!EventSystem.current.IsPointerOverGameObject(pointerId)){
				if(Input.GetMouseButtonDown (0)) {
					if(lastHit) lastHit.GetComponent<BuildZone>().CloseOut();
				}
			}
		}

	}

}
