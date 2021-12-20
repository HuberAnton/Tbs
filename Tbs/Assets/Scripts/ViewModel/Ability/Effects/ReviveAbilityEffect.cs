using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Basic revival spell.
// Stats have no impact so probably good
// to be used with items.
public class ReviveAbilityEffect : BaseAbilityEffect
{
    // How much to recover out of MHP
    public float percent;

    // Consider experimenting with power notification.
    public override int Predict(Tile target)
    {
        Stats s = target.m_content.GetComponent<Stats>();
        return Mathf.FloorToInt(s[StatTypes.MHP] * percent);
    }

    protected override int OnApply(Tile target)
    {
        Stats s = target.m_content.GetComponent<Stats>();
        int value = s[StatTypes.HP] = Predict(target);
        return value;
    }

}
