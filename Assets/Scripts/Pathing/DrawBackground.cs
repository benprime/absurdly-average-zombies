using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawBackground : MonoBehaviour {
    //public string mapNum;
    //public int columns = 20;        //tile width of map
    //public int rows = 12;           //tile height of map 
    //
    //private Transform mapHolder;
    //private List<Vector3> gridPositions = new List<Vector3>();   //A list of possible locations to place tiles.
    //
    //
    //// Use this for initialization
    //void Start () {
    //    //Instantiate map.
    //    mapHolder = new GameObject("Map").transform;
    //
    //    //Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
    //    for (int x = -1; x < columns + 1; x++)
    //    {
    //        //Loop along y axis, starting from -1 to place floor or outerwall tiles.
    //        for (int y = -1; y < rows + 1; y++)
    //        {
    //            //Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
    //            GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
    //
    //            //Check if we current position is at board edge, if so choose a random outer wall prefab from our array of outer wall tiles.
    //            if (x == -1 || x == columns || y == -1 || y == rows)
    //                toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
    //
    //            //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
    //            GameObject instance =
    //                Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
    //
    //            //Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
    //            instance.transform.SetParent(mapHolder);
    //        }
    //    }
    //}
    //
    //void LoadMap()
    //{
    //    string filePath = "Assets/ArtAssets/Level" + mapNum + ".json";
    //
    //    TextAsset file = Resources.Load(filePath) as TextAsset;
    //}
	//
	//// Update is called once per frame
	//void Update () {
	//
	//}
}
