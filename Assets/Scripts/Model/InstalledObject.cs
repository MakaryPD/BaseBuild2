using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using UnityEngine;

public class InstalledObject 
{
    //Some installed objects my be bigger than a one tile. Expample: Sofa is 3 points wide and takes 3 tiles. 
    Tile tile;
    int width = 1;
    int height = 1; 

    // ID for object. 
    string objectType;

    //Movement cost is a  mulitplier. 
    //SPECIAL: IF value is 0 character cant walk on the installed object. 
    //Tiles and effects also have a movement cost that can stack. 
    float movementCost = 1;


    public Tile Tile { get => tile; set => tile = value; }

    protected InstalledObject()
    {

    }

    //Used by object factory to create prototype. This constructor doesn't ask for a tile.  
    static public InstalledObject CreatePrototype(string objectType, float movementCost, int width = 1, int height = 1)
    {
        InstalledObject prototype = new InstalledObject();

        prototype.objectType = objectType;
        prototype.movementCost = movementCost;
        prototype.width = width;
        prototype.height = height;

        return prototype; 
    }

    static public InstalledObject PlaceInstance(InstalledObject prototype, Tile tile)
    {
        InstalledObject obj = new InstalledObject();

        obj.objectType = prototype.objectType;
        obj.movementCost = prototype.movementCost;
        obj.width = prototype.width;
        obj.height = prototype.height;

        obj.tile = tile;

        // TODO: this assumes we are 1x1. 
        if (tile.PlaceObject(obj) == false)
        {
            //It will be automatically collected by GC. (Garbage Collector)
            return null; 
        }
             

        return obj; 
    }

}


