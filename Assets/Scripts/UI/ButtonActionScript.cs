using UnityEngine;
using System.Collections;

public class ButtonActionScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LoadLevel(string levelName)
	{
		Application.LoadLevel (levelName);
	}
}
