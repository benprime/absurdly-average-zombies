using System.IO;
using UnityEngine;
using SimpleJson;
using System.Collections;


public class MapLoader : MonoBehaviour
{
    public string mapFileName;
    public string tilesetFileName;

    const uint FLIPPED_HORIZONTALLY_FLAG = 0x80000000;
    const uint FLIPPED_VERTICALLY_FLAG = 0x40000000;
    const uint FLIPPED_DIAGONALLY_FLAG = 0x20000000;

    [HideInInspector]
    public Sprite[] tiles;

    // Use this for initialization
    void Start()
    {

		GameObject map = GameObject.Find("Map");
		GameObject mapTiles = new GameObject();

        // TODO: Maybe someday make this loader take other map formats into account.
        // for now, we'll make some basic assumptions (single layer 2d orthogonal)

        // Parse map file
        // -----------------------------------------------------
        TextAsset ta = Resources.Load<TextAsset>(this.mapFileName);
        SimpleJSON.JSONNode rootNode = SimpleJSON.JSON.Parse(ta.text);

        // note: the layers also have their own width and height.
        // we are assuming they will always be the same.
        int widthInTiles = int.Parse(rootNode["width"]);
        int heightInTiles = int.Parse(rootNode["height"]);
        int TileWidth = int.Parse(rootNode["tilewidth"]);
        int TileHeight = int.Parse(rootNode["tileheight"]);


        // Load tilesets (assuming just one for now)
        // -----------------------------------------------------
        int tilesetTileCount = int.Parse(rootNode["tilesets"][0]["tilecount"]);
        int tilesetTileWidth = int.Parse(rootNode["tilesets"][0]["tilewidth"]);
        int tilesetTileHeight = int.Parse(rootNode["tilesets"][0]["tileheight"]);
        int tilesetImageWidth = int.Parse(rootNode["tilesets"][0]["imagewidth"]);
        int tilesetImageHeight = int.Parse(rootNode["tilesets"][0]["imageheight"]);

        // get the number of rows/cols in the tileset
        int colWidth = tilesetImageWidth / tilesetTileWidth;
        int maxRow = (tilesetImageHeight / tilesetTileHeight) - 1;

        string imageFilename = Path.GetFileNameWithoutExtension(rootNode["tilesets"][0]["image"]);
        
        Texture2D tileset = (Texture2D)Resources.Load(tilesetFileName);
        this.tiles = new Sprite[tilesetTileCount];

        // create a sprite array on the fly
        for (int i = 0; i < tilesetTileCount; i++)
        {
            int x = (i % colWidth) * tilesetTileWidth;
            int y = (maxRow - (i / colWidth)) * tilesetTileHeight;

            Rect srcRect = new Rect(x, y, tilesetTileWidth, tilesetTileHeight);
            this.tiles[i] = Sprite.Create(tileset, srcRect, new Vector2(0.5f, 0.5f), TileWidth);

        }

        int dataIndex = 0;

        Vector3 mapSize = GetComponent<Renderer>().bounds.size;

        // a little hacky, but lines up the maps with old method (to avoid having to update all the maps)
        float y_adjust = (mapSize.y / 2) + .48f;// .28 -.03
        float x_adjust = (mapSize.x / 2) + .06f;//-.16  .18

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
                GameObject tile = new GameObject("Tile");// tiles[gid - 1];
				tile.transform.SetParent(mapTiles.transform);
                tile.transform.position = new Vector3(x - x_adjust, y - y_adjust);
                tile.layer = 8; // terrain layer
                SpriteRenderer sr = tile.AddComponent<SpriteRenderer>();
                sr.sortingLayerName = "Background";
				sr.sprite = tiles[gid-1];


                // handle the rotations and flipping
                Vector3 scale = tile.transform.localScale;

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
                    scale.y *= -1;

                    // TODO: simplify this logic once it is properly understood... WTF anyway
                    // if both x and y scales are -1 or both are 1, then the rotation must be -270 (or 90)
                    if (scale.x == scale.y)
                    {
                        tile.transform.Rotate(0, 0, -270); // same as 90
                    }
                    else
                    {
                        // if one is -1 and the other is 1, we rotate 270... brain too tired to think of why the hell this is true
                        // this was discovered by literally trying every combination of numers... I feel like I don't know math anymore.
                        tile.transform.Rotate(0, 0, 270);
                    }
				}
				tile.transform.localScale = scale;
            }
		}
		Vector3 offScale = new Vector3 (.96f, .96f, 1f);
		mapTiles.transform.localScale = offScale;
		mapTiles.transform.SetParent(map.transform);


		//Vector3 tempVec = map.transform.localScale;
		//tempVec.x *= .98f;
		//tempVec.y *= .98f;
		//map.transform.localScale = tempVec;
    }

    // Update is called once per frame
    void Update()
	{

    }
}
