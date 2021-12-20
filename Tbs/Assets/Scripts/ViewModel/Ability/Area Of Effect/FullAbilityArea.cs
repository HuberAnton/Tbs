using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// If you want the whole targetable area to be effected
// regardless of position of cursor.
// Cone or line is a good example.
public class FullAbilityArea : AbilityArea
{
    public override List<Tile> GetTilesInArea(Board board, Point pos)
    {
        AbilityRange ar = GetComponent<AbilityRange>();
        return ar.GetTilesInRange(board);
    }
}
