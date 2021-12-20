using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Ability range that can hit all tiles
// from any position. Note that
// this doesn't need to use the boards search function
// but just the tiles that make up the board.
// If some tiles were not used in gameplay sense
// you would need to do the search still.
public class InfiniteAbilityRange : AbilityRange
{
    public override bool PositionOriented { get { return false; } }

    public override List<Tile> GetTilesInRange(Board board)
    {
        return new List<Tile>(board.m_tiles.Values);
    }
}
