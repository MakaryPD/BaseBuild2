using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Initializes world. 
/// </summary>
public class WorldController : MonoBehaviour
{ 
    public static WorldController Instance { get; protected set; }

    Dictionary<Tile, GameObject> tileGameObjectMap; 
    public World world { get; protected set; }
    [SerializeField] Sprite floor_sprite;

    private void Awake()
    {
        Instance = this; 
    }

    // Start is called before the first frame update
    void Start()
    {
        world = new World();

        tileGameObjectMap = new Dictionary<Tile, GameObject>();

        GameObject tileParent = CreateTileParentGameObject(); 
        //Create a gameObject foreach Tile, so they have a visual representation. 
        for (int x = 0; x < world.Width; x++)
        {
            for (int y = 0; y < world.Height; y++)
            {
                GameObject tileGameObject = CreateTileGameObject(x,y);
                tileGameObject.transform.SetParent(tileParent.transform, true); 
            }
        }

        world.RandomizeTiles(); 
    }

    /// <summary>
    /// Creates an empty gameObject that will hold all the tiles in the inspector. 
    /// </summary>
    public GameObject CreateTileParentGameObject()
    {
        GameObject tileParent = new GameObject();
        tileParent.name = "Tile Parent";

        return tileParent; 
    }

    public GameObject CreateTileGameObject(int x, int y)
    {
        GameObject tileGameObject = new GameObject();
        tileGameObject.name = "Tile_" + x + "_" + y;
        tileGameObject.AddComponent<SpriteRenderer>();

        Tile tileData = world.GetTileAt(x, y);
        tileGameObject.transform.position = new Vector3(tileData.X, tileData.Y, 0);

        // With lambda we can pass a function with more parameters. 
        if (tileData != null)
        {
            tileData.RegisterActionOnTileTypeChanged(OnTileTypeChanged); 
        }

        tileGameObjectMap.Add(tileData, tileGameObject);

        return tileGameObject;
    } 

    // Update is called once per frame
    void Update()
    {

    }

    float randomizeTimer = 2f; 
    void RandomizeTiles()
    {
        randomizeTimer -= Time.deltaTime; 

        if(randomizeTimer < 0)
        {
            world.RandomizeTiles();
            randomizeTimer = 2f; 
        }
    }

    void OnTileTypeChanged(Tile tileData)
    {
        if (!tileGameObjectMap.ContainsKey(tileData))
        {
            Debug.LogError("No tileData " + tileData.X + " " + tileData.Y + " in Dictionary!");
            return;
        }

        if(tileGameObjectMap[tileData] == null)
        {
            Debug.LogError("No gameObject representation for this tileData.");
            return; 
        }

        if(tileData.Type == TILE_TYPE.FLOOR)
        {
            tileGameObjectMap[tileData].GetComponent<SpriteRenderer>().sprite = floor_sprite; 
        }
        else if(tileData.Type == TILE_TYPE.EMPTY)
        {
            tileGameObjectMap[tileData].GetComponent<SpriteRenderer>().sprite = null;
        }
        else
        {
            Debug.LogError("Error - Unrecognized tile type!"); 
        }
    }
    public Tile GetTileAtWorldCoord(Vector3 coord)
    {
        int x = Mathf.RoundToInt(coord.x);
        int y = Mathf.RoundToInt(coord.y);

        return world.GetTileAt(x, y);
    }
}
