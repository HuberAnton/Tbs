using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// All data needed for creation of a level tile.
[System.Serializable]
public class TileData
{
    public Vector3 m_position;
    // Being from water, rock grass ect.
    public int m_tileSet;
    // Ramp, cliff, empty space, wall pattern. 
    public int m_tileType;
    // Scripts to be added to the tile.
    // Eg Spawn point, Reinforcment point, deployment point.
    public List<GameObject> m_features;

    // If a unit start off here.
    public GameObject m_content;

    public TileData(Vector3 position, int tileSet, int tileType, List<GameObject> features)
    {
        this.m_position = position;
        this.m_tileSet = tileSet;
        this.m_tileType = tileType;
        this.m_features = features;
    }

}