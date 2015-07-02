using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Object_Placement : MonoBehaviour {
	public GameObject obj, placementIcon;
	private GameObject square;
	public Color openPlace, blockPlace;
	private bool isPlaceable = false;
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
				if(!EventSystem.current.IsPointerOverGameObject()){ //do not place object when mouse is over button
					if (isPlaceable) {
						int cost = obj.GetComponent<Turret_Stats> ().costCurrency;
						if (GameManager.instance.GetPlayerTotalCurrency () >= cost) {
							Vector3 target = Camera.main.ScreenToWorldPoint (Input.mousePosition);
							target.z = 0;
							GameManager.instance.PlayerCurrencyTransaction (-cost);
							Instantiate (obj, square.transform.position, Quaternion.identity);
						}
					}
				}
				Destroy (square);
			} else if (Input.GetMouseButtonDown (0)) {
				if(!EventSystem.current.IsPointerOverGameObject()){ //do not begin placing object when mouse is over a button
					if (!square) {
						Vector3 temp = Camera.main.ScreenToWorldPoint (Input.mousePosition);
						temp.z = 0;
						square = Instantiate (placementIcon, temp, Quaternion.identity) as GameObject;
					}
				}
			} else {
				if (square) {
					Vector3 currPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);

					//lock mouse cursor to grid cordinates
					//currPos.x = Mathf.Round (currPos.x);
					//currPos.y = Mathf.Round (currPos.y);

					currPos.z = 0;
					square.transform.position = currPos;
					if (!square.GetComponent<BoxCollider2D> ().IsTouchingLayers (LayerMask.GetMask("BuildZone"))) {
						square.GetComponent<SpriteRenderer> ().color = blockPlace;
						isPlaceable = false;
					} else {
						square.GetComponent<SpriteRenderer> ().color = openPlace;
						isPlaceable = true;  

						//WIP lock mouse cursor to build zone - probably getting scrapped
						//RaycastHit hitInfo = new RaycastHit();
						//bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), 100, LayerMask.GetMask("Enemy_Targetable"));
						//if (hit) 
						//{
						//	Debug.Log("Hit " + hitInfo.transform.gameObject.name);
						//} else {
						//	Debug.Log("No hit");
						//}
					}
				}
			}
		}
	}
}
