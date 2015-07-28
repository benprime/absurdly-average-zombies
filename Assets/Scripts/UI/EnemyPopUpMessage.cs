using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyPopUpMessage : MonoBehaviour {
	public float lifespan = 1f;
	public float moveSpeed = 1f;
	private float timer = 0;

	// Use this for initialization
	void Awake () {
		Transform uiCanvas = FindObjectOfType<Canvas>().transform;
		transform.SetParent(uiCanvas, false);
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		transform.Translate (0, moveSpeed * Time.deltaTime, 0);
		Color c = gameObject.GetComponent<Text>().color;
		c.a = 1 - (timer / lifespan);
		gameObject.GetComponent<Text>().color = c;
		if(timer >= lifespan) Destroy (this.gameObject);
	}
}
