using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsorbDamageAbilityEffect : BaseAbilityEffect
{
    // Take note of this. It may cause issues.
    // If you add or remove abilities in odd spots
    // it may break.
    // Might be worth having an ability chance modifier?
    public int trackedSiblingIndex;
    BaseAbilityEffect effect;
    int amount;

    private void Awake()
    {
        effect = GetTrackedEffect();
    }

    private void OnEnable()
    {
        this.AddObserver(OnEffectHit, HitNotification, effect);
        this.AddObserver(OnEffectMiss, MissedNotificaiton, effect);
    }

    private void OnDisable()
    {
        this.RemoveObserver(OnEffectHit, HitNotification, effect);
        this.RemoveObserver(OnEffectMiss, MissedNotificaiton, effect);
    }

    protected override int OnApply(Tile target)
    {
        Stats s = GetComponentInParent<Stats>();
        s[StatTypes.HP] += amount;
        return amount;
    }

    // Not sure if this needs a value depending on the 
    // amount of damage done.
    public override int Predict(Tile target)
    {
        return 0;
    }

    void OnEffectHit(object sender, object args)
    {
        amount = (int)args;
    }

    void OnEffectMiss(object sender, object args)
    {
        amount = 0;
    }


    BaseAbilityEffect GetTrackedEffect()
    {
        Transform owner = GetComponentInParent<Ability>().transform;
        if(trackedSiblingIndex >= 0 && trackedSiblingIndex < owner.childCount)
        {
            Transform sibling = owner.GetChild(trackedSiblingIndex);
            return sibling.GetComponent<BaseAbilityEffect>();
        }
        return null;
    }

}
