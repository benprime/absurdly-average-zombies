using System.IO;
using UnityEngine;
using SimpleJson;
using System.Collections;


public class MapLoader : MonoBehaviour
{

    public string mapFileName;
    public Sprite[] tiles;

    const uint FLIPPED_HORIZONTALLY_FLAG = 0x80000000;
    const uint FLIPPED_VERTICALLY_FLAG = 0x40000000;
    const uint FLIPPED_DIAGONALLY_FLAG = 0x20000000;


    // Use this for initialization
    void Start()
    {
        GameObject map = GameObject.Find("Map");

        gameObject.transform.position = (new Vector3(map.transform.localPosition.x, map.transform.localPosition.y, 0));

        // TODO: Maybe someday make this loader take other map formats into account.
        // for now, we'll make some basic assumptions
        TextAsset ta = Resources.Load<TextAsset>(this.mapFileName);
        SimpleJSON.JSONNode rootNode = SimpleJSON.JSON.Parse(ta.text);

        // note: the layers also have their own width and height.
        // we are assuming they will always be the same.
        int widthInTiles = int.Parse(rootNode["width"]);
        int heightInTiles = int.Parse(rootNode["height"]);
        int TileWidth = int.Parse(rootNode["tilewidth"]);
        int TileHeight = int.Parse(rootNode["tileheight"]);

        // save tile size in unity units
        //Vector3 tileSize = Camera.main.ScreenToWorldPoint(new Vector3(TileWidth - 20, TileHeight - 20, 0f));

        int dataIndex = 0;

        Vector3 mapSize = GetComponent<Renderer>().bounds.size;
        float y_adjust = mapSize.y / 2;
        float x_adjust = mapSize.x / 2;

        // y = 0 has no data?
        for (int y = heightInTiles; y > 0; y--)
        {
            for (int x = 0; x < widthInTiles; x++)
            {
                string gid_string = rootNode["layers"][0]["data"][dataIndex++];
                if (string.IsNullOrEmpty(gid_string))
                {
                    Debug.Log("Missing tile data @ x: " + x.ToString() + ", y: " + y.ToString());
                    continue;
                }
                // get the gid that may contain bit shifts
                uint raw_gid = uint.Parse(gid_string);

                // check which of the flags are present
                bool flip_horizontal = (raw_gid & FLIPPED_HORIZONTALLY_FLAG) == FLIPPED_HORIZONTALLY_FLAG;
                bool flip_vertical = (raw_gid & FLIPPED_VERTICALLY_FLAG) == FLIPPED_VERTICALLY_FLAG;
                bool flip_diagonal = (raw_gid & FLIPPED_DIAGONALLY_FLAG) == FLIPPED_DIAGONALLY_FLAG;

                // remove all the flags so we just get the normal gid back
                ulong gid = raw_gid & ~(FLIPPED_HORIZONTALLY_FLAG | FLIPPED_VERTICALLY_FLAG | FLIPPED_DIAGONALLY_FLAG);


                // grab the prefab tile based on gid
                Sprite tilePrefab = tiles[gid - 1];

                GameObject instance = Instantiate(tilePrefab, new Vector3(x - x_adjust, y - y_adjust), Quaternion.identity) as GameObject;
                instance.layer = 8; // terrain layer

                // handle the rotations and flipping
                Vector3 scale = instance.transform.localScale;

                if (flip_vertical)
                {
                    scale.y *= -1;
                }

                if (flip_horizontal)
                {
                    scale.x *= -1;
                }

                if (flip_diagonal)
                {
                    instance.transform.Rotate(0, 0, 270);
                    scale.x *= -1;
                }

                instance.transform.localScale = scale;
                instance.transform.SetParent(map.transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
