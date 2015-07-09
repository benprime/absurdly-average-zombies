using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {
	private float timer = 0;
	public float waitTime = 2f;
	public string levelToLoad = "MainMenu";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if(timer >= waitTime) {
			Application.LoadLevel (levelToLoad);
		}
	}
}
