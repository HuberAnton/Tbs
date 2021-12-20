using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // Will probably be tied to maps.
    public const float m_stepHeight = 0.25f;
    public Point m_pos;
    public int m_height;

    // what is on the tile currently.
    public GameObject m_content;

    // Pathfinding related.
    [HideInInspector]
    public Tile m_previous;
    [HideInInspector]
    public int m_distance;

    // I can probably add something here for layers.
    // Eg tile layer 0 so touches the bottom
    // Tile layer 1 sits above 0 with some kind of offest.

    // I need to practice with properties.
    public Vector3 m_center
    {
        get
        {
            return new Vector3(m_pos.m_x, m_height * m_stepHeight, m_pos.m_y);
        }
    }

    // Matches the tile(view) to the point(model)
    void Match()
    {
        transform.localPosition = new Vector3(m_pos.m_x, m_height * m_stepHeight / 2f, m_pos.m_y);
        transform.localScale = new Vector3(1, m_height * m_stepHeight, 1);
    }


    public void Grow()
    {
        m_height++;
        Match();
    }

    public void Shrink()
    {
        m_height--;
        Match();
    }

    public void Load(Point a_point, int a_height)
    {
        m_pos = a_point;
        m_height = a_height;
        Match();
    }

    public void Load(Vector3 a_vector)
    {
        Load(new Point((int)a_vector.x, (int)a_vector.z), (int)a_vector.y);
    }

}
