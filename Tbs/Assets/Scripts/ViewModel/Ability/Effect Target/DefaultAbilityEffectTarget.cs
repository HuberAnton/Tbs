using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Default effect target.
// Returns a unit if it is alive.
public class DefaultAbilityEffectTarget : AbilityEffectTarget
{
    public override bool IsTarget(Tile tile)
    {
        if (tile == null || tile.m_content == null)
            return false;

        Stats s = tile.m_content.GetComponent<Stats>();
        return s != null && s[StatTypes.HP] > 0;
    }
}
