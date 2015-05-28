using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid_Generate : MonoBehaviour {
	public int mapWidth = 10, mapHeight = 5, gridHoriz = 2, gridVert = 2;
	private float gridSpaceHoriz, gridSpaceVert;
	public GameObject node;

	// Use this for initialization
	void Start () {
		gridSpaceHoriz = mapWidth / gridHoriz;
		gridSpaceVert = mapHeight / gridVert;
		//node = new GameObject();
		//node.name = "AutoPathNode";
		for(float x = 0; x <= mapWidth; x += gridSpaceHoriz) {
			for(float y = 0; y <= mapHeight; y += gridSpaceVert) {
				Vector3 pos = new Vector3(x + transform.position.x, y + transform.position.y, 0);
				Instantiate (node, pos, Quaternion.identity);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
