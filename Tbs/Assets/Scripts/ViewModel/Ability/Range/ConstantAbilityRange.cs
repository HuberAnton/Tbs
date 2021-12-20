using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Ability range that is even in all directions
// based on the horizontal and vertical ranges.
public class ConstantAbilityRange : AbilityRange
{
    // Might want to add an offset to this class version specifically.
    // Used when you want to skip closer tiles.
    public int offset = 0;

    public override List<Tile> GetTilesInRange(Board board)
    {
        return board.Search(unit.m_tile, offset, ExpandSearch);
    }

    // I was wondering how this fulfills arguments in the boards
    // search. Turns out the return is the 3rd argument.
    // This is passed into the search function to be used as the search.
    bool ExpandSearch(Tile from, Tile to)
    {
        // Check if the tile 1 further away can be checked
        return (from.m_distance + 1) <= horizontal && 
            // Checks if the range to the next tile is within range.
            Mathf.Abs(to.m_height - unit.m_tile.m_height) <= vertical;
    }
}
