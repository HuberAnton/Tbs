using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Tile m_tile { get; protected set; }
    public Directions m_direction;

    public void Place(Tile a_target)
    {
        // If on a tile and the tile is the one containing this unit
        if (m_tile != null && m_tile.m_content == this.gameObject)
        {
            // Unassigne this unit from the tile
            m_tile.m_content = null;
        }

        // Set target tile to units current tile 
        m_tile = a_target;

        if (a_target != null)
        {
            // Set the content of the tile to this unit
            a_target.m_content = this.gameObject;
        }

    }

    // Places and rotates the unit on the tiles center.
    public void Match()
    {
        transform.localPosition = m_tile.m_center;
        transform.localEulerAngles = m_direction.ToEuler();
    }

}
