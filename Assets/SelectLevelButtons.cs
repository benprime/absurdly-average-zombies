﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class SelectLevelButtons : MonoBehaviour {

	// little bit hacky, but it'll work for now
	int firstLevelIndex = 2;

	public void LoadLevel(string levelName)
	{
		Application.LoadLevel (levelName);
	}
	
	// Use this for initialization
	void Start () {
		// level 1 is index 2
		Button[] buttons = FindObjectsOfType<Button> ();
		
		// still more hackery
		for (int i = 2; i < Application.levelCount; i++) {
			string levelName = "Level" + (i - 1);
			string buttonName = levelName + "Button";

			Button button = (Button)GameObject.Find(buttonName).GetComponent<Button>();

			button.enabled = !GameManager.instance.progressManager.IsLevelLocked(levelName);
		}

		/*
		foreach (Button b in buttons) {
			if(GameManager.instance.progressManager.IsLevelLocked(t.text))
			{
				b.enabled = false;
			}
			else
			{
				b.enabled = true;
			}
		}
		*/
		
		Debug.Log ("Enabling buttons!");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
