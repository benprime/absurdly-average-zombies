using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_FloatingHealthBar : MonoBehaviour {
	public Image healthBarPrefab;
	public float barOffset = .5f;
	private Transform uiCanvas;
	public Image healthBar;

	// Use this for initialization
	void Start () {
		uiCanvas = FindObjectOfType<Canvas>().transform;
		healthBar = Instantiate(healthBarPrefab) as Image;
		healthBar.transform.SetParent (uiCanvas, false);
		healthBar.color = Color.green;
		healthBar.rectTransform.position = new Vector3(transform.position.x, transform.position.y + barOffset, transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		healthBar.rectTransform.position = new Vector3(transform.position.x, transform.position.y + barOffset, transform.position.z);
	}
}
