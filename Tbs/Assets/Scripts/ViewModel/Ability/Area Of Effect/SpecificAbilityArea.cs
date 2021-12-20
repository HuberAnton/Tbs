using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecificAbilityArea : AbilityArea
{
    public int horizontal;
    public int vertical;
    Tile tile;

    public override List<Tile> GetTilesInArea(Board board, Point pos)
    {
        tile = board.GetTile(pos);
        return board.Search(tile, ExpandSearch);
    }


    bool ExpandSearch(Tile from, Tile to)
    {
        // Very similar to ability range checks for both distance and height.
        return (from.m_distance + 1) <= horizontal && Mathf.Abs(to.m_height - tile.m_height) <= vertical;
    }
}
