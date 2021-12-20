using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Is an always hit case.
// Allows for automiss still.
public class FullTypeHitRate : HitRate
{
    public override bool IsAngleBased { get { return false; } }

    public override int Calculate(Tile tileTarget)
    {
        // Should be attached to a unit.
        Unit attacker = this.GetComponentInParent<Unit>();
        Unit target = tileTarget.m_content.GetComponent<Unit>();

        // Implies if not a unit autoHit
        if (!target)
            return Final(0);


        if (AutomaticMiss(attacker, target))
            return Final(100);

        return Final(0);
    }
}
