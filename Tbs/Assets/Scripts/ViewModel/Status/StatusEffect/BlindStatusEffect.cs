using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindStatusEffect : StatusEffect
{
    private void OnEnable()
    {
        this.AddObserver(OnHitRateStatusCheck, HitRate.StatusCheckNotification);
    }

    private void OnDisable()
    {
        this.RemoveObserver(OnHitRateStatusCheck, HitRate.StatusCheckNotification);
    }

    // info == (unit)attacker,(Unit)target and (int)adjusted hitrate.
    void OnHitRateStatusCheck(object sender, object args)
    {
        Info<Unit, Unit, int> info = (Info<Unit, Unit, int>)args;
        Unit owner = GetComponentInParent<Unit>();
        
        // As this effects a unit
        // it will be attached to 1 of the 
        // units otherwise it shouldn't exist.
        // Attacker blind
        if(owner == info.arg0)
        {
            info.arg2 += 50;
        }
        // Defender blind
        else if(owner == info.arg1)
        {
            info.arg2 -= 20;
        }
    }
}
