using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractWithBuildZone : MonoBehaviour {

	private GameObject lastHit = null;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.forward, 20, LayerMask.GetMask("BuildZone"));
		if(hitInfo) { //mouse is over a BuildZone
			GameObject hitZone = hitInfo.transform.gameObject;
			SpriteRenderer hitSprite = hitZone.GetComponent<SpriteRenderer> ();

			if(Input.GetMouseButtonDown (0)) {
				//hitSprite.color = Color.green;  //TODO: make flag in BuildZone class that sets color when mouse is over it
				if(lastHit) lastHit.GetComponent<BuildZone>().CloseOut();
				lastHit = hitZone;
			}
			if (Input.GetMouseButtonUp (0)) {
				if(!EventSystem.current.IsPointerOverGameObject()){ //do not place object when mouse is over button
					hitZone.GetComponent<BuildZone>().PopRadialMenu (hitZone.transform.position);
				}				
			}
		}
		else { //mouse is not over a BuildZone
			if(Input.GetMouseButtonDown (0)) {
				if(!EventSystem.current.IsPointerOverGameObject()){
					if(lastHit) lastHit.GetComponent<BuildZone>().CloseOut();
				}
			}
		}

	}

}
