using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;


public enum TILE_TYPE { EMPTY, FLOOR };
/// <summary>
/// Desc: Tile is a basic floor object. 
/// </summary>
public class Tile
{ 
    TILE_TYPE type = TILE_TYPE.EMPTY;

    public TILE_TYPE Type
    { 
        get => type; 
        set{
            TILE_TYPE old = type; 
            type = value;
            if (value != old)
            {
                cbOnTileTypeChange?.Invoke(this);
            }
        }
    }

    LooseObject looseObject;
    InstalledObject installendObject;

    World world;
    int x;
    int y;

    Action<Tile> cbOnTileTypeChange; 

    public int X { get => x; }
    public int Y { get => y; } 

    public Tile(World world, int x, int y)
    {
        this.world = world; 
        this.x = x;
        this.y = y; 
    }

    public bool PlaceObject(InstalledObject objInstance)
    {
        //Delete what was here if objInstance is null. 
        if(objInstance == null)
        {
            installendObject = null;
            return true; 
        }

        //objInstance is not null. Check if installedObject is not null
        if(installendObject != null)
        {
            Debug.LogError("This tile already has an installedObject!");
            return false; 
        }

        //At this point everything is good. 
        installendObject = objInstance;
        return true; 
    }

    public void RegisterActionOnTileTypeChanged(Action<Tile> action)
    {
        cbOnTileTypeChange += action; 
    }

    public void UnregisterActionOnTileTypeChanged(Action<Tile> action)
    {
        cbOnTileTypeChange -= action;
    }
}
