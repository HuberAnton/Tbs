using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ability range that stretches the length of the board
// in a line in all directions.
public class LineAbilityRange : AbilityRange
{
    // Does not work quite correctly.
    // Adds the character casting it to the list 
    // of affected tiles.
    public override DirectionOreinationMode directionOrientation { get { return DirectionOreinationMode.RotationOnly; } }

    public override List<Tile> GetTilesInRange(Board board)
    {
        // Unit posiition is start
        // For some reason this ->
        // Point startPos unit.m_tile.m_pos causes issue
        Point startPos = new Point(unit.m_tile.m_pos.m_x, unit.m_tile.m_pos.m_y);
        Point endPos;
        List<Tile> retValue = new List<Tile>();
        // Used for the length of the lines.
        int displacement;
        // Get the end point based on which direction the unit is facing.
        switch(unit.m_direction)
        {
            case Directions.North:
                // Checks if the line desired is outside of the boards range.
                displacement = (startPos.m_y + horizontal < board.max.m_y) ? startPos.m_y + horizontal : board.max.m_y;
                //if (startPos.m_y + horizontal < board.max.m_y)
                //    endPos = new Point(startPos.m_x, startPos.m_y + horizontal);
                //else
                //    endPos = new Point(startPos.m_x, board.max.m_y);
                endPos = new Point(startPos.m_x, displacement);
                break;
            case Directions.East:
                displacement = (startPos.m_x + horizontal < board.max.m_x) ? startPos.m_x + horizontal : board.max.m_x;
                endPos = new Point(displacement, startPos.m_y);
                break;
            case Directions.South:
                displacement = (startPos.m_y - horizontal > board.min.m_y) ? startPos.m_y - horizontal : board.min.m_y;
                endPos = new Point(startPos.m_x, displacement);
                break;
            default: // West
                displacement = (startPos.m_x - horizontal > board.min.m_x) ? startPos.m_x - horizontal : board.min.m_x;
                endPos = new Point(displacement, startPos.m_y);
                break;
        }

        //// So line does not include starting position.
        //if (startPos.m_x < endPos.m_x)
        //    startPos.m_x++;
        //else if (startPos.m_x > endPos.m_x)
        //    startPos.m_x--;

        int dist = 0;
        while (startPos != endPos)
        {
            // Moves towards end posistion on x axis.
            if (startPos.m_x < endPos.m_x)
                startPos.m_x++;
            else if(startPos.m_x > endPos.m_x) 
                startPos.m_x--;

            // Moves towards end position on y axis.
            if (startPos.m_y < endPos.m_y)
                startPos.m_y++;
            else if (startPos.m_y > endPos.m_y)
                startPos.m_y--;

            // Start pos is now adjusted as above
            // so you will get the next tile in that direction.
            Tile t = board.GetTile(startPos);
            // Check to see if within height range
            if (t != null && Mathf.Abs(t.m_height - unit.m_tile.m_height) <= vertical)
                retValue.Add(t);

            dist++;
            if (dist >= horizontal)
                break;
        }

        return retValue;
    }
}
