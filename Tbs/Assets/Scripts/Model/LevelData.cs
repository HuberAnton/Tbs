using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Stores position and height of all tiles in the level.
// I can't beleive how much I overcomplicated my old version.

// Potentially additional things to include here**************
// Note that this will need to store an atlas of the tiles
// that need to be created.
public class LevelData : ScriptableObject
{
    public List<Vector3> m_tiles;
}
