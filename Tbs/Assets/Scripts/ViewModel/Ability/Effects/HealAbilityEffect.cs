using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Heal ability effect.
// Basic and modified only by power of ability.
public class HealAbilityEffect : BaseAbilityEffect
{
    public override int Predict(Tile target)
    {
        Unit attacker = GetComponentInParent<Unit>();
        Unit defender = target.m_content.GetComponent<Unit>();
        // Note that this is a rather simple heal algorithm
        // If you want it to be affected by more than jsut power
        // add them here.
        return GetStat(attacker, defender, GetPowerNotification, 0);
    }
    protected override int OnApply(Tile target)
    {

        Unit defender = target.m_content.GetComponent<Unit>();
        int value = Predict(target);

        // Some variance to the heal
        value = Mathf.FloorToInt(value * UnityEngine.Random.Range(0.9f, 1.1f));
        // Clamp to max and min range
        value = Mathf.Clamp(value, minDamage, maxDamage);

        Stats s = defender.GetComponent<Stats>();
        // Appliaciton of heal
        s[StatTypes.HP] += value;
        // Passed out possibly to be used by ui.
        return value;
    }

}
