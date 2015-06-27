using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_FloatingHealthBar : MonoBehaviour {
	public Image healthBarPrefab;
	public float barOffset = 10f;
	public Transform uiCanvas;
	private Image healthBar;

	// Use this for initialization
	void Start () {
		healthBar = Instantiate(healthBarPrefab) as Image;
		healthBar.transform.SetParent (uiCanvas, false);
		healthBar.transform.position.Set(0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		healthBar.rectTransform.position = transform.position;
		//healthBar.rectTransform.position.y += barOffset;  //TODO: Make this work
		//healthBar.fillAmount = targetHealth/100;  //TODO: Hook bar to target health and modify with damage/heal
	}
}
