using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAbilityEffect : BaseAbilityEffect
{

    public override int Predict(Tile target)
    {
        // Abilitys are attached to units.
        Unit attacker = GetComponentInParent<Unit>();
        // To reach this point we would have determined
        // something is on the tile
        Unit defender = target.m_content.GetComponent<Unit>();

        // Values shouldn't be 0.
        int attack = GetStat(attacker, defender, GetAttackNotification, 0);
        int defence = GetStat(attacker, defender, GetDefenceNotificaiton, 0);

        // Defence appliaction.
        // Could consider adjusting.
        int damage = attack - (defence / 2);
        // Keep value to 1 as other modifiers will multiply.
        damage = Mathf.Max(damage, 1);

        int power = GetStat(attacker, defender, GetPowerNotification, 0);
        
        // Power avoids defence.
        damage = power * damage / 100;
        damage = Mathf.Max(damage, 1);

        // Crit cals, elemental damages. More situational effects.
        damage = GetStat(attacker, defender, TweakDamageNotification, damage);

        // Make sure it does not exceed min or max ranges
        // May want to consider adding a modifiyer here to uncap it.
        damage = Mathf.Clamp(damage, minDamage, maxDamage);
        return damage;
    }

    protected override int OnApply(Tile target)
    {
        // Get the target unit again.
    
        Unit defender = target.m_content.GetComponent<Unit>();
        // Predict - Since the calculation is based off of the
        // prediction
        int value = Predict(target);
    
        // Modify the predicted value a little.
        value = Mathf.FloorToInt(value * UnityEngine.Random.Range(0.9f, 1.1f));

        value = Mathf.Clamp(value, minDamage, maxDamage);

        Stats s = defender.GetComponent<Stats>();
        s[StatTypes.HP] -= value;
        return value;
    }
}