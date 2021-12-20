using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Uses resistance instead of eva.
public class STypeHitRate : HitRate
{
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

        if (AutomaticHit(attacker, target))
            return Final(0);

        int res = GetResistance(target);
        res = AdjustForRelativeFacing(attacker, target, res);
        res = AdjustForStatusEffects(attacker, target, res);
        res = Mathf.Clamp(res, 0, 100);

        return Final(res);
    }

    int GetResistance(Unit target)
    {
        Stats s = GetComponentInParent<Stats>();
        return s[StatTypes.RES];
    }

    int AdjustForRelativeFacing(Unit attacker, Unit target, int rate)
    {
        switch(attacker.GetFacing(target))
        {
            case Facings.Front:
                return rate;
            case Facings.Side:
                return rate - 10;
            default:
                return rate - 20;
        }
    }

}
