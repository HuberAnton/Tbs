using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DirectionsExtensions
{
    public static Directions GetDirection(this Tile t1, Tile t2)
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
        // Adjust for camera.
        p.GetAdjustedPoint();
        // Get the current forward of camera.
        // Forward direction should == right on keyboard.
        //int forward = (int)CameraRig.Forward;

        int newDireciton;
        // Current facing direciton
        if (p.m_y > 0)
            newDireciton = (int)Directions.North;
        else if (p.m_x > 0)
            newDireciton = (int)Directions.East;
        else if (p.m_y < 0)
            newDireciton = (int)Directions.South;
        else
            newDireciton = (int)Directions.West;

        //newDireciton += forward;

        //if (newDireciton + forward > 3)
        //    newDireciton -= 4;
        //else if (newDireciton + forward < 0)
        //    newDireciton += 4;

        return (Directions)newDireciton;
    }

    // Adjsut the point based on camera movement.
    // Point will be refering default coords. 
    // So 'rotate' the direction clockwise.
    // if north no change.
    // if south inverse direcitons.
    // if east x+ == y- == x- == y+
    // if west x+ == y+ == x- == y-
    public static Point GetAdjustedPoint(this Point p)
    {
        // Point x and y should be between -1 and 1
        Directions forward = CameraRig.Forward;
        if (forward == Directions.South)
            return -p;
        else if (forward == Directions.East)
        {
            int x = p.m_x, y = p.m_y;

            p.m_x = (int)(x * Mathf.Cos(90 * Mathf.Deg2Rad) - y * Mathf.Sin(90 * Mathf.Deg2Rad));
            p.m_y = (int)(x * Mathf.Sin(90 * Mathf.Deg2Rad) + y * Mathf.Cos(90 * Mathf.Deg2Rad));
            return p;
        }
        else if (forward == Directions.West)
        {
            int x = p.m_x, y = p.m_y;

            p.m_x = (int)(x * Mathf.Cos(-90 * Mathf.Deg2Rad) - y * Mathf.Sin(-90 * Mathf.Deg2Rad));
            p.m_y = (int)(x * Mathf.Sin(-90 * Mathf.Deg2Rad) + y * Mathf.Cos(-90 * Mathf.Deg2Rad));
            return p;
        }

        //North - no change
        return p;
    }



    public static Point ApplyCameraDirection(this Point point)
    {
        int direciton = (int)CameraRig.Forward;
        return point;
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
