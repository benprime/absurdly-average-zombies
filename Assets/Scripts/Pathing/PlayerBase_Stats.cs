using UnityEngine;
using System.Collections;

public class PlayerBase_Stats : MonoBehaviour {
	public float maxHitPoints = 100f;
	public float currentHitPoints = 100f;
	public GameObject destroyMessage;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {		
		if (currentHitPoints <= 0) {
			GameObject clone = Instantiate(destroyMessage) as GameObject;
			Transform uiCanvas = FindObjectOfType<Canvas>().transform;
			clone.transform.SetParent (uiCanvas, false);
			Destroy(gameObject);
		}
	}
	
	void TakeDamage(float damage) {
		currentHitPoints -= damage;
		//if(currentHitPoints < (maxHitPoints / 3)) {
		//	gameObject.GetComponent<SpriteRenderer>().color = Color.red;
		//}
		UI_FloatingHealthBar hb = GetComponent<UI_FloatingHealthBar>();
		hb.healthBar.rectTransform.localScale = new Vector3(Mathf.Max (0, (currentHitPoints / maxHitPoints)), 1, 1); //TODO: Make healthbar scale from left pivot point
		
		if(currentHitPoints <= 0) Destroy (hb.healthBar.gameObject);
		if(currentHitPoints <= maxHitPoints / 3) {
			hb.healthBar.color = Color.red;
		}
		else if(currentHitPoints <= 2 * (maxHitPoints / 3)) {
			hb.healthBar.color = Color.yellow;
		}
	}
}
