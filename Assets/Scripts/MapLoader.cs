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
    [HideInInspector] public Vector3 minPos;
    [HideInInspector] public Vector3 maxPos;
    public void Build(TileList tileList)
    {
        // Get
        GameObject mapHolder = GameObject.Find("Map Holder");

        // Build map
        int num = 0;
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

                // SetImage
                Image tileImage = newTile.GetComponent<Image>();
                    tileImage.sprite = Resources.Load<Sprite>("Maps/"+mapName+"/"+tile.Id);

            // Get out cam boundries
            if (num == 0) minPos = rectTransform.localPosition + new Vector3(-0.5f * tile.Width, 0.5f * tile.Height, 1.0f);
            if (num == (tileList.List.Length -1)) maxPos = rectTransform.localPosition + new Vector3(0.5f * tile.Width, -0.5f * tile.Height, 1.0f);

            // Increasing our number
            num += 1;
        }

        // Move our camera
        CameraController.i.Move(new Vector3(0f, 0f, 0f));
    }
}
