using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	
	public static GameManager instance = null;

	// Use this for initialization
	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
		
		// makes it persist between scenes
		DontDestroyOnLoad (gameObject);

		InitGame ();
	}
	
	private void OnLevelWasLoaded(int index)
	{
	}
	
	void InitGame()
	{
		Debug.Log ("init game called!");
	}
	
	public void GameOver()
	{
	}
	
	// Update is called once per frame
	void Update () {
	}
	
}
