using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ability range that only targets itself.
// Self heal or buff?
// Needs work if multiple tile units exist.
public class SelfAbilityRange : AbilityRange
{
    public override bool PositionOriented { get { return false; } }

    public override List<Tile> GetTilesInRange(Board board)
    {
        // You would get that value from the uni.
        List<Tile> retValue = new List<Tile>(1);
        // If a unit occupied multiple tiles
        // They would all be added here.
        retValue.Add(unit.m_tile);
        return retValue;
    }
 
}
