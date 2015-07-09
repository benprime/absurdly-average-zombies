using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuildZone : MonoBehaviour {
	public enum ZONE_STATE {EMPTY, BUILT_ON, DESTROYED};
	public ZONE_STATE currentState = ZONE_STATE.EMPTY;
	public GameObject weaponRadial;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PopRadialMenu() {
		if(currentState == ZONE_STATE.EMPTY) {
			Transform uiCanvas = FindObjectOfType<Canvas>().transform;
			GameObject hub = Instantiate(weaponRadial, Camera.main.WorldToScreenPoint(transform.position), Quaternion.identity) as GameObject;
			hub.transform.SetParent(uiCanvas, false);
			currentState = ZONE_STATE.BUILT_ON;
		}
	}
}
