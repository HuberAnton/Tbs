using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// General use hit. Uses eva
// Most regular attacks will use this variation.
// Adjusts for directional facings.
public class ATypeHitRate : HitRate
{
    public override int Calculate(Tile tileTarget)
    {
        // Should be attached to a unit.
        Unit attacker = this.GetComponentInParent<Unit>();
        Unit target = tileTarget.m_content.GetComponent<Unit>();

        // Implies if not a unit autoHit
        if (!target)
            return Final(0);

        if (AutomaticHit(attacker, target))
            return Final(0);

        if (AutomaticMiss(attacker, target))
            return Final(100);

        int evade = GetEvade(target);

        evade = AdjustForRelaviteFacing(attacker, target, evade);
        evade = AdjustForStatusEffects(attacker, target, evade);
        evade = Mathf.Clamp(evade, 5, 95);
        return Final(evade);
    }

    int GetEvade(Unit target)
    {
        Stats s = target.GetComponentInParent<Stats>();
        return Mathf.Clamp(s[StatTypes.EVD], 0, 100);
    }

    int AdjustForRelaviteFacing(Unit attacker, Unit target, int rate)
    {
        switch (attacker.GetFacing(target))
        {
            case Facings.Front:
                return rate;
            case Facings.Side:
                return rate / 2;
            default:
                return rate / 4;
        }
    }
}
