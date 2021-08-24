using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class MapLoader : MonoBehaviour
{

    // Tile Data
    [System.Serializable]
    public class Tile
    {
        // Main
        public string Id;
        public string Type;

        // Pos and size
        public float X;
        public float Y;
        public float Width;
        public float Height;
    }

    // TileList Data
    [System.Serializable]
    public class TileList
    {
        public Tile[] List;
    }

    // Awake
    public static MapLoader i;
    void Awake()
    {
        i = this;
    }

    // Start
    public TileList tileList = new TileList();
    void Start()
    {
        TileList tileList = Load(mapName);
            Build(tileList);
    }

    // Load
    public string mapName = "jungleCastle";
    public TileList Load(string name)
    {
        var jsonFile = Resources.Load<TextAsset>("Maps/"+name+"/testing_views_settings_normal_level");
        if (jsonFile)
        {
            tileList = JsonUtility.FromJson<TileList>(jsonFile.text);

            print("Loaded "+name+" map!");

            return tileList;
        }
        else
        {
            print("Couldn't find "+name+" map!");

            return null;
        }
    }

    // Build
    public void Build(TileList tileList)
    {
        GameObject mapHolder = GameObject.Find("Map Holder");
        foreach (var tile in tileList.List)
        {
            // Get
            GameObject mapTile = Resources.Load<GameObject>("Prefabs/Map Tile (UI)");

            // Create
            GameObject newTile = Instantiate(mapTile);
                RectTransform rectTransform = newTile.GetComponent<RectTransform>();

                // SetPos
                rectTransform.SetParent(mapHolder.transform, false);
                rectTransform.sizeDelta = new Vector2(tile.Width, tile.Height);
                rectTransform.localPosition = new Vector3(tile.X + tile.Width, tile.Y + tile.Height, 0f);

                // Set Image
                Image tileImage = newTile.GetComponent<Image>();
                    tileImage.sprite = Resources.Load<Sprite>("Maps/"+mapName+"/"+tile.Id);
        }

        print("Map is succesfully built!");
    }

    /*
    // Build
    public void Build(TileList tileList)
    {
        GameObject mapHolder = GameObject.Find("Map");
        foreach (var tile in tileList.List)
        {
            // Get
            GameObject mapTile = Resources.Load<GameObject>("Prefabs/Map Tile");

            // Create
            GameObject newTile = Instantiate(mapTile);

                // SetPos
                newTile.transform.SetParent(mapHolder.transform, false);
                //newTile.transform.sizeDelta = new Vector2(tile.Width, tile.Height);
                //newTile.transform.localScale = new Vector3(tile.Width, tile.Height, 1f);
                newTile.transform.localPosition = new Vector3(tile.X + tile.Width, tile.Y + tile.Height, 0f);

                // Set Image
                SpriteRenderer tileSprite = newTile.GetComponent<SpriteRenderer>();
                    tileSprite.sprite = Resources.Load<Sprite>("Maps/"+mapName+"/"+tile.Id);
        }

        print("Map is succesfully built!");
    }
    */
}
