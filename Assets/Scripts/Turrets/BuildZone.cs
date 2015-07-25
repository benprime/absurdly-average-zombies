using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuildZone : MonoBehaviour {
	public enum ZONE_STATE {EMPTY, BUILT_ON, DESTROYED};
	public ZONE_STATE currentState = ZONE_STATE.EMPTY;
	public GameObject weaponRadial, upgradeRadial;
	private GameObject currentHub = null;
	public GameObject currentWeapon = null;
	public bool menuOpen = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PopRadialMenu(Vector3 location) {
		if(!currentHub) {
			if(currentState == ZONE_STATE.EMPTY) {
				Transform uiCanvas = FindObjectOfType<Canvas>().transform;
				currentHub = Instantiate(weaponRadial, transform.position, Quaternion.identity) as GameObject;
				currentHub.transform.SetParent(uiCanvas, false);
				currentHub.transform.position = location;
				currentHub.GetComponent<UI_WeaponRadial>().connectedZone = this;
				menuOpen = true;
			}
			if(currentState == ZONE_STATE.BUILT_ON) {				
				Transform uiCanvas = FindObjectOfType<Canvas>().transform;
				currentHub = Instantiate(upgradeRadial, transform.position, Quaternion.identity) as GameObject;
				currentHub.transform.SetParent(uiCanvas, false);
				currentHub.transform.position = location;
				currentHub.GetComponent<UI_UpgradeRadial>().connectedZone = this;
				menuOpen = true;
			}
		}
	}
		
	public void CloseOut() {
		if(currentHub) Destroy (currentHub);
		menuOpen = false;
	}
}
