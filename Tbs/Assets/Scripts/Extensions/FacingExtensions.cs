using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Works out facing for square grid
// Might need to do some adjustments for hexes.
public static class FacingExtensions
{
    // I'd have to make these static as well
    // and really there isn't a point
    //float frontLimit = 0.45f;
    //float backLimit = -0.45f;

    public static Facings GetFacing(this Unit attacker, Unit target)
    {
        Vector2 targetDirection = target.m_direction.GetNormal();
        // Standard direction stuff. Cast the points into vector2 using implict conversions.
        Vector2 approachDirection = ((Vector2)(target.m_tile.m_pos - attacker.m_tile.m_pos)).normalized;

        float dot = Vector2.Dot(approachDirection, targetDirection);

        // I think these rules will be alright with hexes
        // but the value ranges might need to be adjusted
        // if I want to go down that route.
        // forward is in front, then sides, then flank, then back.
        // Might be too much.
        if (dot >= 0.45f)
            return Facings.Back;
        if (dot <= -0.45f)
            return Facings.Front;
        return Facings.Side;
    }

}
