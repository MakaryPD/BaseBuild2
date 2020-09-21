using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class World
{
    Tile[,] tiles;
    protected int width;
    protected int height;

    Dictionary<string, InstalledObject> intalledObjectPrototypes; 

    public int Width { get => width; }
    public int Height { get => width; }

public World(int width = 100, int height = 100)
    {
        this.width = width;
        this.height = height;
        tiles = new Tile[width, height];
        
        CreateInstalledObjectPrototypes(); 

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                tiles[x,y] = new Tile(this, x, y);
            }
        }

        Debug.Log("Created world with " + (width * height) + " tiles");
    }

    void CreateInstalledObjectPrototypes()
    {
        intalledObjectPrototypes = new Dictionary<string, InstalledObject>();

        intalledObjectPrototypes.Add("Wall",InstalledObject.CreatePrototype("Wall", 0, 1, 1));
    }

    /// <summary>
    /// Creates random types of floors. 
    /// </summary>
    public void RandomizeTiles()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if(Random.Range(0,2) == 0)
                {
                    tiles[x,y].Type = TILE_TYPE.EMPTY;
                }
                else
                {
                    tiles[x, y].Type = TILE_TYPE.FLOOR;
                }
            }
        }
    }

    public Tile GetTileAt(int x, int y)
    {
        if( x > width || x < 0)
        {
            Debug.LogError("Tile (" + x + "," + y + ") is out of range");
            return null; 
        }
        if (y > height || y < 0)
        {
            Debug.LogError("Tile (" + x + "," + y + ") is out of range");
            return null;
        }
        return tiles[x,y]; 
    }
}
