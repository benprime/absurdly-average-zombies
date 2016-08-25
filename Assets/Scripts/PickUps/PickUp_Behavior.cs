using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PickUp_Behavior : MonoBehaviour {

	private BoxCollider2D boxCol;

	public GameObject popNums;

	public int minReward = 5, maxReward = 10;
	private int worthCurrency = 5;

	// Use this for initialization
	void Awake () {
		boxCol = GetComponent <BoxCollider2D> ();
		transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360.0f));

		worthCurrency = Random.Range (minReward, maxReward);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount == 1) {
			Vector3 wtp = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);
			if (boxCol.OverlapPoint(wtp)) {
				CashAndBurn ();
			}
		}
		//for computer only
		else if (Input.GetMouseButtonUp(0)) {
			Vector3 wmp = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			if (boxCol.OverlapPoint(wmp)) {
				CashAndBurn ();
			}
		}
	}

	void CashAndBurn() {
		AudioSource aud = GetComponent<AudioSource>();
		aud.Stop();
		aud.Play();

		GameManager.instance.PlayerCurrencyTransaction (worthCurrency);
		this.GeneratePopUpNumber("$" + worthCurrency, Color.yellow, true);

	    SpriteRenderer renderer = this.gameObject.GetComponent<SpriteRenderer>();
	    renderer.enabled = false;

	    var script = this.gameObject.GetComponent<TimedDestruction>();
	    script.enabled = false;

		Destroy (gameObject, aud.clip.length);
	}

	protected void GeneratePopUpNumber(string txt, Color txtCol, bool largeText)
	{
		GameObject pop = Instantiate(popNums) as GameObject;
		pop.transform.position = transform.position;
		pop.GetComponent<Text>().text = txt;
		pop.GetComponent<Text>().color = txtCol;
		if (largeText) pop.GetComponent<Text>().fontSize += 10;
	}
}
