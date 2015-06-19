using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class SelectLevel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// level 1 is index 2
		Button[] buttons = FindObjectsOfType<Button> ();

		int highestCompletedLevel = GameManager.instance.HighestCompletedScene - 2;
		// level 1 is always unlocked
		if (highestCompletedLevel < 1)
			highestCompletedLevel = 1;

		foreach (Button b in buttons) {
			b.enabled = false;
		}

		// This is broken, perhaps we'll just add the buttons
		// on the fly and place them mathematically
		for (int i = 0; i < highestCompletedLevel; i++) {
			buttons[i].enabled = true;
		}

		Debug.Log ("Enabling buttons!");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
