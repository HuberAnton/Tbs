using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DirectionsExtensions
{
    public static Directions GetDirection (this Tile t1, Tile t2)
    {
        if (t1.m_pos.m_y < t2.m_pos.m_y)
            return Directions.North;
        if (t1.m_pos.m_x < t2.m_pos.m_x)
            return Directions.East;
        if (t1.m_pos.m_y > t2.m_pos.m_y)
            return Directions.South;

            return Directions.West;
    }

    // Alternate getting of direction from a point.
    // Used mainly with abilities.
    public static Directions GetDirections(this Point p)
    {
        if (p.m_y > 0)
            return Directions.North;
        if (p.m_x > 0)
            return Directions.East;
        if (p.m_y < 0)
            return Directions.South;
        return Directions.West;
    }

    // So it returns a direction extention? or a enum?
    // Or can this only be called by itself?
    // As in this class has to pass in a direcion?
    // It might just be syntax for the class extension
    // Since you need to call this function on the 
    // enum itself.
    public static Vector3 ToEuler(this Directions d)
    {
        return new Vector3(0, (int)d * 90, 0);
    }

    public static Point GetNormal(this Directions dir)
    {
        switch(dir)
        {
            case Directions.North:
                return new Point(0, 1);
            case Directions.East:
                return new Point(1, 0);
            case Directions.South:
                return new Point(0, -1);
            default: // West
                return new Point(-1, 0);
        }
     
    }
}
