using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeAbilityRange : AbilityRange
{
    // Units direction
    public override bool directionOrientation { get { return true; } }

    public override List<Tile> GetTilesInRange(Board board)
    {
        // Unit tile position
        Point pos = unit.m_tile.m_pos;
        List<Tile> retValue = new List<Tile>();
        // North and east are positive while south and west are negative
        int dir = (unit.m_direction == Directions.North || unit.m_direction == Directions.East) ? 1 : -1;
        // How far away from start pos.
        int lateral = 1;

        // When facing is north or south.
        if(unit.m_direction == Directions.North || unit.m_direction == Directions.South)
        {
            for (int y = 1; y <= horizontal; ++y)
            {
                // Min and max movement for each step outward.
                int min = -(lateral / 2);
                int max = (lateral / 2);

                // Will then steo keft and right of the y position
                // until horizontal limit hit
                for (int x = min; x <= max; ++x)
                {
                    // Get a new point in facing direction of unit. * dir is just for positive or negative direction.
                    Point next = new Point(pos.m_x + x, pos.m_y + (y * dir));
                    Tile tile = board.GetTile(next);
                    // Checks and adds tile if it exists.
                    if (ValidTile(tile))
                        retValue.Add(tile);
                }
                lateral += 2;
            }
        }
        // When facing is east or west.
        // Same as above but multiplys facing by x instead of y.
        else
        {
            for (int x = 1; x <= horizontal; ++x)
            {
                int min = -(lateral / 2);
                int max = (lateral / 2);

                for (int y = min; y <= max; ++y)
                {
                    Point next = new Point(pos.m_x + (x * dir), pos.m_y + y);
                    Tile tile = board.GetTile(next);
                    if (ValidTile(tile))
                        retValue.Add(tile);
                }
                lateral += 2;
            }
        }
        return retValue;
    }

    bool ValidTile(Tile t)
    {
        // If tile exists and within vertical range tile is valid.
        return t != null && Mathf.Abs(t.m_height - unit.m_tile.m_height) <= vertical;
    }

}
