using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/* Points in space on the grid.
 * Using Model View Controller (MVC)
 * This is a Model.
 */

// Good utility information here.

[System.Serializable]
public class Point : IEquatable<Point>
{
    public int m_x;
    public int m_y;

    public Point(int a_x, int a_y)
    {
        m_x = a_x;
        m_y = a_y;
    }

    // Overloads

    public static Point operator +(Point lhs, Point rhs)
    {
        return new Point(lhs.m_x + rhs.m_x, lhs.m_y + rhs.m_y);
    }

    public static Point operator -(Point lhs, Point rhs)
    {
        return new Point(lhs.m_x - rhs.m_x, lhs.m_y - rhs.m_y);
    }

    public static bool operator ==(Point lhs, Point rhs)
    {
        return lhs.m_x == rhs.m_x && lhs.m_y == rhs.m_y;
    }
    // I like this. It's cheeky.
    public static bool operator !=(Point lhs, Point rhs)
    {
        return !(lhs == rhs);
    }

    public override bool Equals(object obj)
    {
        if(obj is Point)
        {
            Point p = (Point)obj;
            return m_x == p.m_x && m_y == p.m_y;
        }
        return false;
    }

    public bool Equals(Point p)
    {
        return m_x == p.m_x && m_y == p.m_y;
    }

    public override int GetHashCode()
    {
        return m_x ^ m_y;
    }

    public override string ToString()
    {
        return string.Format("({0},{1})", m_x, m_y);
    }

    // First time seeing this.
    // Allows casting and conversions.
    // Note that there is also explicict casting
    public static implicit operator Vector2(Point p)
    {
        return new Vector2(p.m_x, p.m_y);
    }

}