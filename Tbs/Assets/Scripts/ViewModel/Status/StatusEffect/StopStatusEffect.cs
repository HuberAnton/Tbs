using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopStatusEffect : StatusEffect
{
    Stats myStats;

    private void OnEnable()
    {
        myStats = GetComponentInParent<Stats>();
        if (myStats)
            this.AddObserver(OnCounterWillChange,
                Stats.WillChangeNotification(StatTypes.CTR), myStats);
        this.AddObserver(OnAutomaticHitCheck, HitRate.AutomaticHitCheckNotification);
    }

    private void OnDisable()
    {
        this.RemoveObserver(OnCounterWillChange,
            Stats.WillChangeNotification(StatTypes.CTR), myStats);
        this.RemoveObserver(OnAutomaticHitCheck, HitRate.AutomaticHitCheckNotification);
    }


    void OnCounterWillChange(object sender, object args)
    {
        ValueChangeException exc = (ValueChangeException)args;
        // Note that nothing needs to be returned as the object passed in
        // is not local. 
        // Flipping the toggle stops turns from occuring but CTR may still increase.
        exc.FlipToggle();
    }

    void OnAutomaticHitCheck(object sender, object args)
    {
        Unit owner = GetComponentInParent<Unit>();
        MatchException exc = (MatchException)args;
        if (owner == exc.toggle)
            exc.FlipToggle();
    }

}
