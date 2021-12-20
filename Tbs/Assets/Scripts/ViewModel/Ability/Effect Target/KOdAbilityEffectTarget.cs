using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Only returns true if there is a target with no hp.
public class KOdAbilityEffectTarget : AbilityEffectTarget
{
    public override bool IsTarget(Tile tile)
    {
        if (tile == null || tile.m_content == null)
            return false;

        Stats s = tile.m_content.GetComponent<Stats>();
        return s != null && s[StatTypes.HP] <= 0;
    }
}
