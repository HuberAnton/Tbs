using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// All data needed for creation of a level tile.
public class TileData
{
    Vector3 Position;
    // Being from water, rock grass ect.
    int TileSet;
    // Ramp, cliff, empty space, wall pattern. 
    int TileType;
    // Scripts to be added to the tile.
    // Eg Spawn point, Reinforcment point, deployment point.
    List<GameObject> Features;

    // If a unit start off here.
    GameObject Content;
}