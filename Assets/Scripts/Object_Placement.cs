using UnityEngine;
using System.Collections;

public class Object_Placement : MonoBehaviour {
	public GameObject obj, placementIcon;
	private GameObject square;
	public Color openPlace, blockPlace;
	//private Rect sqSize;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonUp (0)) {
			Vector3 target = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			target.z = 0;
			Instantiate (obj, square.transform.position, Quaternion.identity);
			Destroy (square);
		}
		else if(Input.GetMouseButtonDown (0)) {
			if(!square) {
				Vector3 temp = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				temp.z = 0;
				square = Instantiate (placementIcon, temp, Quaternion.identity) as GameObject;
				square.GetComponent<SpriteRenderer>().color = openPlace;
				//sqSize = square.GetComponent<SpriteRenderer>().sprite.rect;
			}
		}
		else {
			if(square) {
				Vector3 currPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				currPos.x = Mathf.Floor (currPos.x);// + (sqSize.width / 2));  //TODO: figure out sprite offset
				currPos.y = Mathf.Floor (currPos.y);//+ (sqSize.height / 2));
				currPos.z = 0;
				square.transform.position = currPos;
				if(square.GetComponent<BoxCollider2D>().IsTouchingLayers ()) {
					square.GetComponent<SpriteRenderer>().color = blockPlace;
				}
				else {
					square.GetComponent<SpriteRenderer>().color = openPlace;
				}
			}
		}
	}

	public void SelectTurretType(GameObject turret) {
		obj = turret;
	}
}
