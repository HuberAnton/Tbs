using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Single target of an ability.
// Returns the tile that the cursor is on.
// Use case singe target ranged attakcs.
public class UnitAbilityArea : AbilityArea
{
    public override List<Tile> GetTilesInArea(Board board, Point pos)
    {
        List<Tile> retValue = new List<Tile>();
        Tile tile = board.GetTile(pos);
        if (tile != null)
            retValue.Add(tile);
        return retValue;
    }
}
