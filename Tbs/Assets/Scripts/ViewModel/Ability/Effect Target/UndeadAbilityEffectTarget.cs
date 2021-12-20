using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Returns 
public class UndeadAbilityEffectTarget : AbilityEffectTarget
{

    // If true needs Undead component to take effect
    // If false Undead component is eqempt from effect. 
    public bool toggle;

    public override bool IsTarget(Tile tile)
    {
        // No tile or nothing on the tile
        if (tile == null || tile.m_content == null)
            return false;
        
        bool hasComponent = tile.m_content.GetComponent<Undead>() != null;
        // If you have stats and 
        if (hasComponent != toggle)
            return false;

        Stats s = tile.m_content.GetComponent<Stats>();
        // true if stats exists and hp above 0
        return s != null && s[StatTypes.HP] > 0;
    }
}
