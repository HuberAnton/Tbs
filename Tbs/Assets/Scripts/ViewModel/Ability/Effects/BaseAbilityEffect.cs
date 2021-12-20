using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ability base class.
// For applying effects, healing and damage.
public abstract class BaseAbilityEffect : MonoBehaviour
{

    protected const int minDamage = -999;
    protected const int maxDamage = 999;

    public const string GetAttackNotification =
    "BaseAbilityEffect.GetAttackNotification";

    public const string GetDefenceNotificaiton =
        "BaseAbilityEffect.GetDefenceNotification";

    public const string GetPowerNotification =
        "BaseAbilityEffect.GetPowerNotification";

    public const string TweakDamageNotification =
        "BaseAbilityEffect.TweakDamageNotification";

    public const string MissedNotificaiton =
        "BaseAbilityEffect.MissedNotificaiton";

    public const string HitNotification =
        "BaseAbilityEffect.HitNotificaiton";


    // For use with ui.
    // Allows for estimates damage/effects.
    public abstract int Predict(Tile target);
    protected abstract int OnApply(Tile target);

    public void Apply(Tile target)
    {
        if (GetComponent<AbilityEffectTarget>().IsTarget(target) == false)
            return;

        if (GetComponent<HitRate>().RollForHit(target))
            this.PostNotification(HitNotification, OnApply(target));
        else
            this.PostNotification(MissedNotificaiton);
    }

    // Gets modified value of stats.
    // Named badly.
    // attack, defence, power and tweaks.
    protected virtual int GetStat(Unit attacker, Unit target, string notification, int startValue)
    {
        // List of mods for the stat.
        var mods = new List<ValueModifier>();
        // Creates an info class to cycle through both
        // the attacker and defenders valueModifiers and stores them in mods.
        var info = new Info<Unit, Unit, List<ValueModifier>>(attacker, target, mods);
        // Populate list with everything attached to this 
        this.PostNotification(notification, info);
        // Sorts the now populated list by modifier priority.
        mods.Sort(Compare);
        // Start value of stat.
        float value = startValue;

        // Loop through and adjsut stat.
        for (int i = 0; i < mods.Count; ++i)
        {
            value = mods[i].Modify(startValue, value);
        }
        // Round down
        int retValue = Mathf.FloorToInt(value);
        // Make sure it does not exceed min or max ranges
        // May want to consider adding a modifiyer here to uncap it.
        retValue = Mathf.Clamp(retValue, minDamage, maxDamage);
        return retValue;
    }

    // For use with sort.
    // How the list is compared.
    int Compare(ValueModifier x, ValueModifier y)
    {
        return x.sortOrder.CompareTo(y.sortOrder);
    }

}
